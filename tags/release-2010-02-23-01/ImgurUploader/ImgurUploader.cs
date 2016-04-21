using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

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
                    hr.Headers.Add("Authorization", ConfigurationManager.AppSettings["ApiKey"]);
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
                    dynamic responsejson = JsonConvert.DeserializeObject(responseString);
                    if (responsejson.success == true)
                        {
                            FileComplete(this, new ImgurUploadInfo
                            {
                                imgur_page = responsejson.data.link,
                                delete_hash = "https://api.imgur.com/3/image/"+responsejson.data.deletehash,
                                file = file
                            });
                        }
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
        public string delete_hash { get; set; }
        public string imgur_page { get; set; }
        public string file { get; set; }

        public string DumpDetails(string delimiter)
        {
            return String.Format("Imgur Page: {1}{0}Delete Page: {4}",
                delimiter,
                this.imgur_page,
                this.delete_hash);
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
