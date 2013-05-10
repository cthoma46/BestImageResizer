//#define THREADED_PROCESSING

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Accusoft.ImagXpressSdk;
using System.Text.RegularExpressions;

namespace BestImageResizer
{
    public partial class ResizerForm : Form
    {      
        #region Private Variables

        private ImagXpress imagXPress;
        private double[] zoomFactors = new double[] {.1,.25,.333,.5,.666,.75,1.0,2.0,3.0,4.0,5.0,6.0,8.0,10.0};
        private int nextZoomFactor = 6;
        private int prevZoomFactor = 5;
        private ImageX image = null;
        private Processor downSampleProc = null;
        private ImageX downSampleImage = null;
        private string filename;
        Action<int, bool> setProgress;
        Action<string> setLabel;
        Action<ImageX> setImage;
        private Thread hideProgressThread = null;
        private Thread downSampleThread = null;
        private Thread multiPageThread = null;
        private Range masterRange = null;

        #endregion

        #region Constructor and Destructor

        public ResizerForm()
        {
            InitializeComponent();

            imagXPress = new ImagXpress();

            fd = new OpenFileDialog();
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.Title = "Open Multipage Tiff";
            fd.Filter = "Tiff Files(*.TIF,*.TIFF)|*.TIF;*.TIFF";

            setProgress = delegate(int progress, bool visible) {
                this.progressBar1.Visible = visible;
                this.progressBar1.Value = progress;
            };
            
            setLabel = delegate(string x) 
            { 
                this.label1.Text = x; 
            };

            setImage = delegate(ImageX image)
            {
                this.imageXView.Image = image;
            };

            imageXView.ZoomFactorChanged += imageXView_ZoomFactorChanged;
        }

        protected override void Dispose( bool disposing )
	    {
		    if( disposing )
		    {
		    	if (components != null) 
		    	{
		    		components.Dispose();
		    	}
                if (image != null)
                {
                    image.Dispose();
                }
                if (downSampleImage != null)
                {
                    downSampleImage.Dispose();
                }
                if (downSampleProc != null)
                {
                    if (downSampleProc.Image != null)
                    {
                        downSampleProc.Image.Dispose();
                    }
                    downSampleProc.Dispose();
                }
		    }
		    base.Dispose( disposing );
        }

        #endregion

        #region Image Handling

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (fd.ShowDialog() == DialogResult.OK)
            {
                filename = fd.FileName;
                Thread thr = new Thread(OpenFile);
                thr.Priority = ThreadPriority.Normal;
                thr.Name = "File Open Thread";
                thr.Start();

            }
        }

        private void DownsampleBtn_Click(object sender, EventArgs e)
        {
            DownsampleBtn.Enabled = false;

            if (downSampleThread != null)
            {
                if (downSampleThread.IsAlive)
                {
                    downSampleThread.Abort();
                    downSampleThread.Join();
                }

                downSampleThread = null;
            }

            masterRange = new Range(0, 100);
            downSampleThread = new Thread(DownSample);
            downSampleThread.Name = "Downsampling thread";
            downSampleThread.Priority = ThreadPriority.Normal;
            downSampleThread.Start();
        }

        private void OutToPNGBtn_Click(object sender, EventArgs e)
        {
            if (savePNGDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveOptions saveOpts = new SaveOptions();
                saveOpts.Format = ImageXFormat.Png;
                saveOpts.Png.Interlaced = false;
                saveOpts.Png.TransparencyMatch = TransparencyMatch.None;
                saveOpts.Png.Filter = PngFilter.Optimal;

                if (downSampleImage != null)
                    downSampleImage.Save(savePNGDialog.FileName, saveOpts);
                else
                    image.Save(savePNGDialog.FileName, saveOpts);

                //briefly display a file saved message
                Thread thr = new Thread(() => {
                    Match match = Regex.Match(savePNGDialog.FileName, @"(?<=\\)\w*\.\w{1,4}(?!\\)", RegexOptions.None);
                    this.label1.Invoke(setLabel, "Saved as " + match.Groups[0].Value);
                    
                    Thread.Sleep(5000);

                    match = Regex.Match(fd.FileName, @"(?<=\\)\w*\.\w{1,4}(?!\\)", RegexOptions.None);
                    this.label1.Invoke(setLabel, match.Groups[0].Value);
                });

                thr.Name = "PNG File Save Message Thread";
                thr.Priority = ThreadPriority.BelowNormal;
                thr.Start();
            }
        }

        private void OutToTiffBtn_Click(object sender, EventArgs e)
        {
            if (saveTIFFDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            if (multiPageThread != null)
            {
                if (multiPageThread.IsAlive)
                {
                    multiPageThread.Abort();
                    multiPageThread.Join(50);
                }
                multiPageThread = null;
            }

            this.pageNextBtn.Enabled = false;
            this.pagePrevBtn.Enabled = false;

            multiPageThread = new Thread(MultiPageExport);
            multiPageThread.Priority = ThreadPriority.AboveNormal;
            multiPageThread.Name = "Multipage Tiff Exporting Thread";
            multiPageThread.Start();
        }

        private void MultiPageExport()
        {
            //We need to cancel this thread, we need it.
            if(downSampleThread != null)
            {
                if(downSampleThread.IsAlive)
                {
                    downSampleThread.Abort();
                }
                //This resets the image viewer to the main image
                this.splitContainer1.Invoke((Action)delegate { RefreshPage(); });
            }


            int pages = image.PageCount;

            //Set up save options
            /*
            SaveOptions savePngOpts = new SaveOptions();
            savePngOpts.Format = ImageXFormat.Png;
            savePngOpts.Png.Interlaced = false;
            savePngOpts.Png.TransparencyMatch = TransparencyMatch.None;
            savePngOpts.Png.Filter = PngFilter.None;
            */

            SaveOptions saveTifOpts = new SaveOptions();
            saveTifOpts.Format = ImageXFormat.Tiff;
            saveTifOpts.Tiff.ColorSpace = ColorSpace.Rgb;
            saveTifOpts.Tiff.Compression = Compression.NoCompression;
            saveTifOpts.Tiff.MultiPage = true;

            
            //Get filename and path for operations
            Match match = Regex.Match(saveTIFFDialog.FileName, @"(?<=\\)\w*\.\w{1,4}(?!\\)", RegexOptions.None);
            string fileName = match.Groups[0].Value;

            this.label1.Invoke(setLabel, "Exporting to " + fileName);

            match = Regex.Match(saveTIFFDialog.FileName, @".*(?=\w*\.TIF(?!\\))", RegexOptions.None);
            string path = match.Groups[0].Value;

            try
            {
                //clear old tif or we might append to it
                try { System.IO.File.Delete(saveTIFFDialog.FileName); }
                catch (Exception e) { }//Incase of file not found

                //Iterate through the pages and insert one at a time
                for (int i = 0; i < pages; i++)
                {
                    //Set the page
                    image.Page = i + 1;

                    //Set the progress bar range for the downsampling thread
                    masterRange = new Range();
                    masterRange.outLow = (float)(100.0f * ((float)(5 * i) / (float)(5 * pages)));
                    masterRange.outHigh = (float)(100.0f * ((float)(5 * (i + 1) - 1) / (float)(5 * pages)));

                    //prepare to downsample selected page
                    downSampleThread = new Thread(DownSample);
                    downSampleThread.Priority = ThreadPriority.AboveNormal;
                    downSampleThread.Name = "Downsample Page " + image.Page.ToString() + " Thread";
                    downSampleThread.Start();
                    downSampleThread.Join();

                    //add the page to the output TIF
                    progressBar1.Invoke(setProgress, (int)Math.Floor(100.0f * ((float)(5 * (i + 1) - 1) / (float)(5 * pages))), true);
                    if (downSampleImage == null) return;

                    //we need to make the initial multipage TIF file on the first page
                    if (i == 0)
                    {
                        downSampleImage.Save(saveTIFFDialog.FileName, saveTifOpts);
                        //Set to false so we can append the pages
                        saveTifOpts.Tiff.MultiPage = false;
                    }
                    else
                    {
                        //saving as TIF because it refused to add PNG images to a TIF file
                        downSampleImage.Save(path + "temp.TIF", saveTifOpts);
                        ImageX.InsertPage(imagXPress, path + "temp.TIF", saveTIFFDialog.FileName, i + 1);
                    }

                    //Kill the temporary page, just in case
                    try { System.IO.File.Delete(path + "temp.TIF"); }
                    catch (Exception e) { }//Incase of file not found
                    
                    //clean up resources for next iteration
                    downSampleImage.Dispose();
                    downSampleImage = null;

                    progressBar1.Invoke(setProgress, (int)Math.Floor(100.0f * ((float)(5 * (i + 1)) / (float)(5 * pages))), true);
                }
            }
            catch (ImageXException ex)
            {
                try { System.IO.File.Delete(saveTIFFDialog.FileName); }
                catch (Exception e) { }//Incase of file not found
                MessageBox.Show(ex.Message, "Tif Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(downSampleImage != null) downSampleImage.Dispose();
                try { System.IO.File.Delete(path + "temp.TIF"); }
                catch (Exception e) { }

                //Progress par is done now
                hideProgressBar();

                //Reset to the original image again
                image.Page = 1;

                //reenable page controls
                this.splitContainer1.Invoke((Action)delegate
                {
                    this.pageNextBtn.Enabled = true;
                    this.pagePrevBtn.Enabled = false;
                    RefreshPage();
                });

                //Say we exported successfully for 5 secs, then return to normal label
                Thread thr = new Thread(() =>
                {

                    this.label1.Invoke(setLabel, "Exported to " + fileName);

                    Thread.Sleep(5000);

                    match = Regex.Match(fd.FileName, @"(?<=\\)\w*\.\w{1,4}(?!\\)", RegexOptions.None);
                    this.label1.Invoke(setLabel, match.Groups[0].Value);
                });

                thr.Name = "TIF File Export Message Thread";
                thr.Priority = ThreadPriority.BelowNormal;
                thr.Start();
            }
        }

        private void OpenFile()
        {
            
            //Image is not loaded
            this.Invoke(setProgress, 0, true);

            image = ImageX.FromFile(imagXPress, filename);
            //changes to image automatically redraw
            image.AutoInvalidate = true;

            //Image is totally loaded
            this.Invoke(setProgress,100, true);

            //Set label to open image
            Match match = Regex.Match(fd.FileName,@"(?<=\\)\w*\.\w{1,4}(?!\\)",RegexOptions.None);
            this.label1.Invoke(setLabel, match.Groups[0].Value);

            this.imageXView.Invoke((Action)(() => { 
                imageXView.Image = image;
                imageXView.Refresh();

                //Enable controls only when image is loaded
                DownsampleBtn.Enabled = true;
                OutToPNGBtn.Enabled = true;
                
                //set up buttons for multipage vs single page TIF
                if (image.PageCount > 1)
                {
                    image.Page = 1;
                    pagePrevBtn.Visible = true;
                    pageNextBtn.Visible = true;
                    pagePrevBtn.Enabled = false;
                    pageNextBtn.Enabled = true;
                    pageLabel.Visible = true;
                    OutToTiffBtn.Enabled = true;
                }
                else
                {
                    pagePrevBtn.Visible = false;
                    pageNextBtn.Visible = false;
                    pagePrevBtn.Enabled = false;
                    pageNextBtn.Enabled = false;
                    OutToTiffBtn.Enabled = false;
                    pageLabel.Visible = false;
                    pageLabel.Text = "";
                }

                RefreshPage();

                //Set up zoom controls, set FitWidth default zoom
                SetZoom(0.0, AutoResizeType.FitWidth);
                zoomBestFitBtn.Enabled = true;
                zoomLabel.Enabled = true;
                zoomInBtn.Enabled = true;
                zoomOutBtn.Enabled = true;
                zoomFitWidthBtn.Enabled = true;
            }));

            //hides the progress bar after processing is complete
            hideProgressBar();
        }

        private float scaleTo(float input, Range range)
        {
            float inRange = range.inHigh - range.inLow;
            float outRange = range.outHigh - range.outLow;

            float normalized = (input - range.inLow) / inRange;
            return normalized * outRange + range.outLow;
        }

        private void DownSample()
        {
            
            if (image != null)
            {
                Range range = new Range(masterRange.outLow, masterRange.outHigh);
                bool notDone = true;

                Thread thr = new Thread(delegate() {
                    while(notDone)
                    {
                        lock (range)
                        {
                            progressBar1.Invoke(setProgress, (int)Math.Floor(scaleTo(downSampleProc.ProgressPercent, range)), true);
                        }
                        Thread.Sleep(5);
                    }
                });

                thr.Name = "Progress Bar Updater";

                try
                {
                    range.outLow = scaleTo(0, masterRange);
                    range.outHigh = scaleTo(25, masterRange);

                    downSampleProc = new Processor(imagXPress, image.Copy());
                    thr.Start();
                    //Console.WriteLine("Start progressbar thread");
                    
                    //to 24 bits
                    lock (range)
                    {
                        range.outLow = scaleTo(26, masterRange);
                        range.outHigh = scaleTo(50, masterRange);
                    }

                    #if THREADED_PROCESSING
                    Thread executionThread = new Thread(() =>
                    {
                        #endif
                        downSampleProc.ColorDepth(24, PaletteType.Fixed, DitherType.Accusoft);
                        #if THREADED_PROCESSING
                    });
                    executionThread.Start();
                    #endif

                    //Console.WriteLine("Sample to 24 bits done");
                    #if THREADED_PROCESSING
                    executionThread.Join();
                    #endif

                    //downscale
                    lock (range)
                    {
                        range.outLow = scaleTo(51, masterRange);
                        range.outHigh = scaleTo(75, masterRange);
                    }

                    #if THREADED_PROCESSING
                    executionThread = new Thread(() =>
                    {
                        #endif                
                        System.Drawing.Size newSize = new System.Drawing.Size((int)Math.Round(0.6 * image.Width), (int)Math.Round(0.6 * image.Height));
                        downSampleProc.Resize(newSize, ResizeType.Quality);
                        #if THREADED_PROCESSING
                    });
                    executionThread.Start();
                    #endif

                    
                    //Console.WriteLine("Downscale to 60% done");
                    #if THREADED_PROCESSING
                    executionThread.Join();
                    #endif

                    //to 4 bits
                    lock (range)
                    {
                        range.outLow = scaleTo(76, masterRange);
                        range.outHigh = scaleTo(100, masterRange);
                    }

                    #if THREADED_PROCESSING
                    executionThread = new Thread(() =>
                    {
                        #endif
                        downSampleProc.ColorDepth(4, PaletteType.Gray, DitherType.Accusoft);
                        #if THREADED_PROCESSING
                    });
                    executionThread.Start();
                    #endif

                    //Console.WriteLine("Sample to 4 bits done");
                    #if THREADED_PROCESSING
                    executionThread.Join();
                    #endif

                    imageXView.Invoke(setImage, downSampleProc.Image.Copy());
                    Thread.Sleep(50);

                    notDone = false;
                    thr.Join(50);
                    this.progressBar1.Invoke(setProgress, 100, true);
                    

                    downSampleImage = downSampleProc.Image.Copy();
                    if (masterRange.outHigh == 100)
                    {
                        hideProgressBar();
                        
                    }
                }
                catch (ProcessorException ex)
                {
                    
                    MessageBox.Show(ex.Message, "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (ImageXException ex)
                {
                    MessageBox.Show(ex.Message, "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        #endregion

        #region Zoom Controls

        void imageXView_ZoomFactorChanged(object sender, ZoomFactorChangedEventArgs e)
        {
            if (imageXView.Image == null) return;

            zoomLabel.Text = "Zoom: " + Math.Round(imageXView.ZoomFactor * 100).ToString() + "%";

            switch (imageXView.AutoResize)
            {
                case AutoResizeType.CropImage:
                    
                    break;
                case AutoResizeType.BestFit:
                    zoomLabel.Text += "\nBest Fit";
                    break;
                case AutoResizeType.FitWidth:
                    zoomLabel.Text += "\nFit Width";
                    break;
            }
        }

        private void SetZoom(double zoom, AutoResizeType autoResize)
        {
            if (zoom > 0.0)
            {
                //set the zoom factor directly
                this.imageXView.AutoResize = AutoResizeType.CropImage;
                this.imageXView.ZoomFactor = zoom;
            }
            else
            {
                //Set the auto resizing
                this.imageXView.AutoResize = autoResize;
                
                //Determine the closest zoom factors for zoom in and out based on the current zoom
                //and enable or disable buttons accordingly
                int i;
                for (i = 0; (Math.Round(100.0 * imageXView.ZoomFactor) > Math.Round(100.0 * zoomFactors[i]) && i < zoomFactors.Length - 1); ++i);
                    
                if (i == 0)
                {
                    zoomInBtn.Enabled = true;
                    zoomOutBtn.Enabled = false;
                    prevZoomFactor = 0;
                    if (Math.Round(zoomFactors[i] * 100.0) == Math.Round(imageXView.ZoomFactor * 100.0))
                    {
                        nextZoomFactor = 1;
                    }
                    else
                    {
                        nextZoomFactor = 0;
                    }
                }
                else if (i + 1 == zoomFactors.Length)
                {
                    if (imageXView.ZoomFactor < zoomFactors[i])
                    {
                        prevZoomFactor = i - 1;
                        nextZoomFactor = i;
                        zoomInBtn.Enabled = true;
                        zoomOutBtn.Enabled = true;
                    }
                    else if (imageXView.ZoomFactor > zoomFactors[i])
                    {
                        prevZoomFactor = i;
                        nextZoomFactor = i;
                        zoomInBtn.Enabled = false;
                        zoomOutBtn.Enabled = true;
                    }
                    else
                    {
                        prevZoomFactor = i - 1;
                        nextZoomFactor = i;
                        zoomInBtn.Enabled = false;
                        zoomOutBtn.Enabled = true;
                    }
                }
                else
                {
                    zoomInBtn.Enabled = true;
                    zoomOutBtn.Enabled = true;
                    nextZoomFactor = i;
                    if ((Math.Round(100.0 * zoomFactors[i]) == Math.Round(imageXView.ZoomFactor * 100.0)) && i + 1 < zoomFactors.Length) nextZoomFactor += 1;
                    prevZoomFactor = i - 1;
                }
            }
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            //Set new zoom
            double newZoom = zoomFactors[prevZoomFactor];
            nextZoomFactor = prevZoomFactor + 1;

            //Determine new zoom factors and which buttons to enable or disable
            if (prevZoomFactor == 0)
            {
                zoomOutBtn.Enabled = false;
            }
            else
            {
                prevZoomFactor -= 1;
            }

            if (nextZoomFactor >= zoomFactors.Length)
            {
                zoomInBtn.Enabled = false;
                nextZoomFactor = zoomFactors.Length - 1;
            }
            else
            {
                zoomInBtn.Enabled = true;
            }

            SetZoom(newZoom, AutoResizeType.CropImageToControl);
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            //Set new zoom
            double newZoom = zoomFactors[nextZoomFactor];
            prevZoomFactor = nextZoomFactor - 1;

            //Determine new zoom factors and which buttons to enable or disable
            if (nextZoomFactor == zoomFactors.Length - 1)
            {
                zoomInBtn.Enabled = false;
            }
            else
            {
                nextZoomFactor = nextZoomFactor + 1;
            }

            if (prevZoomFactor < 0)
            {
                zoomOutBtn.Enabled = false;
                prevZoomFactor = 0;
            }
            else
            {
                zoomOutBtn.Enabled = true;
            }

            SetZoom(newZoom, AutoResizeType.CropImageToControl);
        }

        private void zoomBestFitBtn_Click(object sender, EventArgs e)
        {
            SetZoom(0.0, AutoResizeType.BestFit);
        }

        private void zoomFitWidthBtn_Click(object sender, EventArgs e)
        {
            SetZoom(0.0, AutoResizeType.FitWidth);
        }

        #endregion

        #region Page Controls

        private void pageNextBtn_Click(object sender, EventArgs e)
        {
            image.Page += 1;
            if(image.Page == 2) pagePrevBtn.Enabled = true;
            if(image.Page == image.PageCount) pageNextBtn.Enabled = false;

            RefreshPage();
        }

        private void pagePrevBtn_Click(object sender, EventArgs e)
        {
            image.Page -= 1;
            if (image.Page == 1) pagePrevBtn.Enabled = false;
            if (image.Page + 1 == image.PageCount) pageNextBtn.Enabled = true;

            RefreshPage();
        }

        private void RefreshPage()
        {
            imageXView.Image = image;
            pageLabel.Text = "Page " + image.Page.ToString() + " of " + image.PageCount.ToString();

            if (downSampleThread != null)
            {
                if (downSampleThread.IsAlive)
                {
                    downSampleThread.Abort();
                    downSampleThread.Join();
                }

                downSampleThread = null;
            }

            DownsampleBtn.Enabled = true;
            if (downSampleProc != null)
            {
                if (downSampleProc.Image != null)
                {
                    downSampleProc.Image.Dispose();
                }
                downSampleProc.Dispose();
                downSampleProc = null;
            }
            if (downSampleImage != null)
            {
                downSampleImage.Dispose();
                downSampleImage = null;
            }

        }

        #endregion

        private void hideProgressBar()
        {
            if (hideProgressThread != null && hideProgressThread.IsAlive)
            {
                hideProgressThread.Abort();
                hideProgressThread.Join();
            }
            hideProgressThread = new Thread(() => {
                Thread.Sleep(2000);
                this.progressBar1.Invoke(setProgress, 0, false);
            });
            hideProgressThread.Priority = ThreadPriority.BelowNormal;
            hideProgressThread.Start();
        }

        private class Range
        {
            public float inLow = 0;
            public float inHigh = 100;
            public float outLow = 0;
            public float outHigh = 0;

            public Range()
            {
            }

            public Range(float outLow, float outHigh)
            {
                this.outLow = outLow;
                this.outHigh = outHigh;
            }
        }    
    }
}