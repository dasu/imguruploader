using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using System.Xml;

namespace ImgurUploader
{
    public class ImgurUploader
    {
        public delegate void UploadStatusHandler(object source, ImgurUploaderStatus s);
        public event UploadStatusHandler UpdateStatus;

        public delegate void InvalidFileHandler(object source, string file, string message);
        public event InvalidFileHandler InvalidFileFound;

        public delegate void FileCompleteHandler(object source, ImgurUploadInfo s);
        public event FileCompleteHandler FileComplete;

        public List<string> Files { get; set; }

        public void UploadFiles()
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("key", ConfigurationManager.AppSettings["APIKey"]);

            int currentFileNumber = 0;
            foreach (string file in Files)
            {
                currentFileNumber++;
                string TotalMessage = String.Format("Dealing with file {0}/{1}", currentFileNumber, Files.Count);
                double TotalProgress = (currentFileNumber - 1) / (Files.Count * 1.0f);
                ImgurUploaderStatus status = 
                    new ImgurUploaderStatus { 
                        TotalMessage = TotalMessage, 
                        TotalProgress = TotalProgress,
                        FileBeingProcessed = Path.GetFileName(file)
                    };
                try
                {
                    status.FileProcessProgress = 0;
                    
                    status.FileProcessMessage = "Verifying existence";
                    UpdateStatus(this, status);

                    if (!File.Exists(file)) 
                        throw new InvalidFileException { Message = "File could not be found" };

                    status.FileProcessMessage = "Verifying file integrity";
                    UpdateStatus(this, status);
                    try
                    { Image i = Image.FromFile(file); }
                    catch (ArgumentException) { throw new InvalidFileException { Message = "File did not appear to be an image" }; }

                    //okee - moving on! now lets compose a webrequest and send it along it's merry way.                    

                    HttpWebRequest hr = WebRequest.Create(ConfigurationManager.AppSettings["UploadURL"]) as HttpWebRequest;
                    string bound = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                    hr.ContentType = "multipart/form-data; boundary=" + bound;
                    hr.Method = "POST";
                    hr.KeepAlive = true;
                    hr.Credentials = CredentialCache.DefaultCredentials;

                    byte[] boundBytes = Encoding.ASCII.GetBytes("\r\n--" + bound + "\r\n");
                    string formDataTemplate = "\r\n--" + bound + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                    //add fields + a boundary
                    MemoryStream fieldData = new MemoryStream();
                    foreach (string key in fields.Keys)
                    {
                        byte[] formItemBytes = Encoding.UTF8.GetBytes(
                                string.Format(formDataTemplate, key, fields[key]));
                        fieldData.Write(formItemBytes, 0, formItemBytes.Length);
                    }
                    fieldData.Write(boundBytes, 0, boundBytes.Length);

                    //calculate the total length we expect to send
                    string headerTemplate =
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";
                    long fileBytes = 0;
                    
                    byte[] headerBytes = Encoding.UTF8.GetBytes(
                            String.Format(headerTemplate, "image", file));
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);

                    fileBytes += headerBytes.Length;
                    fileBytes += fs.Length;
                    fileBytes += boundBytes.Length;
                    hr.ContentLength = fieldData.Length + fileBytes;

                    Stream s = hr.GetRequestStream();
                    //write the fields to the request stream
                    Debug.WriteLine("sending field data");
                    fieldData.WriteTo(s);

                    //write the files to the request stream
                    Debug.WriteLine("sending file data");
                    status.FileProcessMessage = "Sending File...";
                    UpdateStatus(this, status);
                    s.Write(headerBytes, 0, headerBytes.Length);

                    int bytesRead = 0;
                    long bytesSoFar = 0;
                    byte[] buffer = new byte[10240];
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        bytesSoFar += bytesRead;
                        s.Write(buffer, 0, bytesRead);
                        Debug.WriteLine(String.Format("sending file data {0:0.000}%", (bytesSoFar * 100.0f) / fs.Length));
                        status.FileProcessProgress = (bytesSoFar * 1.0f) / fs.Length;
                        status.TotalProgress = TotalProgress + 1 / (Files.Count * 1.0f) * status.FileProcessProgress;
                        UpdateStatus(this, status);
                    }

                    s.Write(boundBytes, 0, boundBytes.Length);
                    fs.Close();
                    s.Close();

                    status.FileProcessMessage = "File Sent, Waiting for Response...";
                    UpdateStatus(this, status);

                    HttpWebResponse response = hr.GetResponse() as HttpWebResponse;
                    string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Debug.Write(responseString);

                    status.FileProcessMessage = "All finished, what's next?";
                    status.FileProcessProgress = 1;
                    UpdateStatus(this, status);

                    XmlDocument responseXml = new XmlDocument();
                    responseXml.LoadXml(responseString);
                    if (responseXml["rsp"].Attributes["stat"].Value == "ok")
                        FileComplete(this, new ImgurUploadInfo
                        {
                            imgur_page = responseXml["rsp"]["imgur_page"].InnerText,
                            original_image = responseXml["rsp"]["original_image"].InnerText,
                            small_thumbnail = responseXml["rsp"]["small_thumbnail"].InnerText,
                            large_thumbnail = responseXml["rsp"]["large_thumbnail"].InnerText,
                            delete_page = responseXml["rsp"]["delete_page"].InnerText,
                            delete_hash = responseXml["rsp"]["delete_hash"].InnerText,
                            image_hash = responseXml["rsp"]["image_hash"].InnerText,
                            file = file
                        });
                    else
                        throw new InvalidFileException { Message = "Imgur didn't like it" };

                    hr = null;
                }
                catch (InvalidFileException ex)
                {
                    InvalidFileFound(this, file, ex.Message);
                }
            }

            UpdateStatus(this, new ImgurUploaderStatus
            {
                FileBeingProcessed = "All Finished",
                FileProcessMessage = "",
                FileProcessProgress = 1,
                TotalMessage = "All Files Done",
                TotalProgress = 1
            });
        }
    }

    [Serializable]
    public class ImgurUploadInfo
    {
        public string image_hash { get; set; }
        public string delete_hash { get; set; }
        public string original_image { get; set; }
        public string large_thumbnail { get; set; }
        public string small_thumbnail { get; set; }
        public string imgur_page { get; set; }
        public string delete_page { get; set; }
        public string file { get; set; }

        public string Format(TextFormatType type, ImageFormatSize size, bool includeLinkToOriginal)
        {
            string thumnailToUse = this.original_image;
            switch(size)
            {
                case ImageFormatSize.Large:
                    thumnailToUse = this.large_thumbnail;
                    break;
                case ImageFormatSize.Small:
                    thumnailToUse = this.small_thumbnail;
                    break;
            }

            switch (type)
            {
                case TextFormatType.DirectLink:
                    return String.Format("{0}", this.original_image);
                case TextFormatType.BBCode:
                    return includeLinkToOriginal ? 
                        String.Format("[URL={0}][IMG]{1}[/IMG][/URL]", this.original_image, thumnailToUse) : 
                        String.Format("[IMG]{0}[/IMG]", thumnailToUse);
                case TextFormatType.Html:
                    return includeLinkToOriginal ?
                        String.Format("<a href=\"{0}\" title=\"Hosted by imgur\"><img src=\"{1}\" /></a>", this.original_image, thumnailToUse) :
                        String.Format("<img src=\"{0}\" />", thumnailToUse);
                case TextFormatType.MarkdownLink:
                    return String.Format("[%linktext%]({0})", this.imgur_page);
                case TextFormatType.StackOverflowEmbeddable:
                    return String.Format("![%alttext%]({0})", thumnailToUse);
            }

            return this.original_image;
        }

        public string DumpDetails(string delimiter)
        {
            return String.Format("Imgur Page: {1}{0}Large Thumb: {2}{0}Small Thumb: {3}{0}Delete Page: {4}",
                delimiter,
                this.imgur_page,
                this.large_thumbnail,
                this.small_thumbnail,
                this.delete_page);
        }
    }

    public enum TextFormatType
    {
        DirectLink,Html,BBCode,MarkdownLink,StackOverflowEmbeddable
    }

    public enum ImageFormatSize
    {
        Small,Large,Original
    }

    public class ImgurUploaderStatus
    {
        public string FileBeingProcessed { get; set; }
        public string FileProcessMessage { get; set; }
        public double FileProcessProgress { get; set; }

        public string TotalMessage { get; set; }
        public double TotalProgress { get; set; }
    }

    public class InvalidFileException : Exception
    {
        public string Message { get; set; }
    }
}
