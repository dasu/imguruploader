using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace ImgurUploader
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
            fileListProgressBar.Maximum = uploadProgressBar.Maximum = 1000;
        }

        /// <summary>
        /// 0-1.0 representation of our progress through this current upload
        /// </summary>
        public double UploadProgress { get; set; }
        public string UploadMessage { get; set; }

        /// <summary>
        /// 0-1.0 representation of our progress through the list of files to upload
        /// </summary>
        public double FilesProgress { get; set; }
        public string FilesMessage { get; set; }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            fileListProgressBar.Value = (int)Math.Floor(FilesProgress * 1000);
            uploadProgressBar.Value = (int)Math.Floor(UploadProgress * 1000);
            
            fileListPercent.Text = String.Format("{0:0}%", FilesProgress * 100);
            uploadPercent.Text = String.Format("{0:0}%", UploadProgress * 100);

            fileListName.Text = FilesMessage;
            uploadName.Text = UploadMessage;
        }

        delegate void AddLinkDel(ImgurUploadInfo u);
        public void AddLink(ImgurUploadInfo u)
        {
            if (this.InvokeRequired)
            {
                AddLinkDel d = new AddLinkDel(AddLink);
                this.Invoke(d, new object[] { u });
                return;
            }

            Uploaded.Add(u);
            AddLabelAndLink(u.file, "", false);
            AddLabelAndLink("Imgur Page:", u.imgur_page, true);
            AddLabelAndLink("Original:", u.original_image, true);
            AddLabelAndLink("Large Thumb:", u.large_thumbnail, true);
            AddLabelAndLink("Small Thumb:", u.small_thumbnail, true);
            AddLabelAndLink("Delete Link:", u.delete_page, true);
        }

        List<ImgurUploadInfo> Uploaded = new List<ImgurUploadInfo>();

        private void AddLabelAndLink(string label, string link, bool pad)
        {
            Label t = new Label
            {
                Text = label,
                Height= 14,
                AutoSize = (!pad),
                Padding = (pad) ? new Padding(25,0,0,0) : new Padding(0,5,0,0),
                Width = 100
            };
            linksPanel.Controls.Add(t);

            LinkLabel l = new LinkLabel
            {
                Text = link,
                AutoSize = true,
                Padding = new Padding(0),
            };
            l.Click += new EventHandler((object s, EventArgs e) => { Process.Start(link); });
            linksPanel.Controls.Add(l);
            linksPanel.SetFlowBreak(l, true);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clipboardCopyLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Uploaded == null || Uploaded.Count == 0) return;
            string clipTxt = "";

            /*
            Html List (Large Thumnails)
            Html List (Small Thumnails)
            Message Board List (BBCode) (Large Thumbs)
            Message Board List (BBCode) (Small Thumbs)
            Message Board Lines (BBCode) (Large Thumbs)
            Message Board Lines (BBCode) (Small Thumbs)
            Markdown List (Reddit friendly)
            Simple List (Imgur Pages)
            Simple List (Originals)
            Detail List
             */
            switch (formatCombo.SelectedItem.ToString())
            {
                case "Html List (Large Thumnails)":
                    clipTxt += "<ul>";
                    Uploaded.ForEach(u => clipTxt += String.Format("<li>{0}</li>", 
                        u.Format(TextFormatType.Html, ImageFormatSize.Large, linkCheck.Checked)));
                    clipTxt += "<ul>";
                    break;
                case "Html List (Small Thumnails)":
                    clipTxt += "<ul>";
                    Uploaded.ForEach(u => clipTxt += String.Format("<li>{0}</li>",
                        u.Format(TextFormatType.Html, ImageFormatSize.Small, linkCheck.Checked)));
                    clipTxt += "<ul>";
                    break;
                case "Message Board List (BBCode) (Large Thumbs)":
                    clipTxt += "[LIST]";
                    Uploaded.ForEach(u => clipTxt += String.Format("[*]{0}",
                        u.Format(TextFormatType.BBCode, ImageFormatSize.Large, linkCheck.Checked)));
                    clipTxt += "[/LIST]";
                    break;
                case "Message Board List (BBCode) (Small Thumbs)":
                    clipTxt += "[LIST]";
                    Uploaded.ForEach(u => clipTxt += String.Format("[*]{0}",
                        u.Format(TextFormatType.BBCode, ImageFormatSize.Small, linkCheck.Checked)));
                    clipTxt += "[/LIST]";
                    break;
                case "Message Board Lines (BBCode) (Large Thumbs)":
                    Uploaded.ForEach(u => clipTxt += String.Format("{0}\n",
                        u.Format(TextFormatType.BBCode, ImageFormatSize.Large, linkCheck.Checked)));
                    break;
                case "Message Board Lines (BBCode) (Small Thumbs)":
                    Uploaded.ForEach(u => clipTxt += String.Format("{0}\n",
                        u.Format(TextFormatType.BBCode, ImageFormatSize.Small, linkCheck.Checked)));
                    break;
                case "Markdown List (Reddit friendly)":
                    //doesn't care about image size, links to imgur page
                    Uploaded.ForEach(u => clipTxt += String.Format("* {0}",
                        u.Format(TextFormatType.MarkdownLink, ImageFormatSize.Small, linkCheck.Checked)));
                    break;
                case "Simple List (Imgur Pages)":
                    Uploaded.ForEach(u => clipTxt += String.Format("{0}\n",
                        u.imgur_page));
                    break;
                case "Simple List (Originals)":
                    Uploaded.ForEach(u => clipTxt += String.Format("{0}\n",
                        u.original_image));
                    break;
                default:
                    foreach (ImgurUploadInfo u in Uploaded)
                    {
                        clipTxt += "\r\nFile: " + Path.GetFileName(u.file);
                        clipTxt += "\r\n-----------------------------\r\n";
                        clipTxt += u.DumpDetails("\r\n");
                        clipTxt += "\r\n";
                    }
                    break;
            }

            Clipboard.SetText(clipTxt.Trim());
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            formatCombo.SelectedIndex = 0;
            linkCheck.Checked = true;
            this.BringToFront();
        }
    }
}
