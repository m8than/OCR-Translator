using IronOcr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCR_Translator
{
    public partial class frmMain : Form
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        private Point _startPos;
        private Point _endPos;
        private bool _startProcessing = false;

        private Point startPos
        {
            get
            {
                return _startPos;
            }
            set
            {
                _startPos = value;
                lblStart.Text = value.X + ", " + value.Y;
            }
        }
        private Point endPos
        {
            get
            {
                return _endPos;
            }
            set
            {
                _endPos = value;
                lblEnd.Text = value.X + ", " + value.Y;
            }
        }

        private bool startProcessing
        {
            get
            {
                return _startProcessing;
            }
            set
            {
                _startProcessing = value;
                if (value)
                {
                    btnStartStop.Text = "Stop";
                }
                else
                {
                    btnStartStop.Text = "Start";
                }
            }
        }

        private Form ocrCover;
        private Label ocrLabel;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnSelectScreen_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bitmap bg = ScreenProcessingHelper.getVirtualScreen();

            Form image = new Form();
            image.BackgroundImage = bg;
            image.Opacity = 1;
            image.StartPosition = FormStartPosition.Manual;
            image.Location = new Point(0, 0);

            image.Size = new Size(bg.Width, bg.Height);

            image.FormBorderStyle = FormBorderStyle.None;
            image.Visible = false;
            image.TopMost = false;

            /*
            Label lblTest = new Label();
            lblTest.Location = new Point(10, 10);
            lblTest.Text = "lol";
            test.Controls.Add(lblTest);
            */

            Form selector = new Form();
            selector.Opacity = 0.4;
            selector.BackColor = Color.Black;
            selector.StartPosition = FormStartPosition.Manual;
            selector.Location = new Point(0, 0);

            selector.Size = new Size(0, 0);

            selector.FormBorderStyle = FormBorderStyle.None;
            selector.Visible = false;
            selector.TopMost = true;


            image.Show();

            //waiting for left click originally pressed for this button
            while ((GetAsyncKeyState(0x01) & 0x8000) > 0)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            //while key not down
            while ((GetAsyncKeyState(0x01) & 0x8000) < 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            //when key pressed move overlay form and save
            POINT curStartPos = new POINT();
            POINT curEndPos = new POINT();

            GetCursorPos(out curStartPos);

            image.Hide();
            IntPtr window = ScreenProcessingHelper.getWindowFromPoint(curStartPos);
            image.Show();

            selector.Show();
            selector.Location = new Point(curStartPos.X, curStartPos.Y);

            ScreenProcessingHelper.RECT windowSize = ScreenProcessingHelper.getWindowRect(window);
            int maxWidth = windowSize.x2 - curStartPos.X;
            int maxHeight = windowSize.y2 - curStartPos.Y;

            int minX = windowSize.x1;
            int minY = windowSize.y1;

            //while key is still down
            while ((GetAsyncKeyState(0x01) & 0x8000) > 0)
            {
                GetCursorPos(out curEndPos);

                if (curEndPos.X < minX) curEndPos.X = minX;
                if (curEndPos.Y < minY) curEndPos.Y = minY;

                int width = curEndPos.X - curStartPos.X;
                int height = curEndPos.Y - curStartPos.Y;

                if (width > maxWidth) width = maxWidth;
                if (height > maxHeight) height = maxHeight;

                if (width > 0 && height > 0) {
                    selector.Size = new Size(width, height);
                }
                else if (width > 0 && height <= 0)
                {
                    //if drag right and up
                    height = curStartPos.Y - curEndPos.Y;
                    selector.Location = new Point(curStartPos.X, curEndPos.Y);
                    selector.Size = new Size(width, height);
                }
                else if (width <= 0 && height > 0)
                {
                    //if drag left and down
                    width = curStartPos.X - curEndPos.X;
                    selector.Location = new Point(curEndPos.X, curStartPos.Y);
                    selector.Size = new Size(width, height);
                }
                else
                {
                    //if drag left and up
                    height = curStartPos.Y - curEndPos.Y;
                    width = curStartPos.X - curEndPos.X;
                    selector.Location = new Point(curEndPos.X, curEndPos.Y);
                    selector.Size = new Size(width, height);
                }

                Application.DoEvents();
                Thread.Sleep(1);
            }

            image.Close();
            selector.Close();

            this.startPos = new Point(selector.Location.X, selector.Location.Y);
            this.endPos = new Point(selector.Location.X + selector.Width, selector.Location.Y + selector.Height);
            this.Show();
        }

        private Bitmap imageFeed = null;
        private void updateFormBackground()
        {
            IntPtr window = ScreenProcessingHelper.getWindowFromPoint(this.startPos);
            while (this.startProcessing)
            {
                this.ocrCover.Location = this.startPos;
                this.ocrCover.Size = new Size(this.endPos.X - this.startPos.X, this.endPos.Y - this.startPos.Y);

                Point windowStartPos = ScreenProcessingHelper.screenPointToWindowPoint(window, this.startPos);
                Bitmap image = ScreenProcessingHelper.getWindowScreenshot(window, windowStartPos, this.ocrCover.Size);
                this.ocrCover.BackColor = ScreenProcessingHelper.getModalColour(image);
                //this.ocrCover.BackgroundImage = image;
                this.ocrCover.Visible = true;
                this.imageFeed = image;
                Application.DoEvents();

                Thread.Sleep(5);
            }
        }

        private void processOCR()
        {
            IronOcr.Languages.IOcrLanguagePack languages = new IronOcr.Languages.MultiLanguage(
                    IronOcr.Languages.English.OcrLanguagePack,
                    IronOcr.Languages.French.OcrLanguagePack
            );
            var Ocr = new AutoOcr()
            {
                Language = languages,
                ReadBarCodes = false
            };
            while (this.startProcessing)
            {
                if (this.imageFeed != null)
                {
                    OcrResult Results;
                    if (this.btnTextColour.BackColor == Control.DefaultBackColor)
                    {
                        Results = Ocr.Read(this.imageFeed);
                    }
                    else
                    {
                        Bitmap processed = ScreenProcessingHelper.filterColour(this.imageFeed, this.btnTextColour.BackColor);
                        Results = Ocr.Read(processed);
                    }

                    Debug.WriteLine(Results.Text);
                    if (Results.Text.Count() > 0)
                    {
                        string translation = Vikings.Translate.GoogleTranslate.Translate(Results.Text, "auto", "English");
                        try
                        {
                            this.ocrLabel.Invoke((MethodInvoker)delegate
                            {
                                this.ocrLabel.ForeColor = Color.FromArgb(this.ocrCover.BackColor.ToArgb() ^ 0xffffff); // inverts
                                this.ocrLabel.Text = translation;
                                this.ocrLabel.Font = new Font(Results.FontName, (float)(Results.FontSize / 3), FontStyle.Bold, GraphicsUnit.Pixel);
                                this.ocrLabel.Visible = true;
                            });
                        }
                        catch { }
                    }

                    Application.DoEvents();
                }
                Thread.Sleep(1000);
            }
        }
        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            this.startProcessing = !this.startProcessing;
            this.ocrCover = new Form
            {
                Opacity = 1,
                StartPosition = FormStartPosition.Manual,
                FormBorderStyle = FormBorderStyle.None,
                TopMost = true,
                Visible = false
            };
            
            this.ocrLabel = new Label
            {
                Location = new Point(0, 0),
                AutoSize = false,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                Visible = true,
                BackColor = Color.Transparent
            };

            this.ocrCover.Controls.Add(this.ocrLabel);

            if (this.startProcessing)
            {
                Thread tProcessOCR = new Thread(processOCR);
                tProcessOCR.IsBackground = true;
                tProcessOCR.Start();

                Thread tUpdateFormBackground = new Thread(updateFormBackground);
                tUpdateFormBackground.IsBackground = true;
                tUpdateFormBackground.Start();
            }
        }

        private void BtnTextColour_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bitmap bg = ScreenProcessingHelper.getVirtualScreen();

            Form image = new Form();
            image.BackgroundImage = bg;
            image.Opacity = 1;
            image.StartPosition = FormStartPosition.Manual;
            image.Location = new Point(0, 0);

            image.Size = new Size(bg.Width, bg.Height);

            image.FormBorderStyle = FormBorderStyle.None;
            image.Visible = false;
            image.TopMost = false;

            image.Show();

            //waiting for left click originally pressed for this button
            while ((GetAsyncKeyState(0x01) & 0x8000) > 0)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            //while key not down
            while ((GetAsyncKeyState(0x01) & 0x8000) < 1)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }

            //when key pressed move overlay form and save
            POINT pos = new POINT();
            GetCursorPos(out pos);

            btnTextColour.BackColor = bg.GetPixel(pos.X, pos.Y);
            image.Close();
            this.Show();
        }
    }
}
