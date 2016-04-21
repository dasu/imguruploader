using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Threading;

namespace ImgurUploader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            string[] submittedFiles = new string[] {};
            if (args.Length != 0) submittedFiles = args;
            else
            { // show a file chooser dialog
                OpenFileDialog fd = new OpenFileDialog()
                {
                    Multiselect = true,
                    AutoUpgradeEnabled=true,
                    Filter = "Images(*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp"
                };
                fd.ShowDialog();
                submittedFiles = fd.FileNames;
            }

            List<string> validFileExtensions = new List<string> { "png", "jpg", "jpeg", "bmp", "gif" };

            var validFileNames =
                from a in submittedFiles
                where
                    File.Exists(a) &&
                    validFileExtensions.Contains(Path.GetExtension(a).ToLower().Remove(0, 1))
                select a;

            if (validFileNames.Count() == 0)
            { MessageBox.Show(ConfigurationManager.AppSettings["Message_NoValidFiles"]); return; }
                        
            //make our progress form
            ImgurUploader i = new ImgurUploader { Files = new List<string>(validFileNames) };

            i.UpdateStatus += new ImgurUploader.UploadStatusHandler(i_UpdateStatus);
            i.InvalidFileFound += new ImgurUploader.InvalidFileHandler(i_InvalidFileFound);
            i.FileComplete += new ImgurUploader.FileCompleteHandler(i_FileComplete);

            _f = new ProgressForm();

            _uploadThread = new Thread(i.UploadFiles);
            _uploadThread.Start();
            
            Application.Run(_f);
        }

        static void i_FileComplete(object source, ImgurUploadInfo s)
        {
            //show a message with a link to the imgur page
            _f.AddLink(s);
        }

        static void i_InvalidFileFound(object source, string file, string message)
        {
            //should really do something here...
        }

        static Thread _uploadThread;

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            _uploadThread.Abort();
        }

        static ProgressForm _f;

        static void i_UpdateStatus(object source, ImgurUploaderStatus s)
        {
            _f.FilesMessage = s.TotalMessage;
            _f.FilesProgress = s.TotalProgress;
            _f.UploadMessage = s.FileBeingProcessed + " " + s.FileProcessMessage;
            _f.UploadProgress = s.FileProcessProgress;
        }
    }
}
