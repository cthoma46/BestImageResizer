namespace BestImageResizer
{
    partial class ResizerForm
    {

        #region Windows Form Designer generated code

        private System.ComponentModel.IContainer components;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResizerForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.zoomBox = new System.Windows.Forms.GroupBox();
            this.zoomFitWidthBtn = new System.Windows.Forms.Button();
            this.zoomLabel = new System.Windows.Forms.Label();
            this.zoomBestFitBtn = new System.Windows.Forms.Button();
            this.zoomInBtn = new System.Windows.Forms.Button();
            this.zoomOutBtn = new System.Windows.Forms.Button();
            this.pageLabel = new System.Windows.Forms.Label();
            this.pageNextBtn = new System.Windows.Forms.Button();
            this.pagePrevBtn = new System.Windows.Forms.Button();
            this.DownsampleBtn = new System.Windows.Forms.Button();
            this.OutToTiffBtn = new System.Windows.Forms.Button();
            this.OutToPNGBtn = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.imageXView = new Accusoft.ImagXpressSdk.ImageXView(this.components);
            this.imagXPress = new Accusoft.ImagXpressSdk.ImagXpress(this.components);
            this.fd = new System.Windows.Forms.OpenFileDialog();
            this.savePNGDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveTIFFDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.zoomBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.zoomBox);
            this.splitContainer1.Panel1.Controls.Add(this.pageLabel);
            this.splitContainer1.Panel1.Controls.Add(this.pageNextBtn);
            this.splitContainer1.Panel1.Controls.Add(this.pagePrevBtn);
            this.splitContainer1.Panel1.Controls.Add(this.DownsampleBtn);
            this.splitContainer1.Panel1.Controls.Add(this.OutToTiffBtn);
            this.splitContainer1.Panel1.Controls.Add(this.OutToPNGBtn);
            this.splitContainer1.Panel1.Controls.Add(this.BtnLoad);
            this.splitContainer1.Panel1MinSize = 90;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.imageXView);
            this.splitContainer1.Panel2MinSize = 390;
            this.splitContainer1.Size = new System.Drawing.Size(484, 376);
            this.splitContainer1.SplitterDistance = 90;
            this.splitContainer1.TabIndex = 0;
            // 
            // zoomBox
            // 
            this.zoomBox.Controls.Add(this.zoomFitWidthBtn);
            this.zoomBox.Controls.Add(this.zoomLabel);
            this.zoomBox.Controls.Add(this.zoomBestFitBtn);
            this.zoomBox.Controls.Add(this.zoomInBtn);
            this.zoomBox.Controls.Add(this.zoomOutBtn);
            this.zoomBox.Location = new System.Drawing.Point(3, 143);
            this.zoomBox.Name = "zoomBox";
            this.zoomBox.Size = new System.Drawing.Size(84, 147);
            this.zoomBox.TabIndex = 8;
            this.zoomBox.TabStop = false;
            this.zoomBox.Text = "Zoom";
            // 
            // zoomFitWidthBtn
            // 
            this.zoomFitWidthBtn.Enabled = false;
            this.zoomFitWidthBtn.Location = new System.Drawing.Point(7, 86);
            this.zoomFitWidthBtn.Name = "zoomFitWidthBtn";
            this.zoomFitWidthBtn.Size = new System.Drawing.Size(68, 23);
            this.zoomFitWidthBtn.TabIndex = 11;
            this.zoomFitWidthBtn.Text = "Fit Width";
            this.zoomFitWidthBtn.UseVisualStyleBackColor = true;
            this.zoomFitWidthBtn.Click += new System.EventHandler(this.zoomFitWidthBtn_Click);
            // 
            // zoomLabel
            // 
            this.zoomLabel.Enabled = false;
            this.zoomLabel.Location = new System.Drawing.Point(6, 112);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(69, 32);
            this.zoomLabel.TabIndex = 10;
            this.zoomLabel.Text = "Zoom: 66%\r\nFit Width";
            // 
            // zoomBestFitBtn
            // 
            this.zoomBestFitBtn.Enabled = false;
            this.zoomBestFitBtn.Location = new System.Drawing.Point(6, 56);
            this.zoomBestFitBtn.Name = "zoomBestFitBtn";
            this.zoomBestFitBtn.Size = new System.Drawing.Size(69, 23);
            this.zoomBestFitBtn.TabIndex = 9;
            this.zoomBestFitBtn.Text = "Best Fit";
            this.zoomBestFitBtn.UseVisualStyleBackColor = true;
            this.zoomBestFitBtn.Click += new System.EventHandler(this.zoomBestFitBtn_Click);
            // 
            // zoomInBtn
            // 
            this.zoomInBtn.Enabled = false;
            this.zoomInBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoomInBtn.Location = new System.Drawing.Point(44, 19);
            this.zoomInBtn.Name = "zoomInBtn";
            this.zoomInBtn.Size = new System.Drawing.Size(31, 31);
            this.zoomInBtn.TabIndex = 8;
            this.zoomInBtn.Text = "+";
            this.zoomInBtn.UseVisualStyleBackColor = true;
            this.zoomInBtn.Click += new System.EventHandler(this.zoomInBtn_Click);
            // 
            // zoomOutBtn
            // 
            this.zoomOutBtn.Enabled = false;
            this.zoomOutBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoomOutBtn.Location = new System.Drawing.Point(6, 19);
            this.zoomOutBtn.Name = "zoomOutBtn";
            this.zoomOutBtn.Size = new System.Drawing.Size(31, 31);
            this.zoomOutBtn.TabIndex = 7;
            this.zoomOutBtn.Text = "-";
            this.zoomOutBtn.UseVisualStyleBackColor = true;
            this.zoomOutBtn.Click += new System.EventHandler(this.zoomOutBtn_Click);
            // 
            // pageLabel
            // 
            this.pageLabel.AutoSize = true;
            this.pageLabel.Location = new System.Drawing.Point(8, 355);
            this.pageLabel.Name = "pageLabel";
            this.pageLabel.Size = new System.Drawing.Size(0, 13);
            this.pageLabel.TabIndex = 3;
            this.pageLabel.Visible = false;
            // 
            // pageNextBtn
            // 
            this.pageNextBtn.Enabled = false;
            this.pageNextBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageNextBtn.Image = global::BestImageResizer.Properties.Resources.ArrowRight;
            this.pageNextBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pageNextBtn.Location = new System.Drawing.Point(48, 296);
            this.pageNextBtn.Name = "pageNextBtn";
            this.pageNextBtn.Size = new System.Drawing.Size(39, 53);
            this.pageNextBtn.TabIndex = 6;
            this.pageNextBtn.Text = "Next";
            this.pageNextBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pageNextBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pageNextBtn.UseVisualStyleBackColor = true;
            this.pageNextBtn.Visible = false;
            this.pageNextBtn.Click += new System.EventHandler(this.pageNextBtn_Click);
            // 
            // pagePrevBtn
            // 
            this.pagePrevBtn.Enabled = false;
            this.pagePrevBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pagePrevBtn.Image = global::BestImageResizer.Properties.Resources.ArrowLeft;
            this.pagePrevBtn.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pagePrevBtn.Location = new System.Drawing.Point(3, 296);
            this.pagePrevBtn.Name = "pagePrevBtn";
            this.pagePrevBtn.Size = new System.Drawing.Size(39, 53);
            this.pagePrevBtn.TabIndex = 5;
            this.pagePrevBtn.Text = "Prev";
            this.pagePrevBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pagePrevBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pagePrevBtn.UseVisualStyleBackColor = true;
            this.pagePrevBtn.Visible = false;
            this.pagePrevBtn.Click += new System.EventHandler(this.pagePrevBtn_Click);
            // 
            // DownsampleBtn
            // 
            this.DownsampleBtn.Enabled = false;
            this.DownsampleBtn.Location = new System.Drawing.Point(3, 32);
            this.DownsampleBtn.Name = "DownsampleBtn";
            this.DownsampleBtn.Size = new System.Drawing.Size(84, 23);
            this.DownsampleBtn.TabIndex = 4;
            this.DownsampleBtn.Text = "Reduce";
            this.DownsampleBtn.UseVisualStyleBackColor = true;
            this.DownsampleBtn.Click += new System.EventHandler(this.DownsampleBtn_Click);
            // 
            // OutToTiffBtn
            // 
            this.OutToTiffBtn.Enabled = false;
            this.OutToTiffBtn.Location = new System.Drawing.Point(3, 90);
            this.OutToTiffBtn.Name = "OutToTiffBtn";
            this.OutToTiffBtn.Size = new System.Drawing.Size(84, 47);
            this.OutToTiffBtn.TabIndex = 2;
            this.OutToTiffBtn.Text = "Out to multipage Tiff";
            this.OutToTiffBtn.UseVisualStyleBackColor = true;
            this.OutToTiffBtn.Click += new System.EventHandler(this.OutToTiffBtn_Click);
            // 
            // OutToPNGBtn
            // 
            this.OutToPNGBtn.Enabled = false;
            this.OutToPNGBtn.Location = new System.Drawing.Point(3, 61);
            this.OutToPNGBtn.Name = "OutToPNGBtn";
            this.OutToPNGBtn.Size = new System.Drawing.Size(84, 23);
            this.OutToPNGBtn.TabIndex = 1;
            this.OutToPNGBtn.Text = "Out to PNG";
            this.OutToPNGBtn.UseVisualStyleBackColor = true;
            this.OutToPNGBtn.Click += new System.EventHandler(this.OutToPNGBtn_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(3, 3);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(84, 23);
            this.BtnLoad.TabIndex = 0;
            this.BtnLoad.Text = "Load TIFF";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(283, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(95, 18);
            this.progressBar1.TabIndex = 2;
            this.progressBar1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "No File Loaded";
            // 
            // imageXView
            // 
            this.imageXView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageXView.AutoResize = Accusoft.ImagXpressSdk.AutoResizeType.FitWidth;
            this.imageXView.BorderStyle = Accusoft.ImagXpressSdk.ImageXViewBorderStyle.FixedSingle;
            this.imageXView.Location = new System.Drawing.Point(3, 25);
            this.imageXView.Name = "imageXView";
            this.imageXView.Size = new System.Drawing.Size(384, 348);
            this.imageXView.TabIndex = 0;
            // 
            // fd
            // 
            this.fd.Filter = "Tiff Files(*.TIF,*.TIFF)|*.TIF;*.TIFF";
            this.fd.Title = "Open Multipage Tiff";
            // 
            // savePNGDialog
            // 
            this.savePNGDialog.DefaultExt = "PNG";
            this.savePNGDialog.FileName = "testimg.PNG";
            this.savePNGDialog.Filter = "PNG Images(*.PNG)|*.PNG";
            this.savePNGDialog.InitialDirectory = "C:\\";
            this.savePNGDialog.Title = "Save to PNG";
            // 
            // saveTIFFDialog
            // 
            this.saveTIFFDialog.DefaultExt = "tif";
            this.saveTIFFDialog.FileName = "testimg.TIF";
            this.saveTIFFDialog.Filter = "TIF Images(*.TIF)|*.TIF";
            this.saveTIFFDialog.InitialDirectory = "C:\\";
            this.saveTIFFDialog.Title = "Save as Multipage TIF";
            // 
            // ResizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(484, 376);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "ResizerForm";
            this.Text = "Best Image Resizer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.zoomBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button OutToTiffBtn;
        private System.Windows.Forms.Button OutToPNGBtn;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.OpenFileDialog fd;
        private Accusoft.ImagXpressSdk.ImageXView imageXView;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DownsampleBtn;
        private System.Windows.Forms.Label pageLabel;
        private System.Windows.Forms.Button pageNextBtn;
        private System.Windows.Forms.Button pagePrevBtn;
        private System.Windows.Forms.GroupBox zoomBox;
        private System.Windows.Forms.Button zoomInBtn;
        private System.Windows.Forms.Button zoomOutBtn;
        private System.Windows.Forms.Button zoomBestFitBtn;
        private System.Windows.Forms.Label zoomLabel;
        private System.Windows.Forms.Button zoomFitWidthBtn;
        private System.Windows.Forms.SaveFileDialog savePNGDialog;
        private System.Windows.Forms.SaveFileDialog saveTIFFDialog;

    }
}

