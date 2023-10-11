using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
#if WIN32
using System.Windows.Forms.Design;
#endif
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSLibrary.Windows
{
    /// <summary>
    /// ScrollBarEx control
    /// </summary>
    public class HScrollBarEx : Control
    {
        #region Fields ==============================================================================
        private enum State
        {
            NONE = -1,
            UP_ARROW = 1,
            DOWN_ARROW,
            SCROLL_WIDGET,
            BACK_UP,
            BACK_DOWN
        };
        private Point m_ClickPos = new Point();
        private State m_State = State.NONE;
        //Timer for mouse down
        System.Windows.Forms.Timer m_Timer = null;
        // Double Buffer
        private Bitmap bufferBitmap = null;
        private Graphics bufferGraphics = null;

        private bool disposed = false;

        private Color moChannelColor = Color.LimeGreen;
        private Image moLeftArrowImage = null;
        //private Image moUpArrowImage_Over = null;
        //private Image moUpArrowImage_Down = null;
        private Image moRightArrowImage = null;
        //private Image moDownArrowImage_Over = null;
        //private Image moDownArrowImage_Down = null;
        //private Image moThumbArrowImage = null;
        private Image moThumbLeftImage = null;
        private Image moThumbLeftSpanImage = null;
        private Image moThumbRightImage = null;
        private Image moThumbRightSpanImage = null;
        private Image moThumbMiddleImage = null;

        private int moLargeChange = 10;
        private int moSmallChange = 1;
        private int moMinimum = 0;
        private int moMaximum = 100;
        private int moValue = 0;
        private int nClickPoint;

        private int moThumbTop = 0;

       // private bool moAutoSize = false;

        private bool moThumbDown = false;
        private bool moThumbDragging = false;
        private Size mMinimumSize = new Size();
        /*/// <summary>
        /// Scroll event trigger
        /// </summary>
        public event EventHandler Scroll = null;*/
        /// <summary>
        /// Value change event trigger
        /// </summary>
        public event EventHandler ValueChanged = null;
        #endregion

        #region Properties ==========================================================================
        /// <summary>
        /// Summary:
        ///     Gets or sets a value to be added to or subtracted from the System.Windows.Forms.ScrollBar.Value
        ///     property when the scroll box is moved a large distance.
        ///
        /// Returns:
        ///     A numeric value. The default value is 10.
        ///
        /// Exceptions:
        ///   System.ArgumentOutOfRangeException:
        ///     The assigned value is less than 0.
        /// </summary>
        public int LargeChange
        {
            get { return moLargeChange; }
            set
            {
                moLargeChange = value;
                Invalidate();
            }
        }

        /// <summary>
        ///
        /// Summary:
        ///     Gets or sets the value to be added to or subtracted from the System.Windows.Forms.ScrollBar.Value
        ///     property when the scroll box is moved a small distance.
        ///
        /// Returns:
        ///     A numeric value. The default value is 1.
        ///
        /// Exceptions:
        ///   System.ArgumentOutOfRangeException:
        ///     The assigned value is less than 0.
        /// </summary>
        public int SmallChange
        {
            get { return moSmallChange; }
            set
            {
                moSmallChange = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Summary:
        ///     Gets or sets the lower limit of values of the scrollable range.
        ///
        /// Returns:
        ///     A numeric value. The default value is 0.
        /// </summary>
        public int Minimum
        {
            get { return moMinimum; }
            set
            {
                moMinimum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Summary:
        ///     Gets or sets the upper limit of values of the scrollable range.
        ///
        /// Returns:
        ///     A numeric value. The default value is 100.
        /// </summary>
        public int Maximum
        {
            get { return moMaximum; }
            set
            {
                moMaximum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Summary:
        ///     Gets or sets a numeric value that represents the current position of the
        ///     scroll box on the scroll bar control.
        ///
        /// Returns:
        ///     A numeric value that is within the System.Windows.Forms.ScrollBar.Minimum
        ///     and System.Windows.Forms.ScrollBar.Maximum range. The default value is 0.
        ///
        /// Exceptions:
        ///   System.ArgumentOutOfRangeException:
        ///     The assigned value is less than the System.Windows.Forms.ScrollBar.Minimum
        ///     property value.-or- The assigned value is greater than the System.Windows.Forms.ScrollBar.Maximum
        ///     property value.        
        /// </summary>
        public int Value
        {
            get { return moValue; }
            set
            {
                moValue = value;

                int nTrackWidth = (this.Width - (LeftArrowImage.Width + RightArrowImage.Width));
                float fThumbWidth = ((float)LargeChange / (float)Maximum) * nTrackWidth;
                int nThumbWidth = (int)fThumbWidth;

                if (nThumbWidth > nTrackWidth)
                {
                    nThumbWidth = nTrackWidth;
                    fThumbWidth = nTrackWidth;
                }
                if (nThumbWidth < 56)
                {
                    nThumbWidth = 56;
                    fThumbWidth = 56;
                }

                //figure out value
                int nPixelRange = nTrackWidth - nThumbWidth;
                int nRealRange = (Maximum - Minimum) - LargeChange;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)moValue / (float)nRealRange;

                }

                float fTop = fPerc * nPixelRange;
                moThumbTop = (int)fTop;

                Invalidate();
            }
        }

        /// <summary>
        /// Get or Set channel color
        /// </summary>
        public Color ChannelColor
        {
            get { return moChannelColor; }
            set
            {
                moChannelColor = value;
                //Invalidate();
            }
        }

        /// <summary>
        /// Get or Set left arrow image
        /// </summary>
        public Image LeftArrowImage
        {
            get { return moLeftArrowImage; }
            set { moLeftArrowImage = value; }
        }

        /// <summary>
        /// Get or Set right arrow image
        /// </summary>
        public Image RightArrowImage
        {
            get { return moRightArrowImage; }
            set { moRightArrowImage = value; }
        }

        /// <summary>
        /// Get or Set thumb left image
        /// </summary>
        public Image ThumbLeftImage
        {
            get { return moThumbLeftImage; }
            set { moThumbLeftImage = value; }
        }

        /// <summary>
        /// Get or Set left span image
        /// </summary>
        public Image ThumbLeftSpanImage
        {
            get { return moThumbLeftSpanImage; }
            set { moThumbLeftSpanImage = value; }
        }

        /// <summary>
        /// Get or Set thumb bottom image
        /// </summary>
        public Image ThumbRightImage
        {
            get { return moThumbRightImage; }
            set { moThumbRightImage = value; }
        }

        /// <summary>
        /// Get or Set thumb bottom span image
        /// </summary>
        public Image ThumbRightSpanImage
        {
            get { return moThumbRightSpanImage; }
            set { moThumbRightSpanImage = value; }
        }

        /// <summary>
        /// Get or Set thumb middle image
        /// </summary>
        public Image ThumbMiddleImage
        {
            get { return moThumbMiddleImage; }
            set { moThumbMiddleImage = value; }
        }
        /// <summary>
        /// Get Minimum size of the control
        /// </summary>
        public Size MinimumSize
        {
            get { return mMinimumSize; }
            //set { mMinimumSize = value; }
        }
        #endregion

        #region Methods =============================================================================
        /// <summary>
        /// Constructor
        /// </summary>
        public HScrollBarEx()
        {
            this.Name = "ScrollbarEx";
#if WIN32
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
#endif
            moChannelColor = Color.FromArgb(51, 166, 3);
            LeftArrowImage = CSLibrary.Windows.Properties.Resources.HLeftArrow;
            RightArrowImage = CSLibrary.Windows.Properties.Resources.HRightArrow;


            ThumbRightImage = CSLibrary.Windows.Properties.Resources.HThumbBottom;
            ThumbRightSpanImage = CSLibrary.Windows.Properties.Resources.HThumbSpanBottom;
            ThumbLeftImage = CSLibrary.Windows.Properties.Resources.HThumbTop;
            ThumbLeftSpanImage = CSLibrary.Windows.Properties.Resources.HThumbSpanTop;
            ThumbMiddleImage = CSLibrary.Windows.Properties.Resources.HThumbMiddle;

            this.mMinimumSize = new Size(LeftArrowImage.Width + RightArrowImage.Width + GetThumbWidth(), LeftArrowImage.Height + 1);
            this.Size = new Size(100, LeftArrowImage.Height + 1);
        }
        /// <summary>
		/// Allows an instance of the BatteryLife class to attempt to free resources and perform other cleanup operations.
		/// </summary>
        ~HScrollBarEx()
		{
			Dispose(false);
		}
		/// <summary>
		/// Releases all resources used by the BatteryLife instance.
		/// </summary>
		public new void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// Raises the CSLibrary.Windows.ScrollBarEx.ValueChanged event.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
        /*/// <summary>
        /// Raises the CSLibrary.Windows.ScrollBarEx.Scroll event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnScroll(EventArgs e)
        {
            if (Scroll != null)
                Scroll(this, e);
        }*/

        /// <summary>
        /// Releases the unmanaged resources used by the BatteryLife instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    if (disposing)
                    {
                        // Dispose managed resources.
                        if (m_Timer != null)
                        {
                            m_Timer.Enabled = false;
                            m_Timer.Dispose();
                        }
                        this.bufferBitmap.Dispose();
                        this.bufferGraphics.Dispose();
                    }
                    // Dispose unmanaged resources.
                    this.disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
        /// <summary>
        /// Raises the Resize event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            bool resizeWidth = false, resizeHeight = false;
            if (MinimumSize.Width > Width)
            {
                resizeWidth = true;
            }

            if(MinimumSize.Height > Height)
            {
                resizeHeight = true;
            }
            if (resizeWidth || resizeHeight)
            {
                this.Size = new Size(resizeWidth ? MinimumSize.Width : Width, resizeHeight ? MinimumSize.Height : Height);
                return;
            }

            if (this.bufferGraphics != null)
            {
                this.bufferGraphics.Dispose();
            }

            if (this.bufferBitmap != null)
            {
                this.bufferBitmap.Dispose();
            }

            this.bufferBitmap = new Bitmap(Width, Height);
            this.bufferGraphics = Graphics.FromImage(this.bufferBitmap);

            this.Invalidate();

            base.OnResize(e);
        }
        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing with the hope of preventing noticeable flicker.
        }
        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            IntPtr dc = bufferGraphics.GetHdc();
            IntPtr memBitmapDC = GdiPlus.CreateCompatibleDC(dc);

            IntPtr oBrush = GdiPlus.CreateSolidBrush(GdiPlus.ColorRef.From(moChannelColor));
            IntPtr oWhiteBrush = GdiPlus.CreateSolidBrush(GdiPlus.ColorRef.From(Color.FromArgb(255, 255, 255)));

            //Brush oBrush = new SolidBrush(moChannelColor);
            //Brush oWhiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));

            //draw channel
            GdiPlus.RECT oBrushRect = new GdiPlus.RECT(0, 0, this.Size);
            IntPtr old_brush = GdiPlus.SelectObject(dc, oBrush);
            GdiPlus.FillRect(dc, ref oBrushRect, oBrush);
            //bufferGraphics.FillRectangle(oBrush, new Rectangle(0, 0, this.Width, this.Height));

            //draw channel top and down border colors
            GdiPlus.RECT oWhiteBrushRectLeft = new GdiPlus.RECT(0, 0, new Size(this.Width, 1));
            GdiPlus.RECT oWhiteBrushRectRight = new GdiPlus.RECT(0, Height - 1, new Size(this.Width, 1));
            GdiPlus.SelectObject(dc, oWhiteBrush);
            GdiPlus.FillRect(dc, ref oWhiteBrushRectLeft, oWhiteBrush);
            GdiPlus.FillRect(dc, ref oWhiteBrushRectRight, oWhiteBrush);
            //bufferGraphics.FillRectangle(oWhiteBrush, new Rectangle(0, 0, this.Width, 1));
            //bufferGraphics.FillRectangle(oWhiteBrush, new Rectangle(0, Height - 1, this.Width, 1));

            //draw left arrow
            if (LeftArrowImage != null)
            {
                //bufferGraphics.DrawImage(LeftArrowImage,0,0);// /*new Rectangle(0, 0, LeftArrowImage.Width, Height), new Rectangle(0, 0, LeftArrowImage.Width, LeftArrowImage.Height)*/, GraphicsUnit.Pixel);
                GdiPlus.SelectObject(memBitmapDC, ((Bitmap)LeftArrowImage).GetHbitmap());
                GdiPlus.StretchBlt(dc, 0, 0, LeftArrowImage.Width, Height, memBitmapDC, 0, 0, LeftArrowImage.Width, LeftArrowImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);
            }

            //draw thumb
            int nTrackWidth = (this.Width - (LeftArrowImage.Width + RightArrowImage.Width));
            float fThumbWidth = ((float)LargeChange / (float)Maximum) * nTrackWidth;
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < 56)
            {
                nThumbWidth = 56;
                fThumbWidth = 56;
            }

            //Debug.WriteLine(nThumbHeight.ToString());

            float fSpanWidth = (fThumbWidth - (ThumbMiddleImage.Width + ThumbLeftImage.Width + ThumbRightImage.Width)) / 2.0f;
            int nSpanWidth = (int)fSpanWidth;

            int nTop = moThumbTop;
            nTop += LeftArrowImage.Width;

            //draw top
            //bufferGraphics.DrawImage(ThumbLeftImage, new Rectangle(nTop, 1, ThumbLeftImage.Width, this.Height - 2), new Rectangle(0, 0, ThumbLeftImage.Width, ThumbLeftImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbLeftImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, nTop, 1, ThumbLeftImage.Width, this.Height - 2, memBitmapDC, 0, 0, ThumbLeftImage.Width, ThumbLeftImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += ThumbLeftImage.Width;
            //draw top span
            //Rectangle rect = new Rectangle(nTop, 1, (int)(fSpanWidth * 2), this.Height - 2);

            //bufferGraphics.DrawImage(ThumbLeftSpanImage, rect, new Rectangle(0, 0, ThumbLeftSpanImage.Width, ThumbLeftSpanImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbLeftSpanImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, nTop, 1, (int)(fSpanWidth * 2), this.Height - 2, memBitmapDC, 0, 0, ThumbLeftSpanImage.Width, ThumbLeftSpanImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += nSpanWidth;
            //draw middle
            //bufferGraphics.DrawImage(ThumbMiddleImage, new Rectangle(nTop, 1, ThumbMiddleImage.Width, this.Height - 2), new Rectangle(0, 0, ThumbMiddleImage.Width, ThumbMiddleImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbMiddleImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, nTop, 1, ThumbMiddleImage.Width, this.Height - 2, memBitmapDC, 0, 0, ThumbMiddleImage.Width, ThumbMiddleImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += ThumbMiddleImage.Width;
            //draw bottom span
            //rect = new Rectangle(nTop, 1, nSpanWidth * 2, this.Height - 2);
            //bufferGraphics.DrawImage(ThumbRightSpanImage, rect, new Rectangle(0, 0, ThumbRightSpanImage.Width, ThumbRightSpanImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbRightSpanImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, nTop, 1, nSpanWidth, this.Height - 2, memBitmapDC, 0, 0, ThumbRightSpanImage.Width, ThumbRightSpanImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += nSpanWidth;
            //draw bottom
            //bufferGraphics.DrawImage(ThumbRightImage, new Rectangle(nTop, 1, nSpanWidth, this.Height - 2), new Rectangle(0, 0, ThumbRightImage.Width, ThumbRightImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbRightImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, nTop, 1, ThumbRightImage.Width, this.Height - 2, memBitmapDC, 0, 0, ThumbRightImage.Width, ThumbRightImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            if (RightArrowImage != null)
            {
                //bufferGraphics.DrawImage(RightArrowImage, new Rectangle(this.Width - RightArrowImage.Width, 0, RightArrowImage.Width, this.Height), new Rectangle(0, 0, RightArrowImage.Width, RightArrowImage.Height), GraphicsUnit.Pixel);
                GdiPlus.SelectObject(memBitmapDC, ((Bitmap)RightArrowImage).GetHbitmap());
                GdiPlus.StretchBlt(dc, this.Width - RightArrowImage.Width, 0, RightArrowImage.Width, this.Height, memBitmapDC, 0, 0, RightArrowImage.Width, RightArrowImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);
            }
            
            e.Graphics.DrawImage(this.bufferBitmap, 0, 0);

            GdiPlus.SelectObject(dc, old_brush);
            //release resources
            GdiPlus.DeleteDC(memBitmapDC);
            GdiPlus.DeleteObject(oWhiteBrush);
            GdiPlus.DeleteObject(oBrush);
            bufferGraphics.ReleaseHdc(dc);

            base.OnPaint(e);
        }
#if WIN32
        public override bool AutoSize {
            get {
                return base.AutoSize;
            }
            set {
                base.AutoSize = value;
                if (base.AutoSize) {
                    this.Width = moUpArrowImage.Width;
                }
            }
        }
#endif
        /// <summary>
        /// override OnMouseDown
        /// </summary>
        /// <param name="e"></param>
        protected override void  OnMouseDown(MouseEventArgs e)
        {
            Debug.WriteLine(String.Format("MouseDown Position X={0}, Y={1}", e.X, e.Y));
            Point ptPoint = new Point(e.X, e.Y);// this.PointToClient(new Point(e.X, e.Y)/*Cursor.Position*/);
            int nTrackWidth = (this.Width - (LeftArrowImage.Width + RightArrowImage.Width));
            float fThumbWidth = ((float)LargeChange / (float)Maximum) * nTrackWidth;
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < 56)
            {
                nThumbWidth = 56;
                fThumbWidth = 56;
            }

            int nTop = moThumbTop;
            nTop += LeftArrowImage.Width;

            Rectangle thumbrect = new Rectangle(nTop, 1, nThumbWidth, Height);
            Rectangle uparrowrect = new Rectangle(0, 1, LeftArrowImage.Width, Height);
            Rectangle downarrowrect = new Rectangle(LeftArrowImage.Width + nTrackWidth, 1, LeftArrowImage.Width, Height);
            Rectangle backrect = new Rectangle(LeftArrowImage.Width, 1, this.Width - LeftArrowImage.Width - RightArrowImage.Width, Height);

            if (thumbrect.Contains(ptPoint))
            {
                m_State = State.SCROLL_WIDGET;

                Debug.WriteLine(string.Format("thumbrect{0}.{1}.{2}.{3} @ hit the thumb", 1, nTop, ThumbMiddleImage.Width, nThumbWidth));
                //hit the thumb
                nClickPoint = (ptPoint.X - nTop);
                //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                this.moThumbDown = true;
            }
            else if (uparrowrect.Contains(ptPoint))
            {
                m_State = State.UP_ARROW;

                int nRealRange = (Maximum - Minimum) - LargeChange;
                int nPixelRange = (nTrackWidth - nThumbWidth);
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if ((moThumbTop - SmallChange) < 0)
                            moThumbTop = 0;
                        else
                            moThumbTop -= SmallChange;

                        //figure out value
                        float fPerc = (float)moThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);

                        moValue = (int)fValue;
                        Debug.WriteLine(moValue.ToString());

                        OnValueChanged(EventArgs.Empty);

                        //OnScroll(EventArgs.Empty);

                        Invalidate();
                    }
                }
            }
            else if (downarrowrect.Contains(ptPoint))
            {
                m_State = State.DOWN_ARROW;

                int nRealRange = (Maximum - Minimum) - LargeChange;
                int nPixelRange = (nTrackWidth - nThumbWidth);
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if ((moThumbTop + SmallChange) > nPixelRange)
                            moThumbTop = nPixelRange;
                        else
                            moThumbTop += SmallChange;

                        //figure out value
                        float fPerc = (float)moThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);

                        moValue = (int)fValue;
                        Debug.WriteLine(moValue.ToString());

                        OnValueChanged(EventArgs.Empty);

                        //OnScroll(EventArgs.Empty);

                        Invalidate();
                    }
                }
            }
            else if (backrect.Contains(ptPoint))
            {
                if (ptPoint.X < thumbrect.X)
                {
                    m_State = State.BACK_UP;

                    int nRealRange = (Maximum - Minimum) - LargeChange;
                    int nPixelRange = (nTrackWidth - nThumbWidth);
                    if (nRealRange > 0)
                    {
                        if (nPixelRange > 0)
                        {
                            if ((moThumbTop - LargeChange) < 0)
                                moThumbTop = 0;
                            else
                                moThumbTop -= LargeChange;

                            //figure out value
                            float fPerc = (float)moThumbTop / (float)nPixelRange;
                            float fValue = fPerc * (Maximum - LargeChange);

                            moValue = (int)fValue;
                            Debug.WriteLine(moValue.ToString());

                            OnValueChanged(EventArgs.Empty);

                            //OnScroll(EventArgs.Empty);

                            Invalidate();
                        }
                    }

                }
                else if (ptPoint.X >= thumbrect.X + thumbrect.Width)
                {
                    m_State = State.BACK_DOWN;
                    int nRealRange = (Maximum - Minimum) - LargeChange;
                    int nPixelRange = (nTrackWidth - nThumbWidth);
                    if (nRealRange > 0)
                    {
                        if (nPixelRange > 0)
                        {
                            if ((moThumbTop + LargeChange) > nPixelRange)
                                moThumbTop = nPixelRange;
                            else
                                moThumbTop += LargeChange;

                            //figure out value
                            float fPerc = (float)moThumbTop / (float)nPixelRange;
                            float fValue = fPerc * (Maximum - LargeChange);

                            moValue = (int)fValue;
                            Debug.WriteLine(moValue.ToString());

                            OnValueChanged(EventArgs.Empty);

                            //OnScroll(EventArgs.Empty);

                            Invalidate();
                        }
                    }
                }
                else
                {
                    m_State = State.NONE;
                }
            }
            else
            {
                m_State = State.NONE;
            }

            if (m_State != State.SCROLL_WIDGET)
            {
                if (m_Timer != null)
                {
                    m_Timer.Enabled = false;
                    m_Timer.Dispose();
                    m_Timer = null;
                }

                m_ClickPos = ptPoint;

                m_Timer = new System.Windows.Forms.Timer();
                m_Timer.Interval = 100;
                m_Timer.Tick += new EventHandler(m_Timer_Tick);
                m_Timer.Enabled = true;
            }

            base.OnMouseDown(e);

        }

        void m_Timer_Tick(object sender, EventArgs e)
        {

            int nTrackWidth = (this.Width - (LeftArrowImage.Width + RightArrowImage.Width));
            float fThumbWidth = ((float)LargeChange / (float)Maximum) * nTrackWidth;
            int nThumbWidth = (int)fThumbWidth;
            
            Rectangle uparrowrect = new Rectangle(0, 1, LeftArrowImage.Width, Height);
            Rectangle downarrowrect = new Rectangle(LeftArrowImage.Width + nTrackWidth, 1, LeftArrowImage.Width, Height);
            Rectangle backrect = new Rectangle(LeftArrowImage.Width, 1, this.Width - LeftArrowImage.Width - RightArrowImage.Width, Height);

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < 56)
            {
                nThumbWidth = 56;
                fThumbWidth = 56;
            }

            Point pos = m_ClickPos;

            switch (m_State)
            {
                case State.UP_ARROW:
                    {
                        if (uparrowrect.Contains(pos))
                        {
                            int nRealRange = (Maximum - Minimum) - LargeChange;
                            int nPixelRange = (nTrackWidth - nThumbWidth);
                            if (nRealRange > 0)
                            {
                                if (nPixelRange > 0)
                                {
                                    if ((moThumbTop - SmallChange) < 0)
                                        moThumbTop = 0;
                                    else
                                        moThumbTop -= SmallChange;

                                    //figure out value
                                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                                    float fValue = fPerc * (Maximum - LargeChange);

                                    moValue = (int)fValue;
                                    Debug.WriteLine(moValue.ToString());

                                    OnValueChanged(EventArgs.Empty);

                                    //OnScroll(EventArgs.Empty);

                                    Invalidate();
                                }
                            }
                        }
                    }
                    break;

                case State.DOWN_ARROW:
                    {
                        if (downarrowrect.Contains(pos))
                        {
                            int nRealRange = (Maximum - Minimum) - LargeChange;
                            int nPixelRange = (nTrackWidth - nThumbWidth);
                            if (nRealRange > 0)
                            {
                                if (nPixelRange > 0)
                                {
                                    if ((moThumbTop + SmallChange) > nPixelRange)
                                        moThumbTop = nPixelRange;
                                    else
                                        moThumbTop += SmallChange;

                                    //figure out value
                                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                                    float fValue = fPerc * (Maximum - LargeChange);

                                    moValue = (int)fValue;
                                    Debug.WriteLine(moValue.ToString());

                                    OnValueChanged(EventArgs.Empty);

                                    //OnScroll(EventArgs.Empty);

                                    Invalidate();
                                }
                            }
                        }
                    }
                    break;

                case State.SCROLL_WIDGET:
                    // Nothing should be done
                    break;

                case State.BACK_UP:
                    {
                        if (backrect.Contains(pos))
                        {
                            int nRealRange = (Maximum - Minimum) - LargeChange;
                            int nPixelRange = (nTrackWidth - nThumbWidth);
                            if (nRealRange > 0)
                            {
                                if (nPixelRange > 0)
                                {
                                    if ((moThumbTop - LargeChange) < 0)
                                        moThumbTop = 0;
                                    else
                                        moThumbTop -= LargeChange;

                                    //figure out value
                                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                                    float fValue = fPerc * (Maximum - LargeChange);

                                    moValue = (int)fValue;
                                    Debug.WriteLine(moValue.ToString());

                                    OnValueChanged(EventArgs.Empty);

                                    //OnScroll(EventArgs.Empty);

                                    Invalidate();
                                }
                            }
                        }
                    }
                    break;

                case State.BACK_DOWN:
                    {
                        if (backrect.Contains(pos))
                        {
                            int nRealRange = (Maximum - Minimum) - LargeChange;
                            int nPixelRange = (nTrackWidth - nThumbWidth);
                            if (nRealRange > 0)
                            {
                                if (nPixelRange > 0)
                                {
                                    if ((moThumbTop + LargeChange) > nPixelRange)
                                        moThumbTop = nPixelRange;
                                    else
                                        moThumbTop += LargeChange;

                                    //figure out value
                                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                                    float fValue = fPerc * (Maximum - LargeChange);

                                    moValue = (int)fValue;
                                    Debug.WriteLine(moValue.ToString());

                                    OnValueChanged(EventArgs.Empty);

                                    //OnScroll(EventArgs.Empty);

                                    Invalidate();
                                }
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// override OnMouseUp
        /// </summary>
        /// <param name="e"></param>
        //private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.moThumbDown = false;
            this.moThumbDragging = false;
            if (m_Timer != null)
            {
                m_ClickPos = new Point(-1, -1);
                m_Timer.Enabled = false;
                m_Timer.Dispose();
                m_Timer = null;
            }
            base.OnMouseUp(e);
        }
        /// <summary>
        /// override OnMouseMove
        /// </summary>
        /// <param name="e"></param>
        //private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        protected override void OnMouseMove(MouseEventArgs e)
        {
            m_ClickPos = new Point(e.X, e.Y);
            if (moThumbDown)
            {
                this.moThumbDragging = true;
            }

            if (this.moThumbDragging)
            {

                MoveThumb(e.X);

                OnValueChanged(EventArgs.Empty);
            }

            base.OnMouseMove(e);
        }
  
        private void MoveThumb(int x)
        {
            int nRealRange = Maximum - Minimum;
            int nTrackWidth = (this.Width - (LeftArrowImage.Width + RightArrowImage.Width));
            float fThumbWidth = ((float)LargeChange / (float)Maximum) * nTrackWidth;
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < 56)
            {
                nThumbWidth = 56;
                fThumbWidth = 56;
            }

            int nSpot = nClickPoint;

            int nPixelRange = (nTrackWidth - nThumbWidth);
            if (moThumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = x - (LeftArrowImage.Width + nSpot);

                    if (nNewThumbTop < 0)
                    {
                        moThumbTop = nNewThumbTop = 0;
                    }
                    else if (nNewThumbTop > nPixelRange)
                    {
                        moThumbTop = nNewThumbTop = nPixelRange;
                    }
                    else
                    {
                        moThumbTop = x - (LeftArrowImage.Width + nSpot);
                    }

                    //figure out value
                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                    float fValue = fPerc * (Maximum - LargeChange);
                    moValue = (int)fValue;
                    Debug.WriteLine(moValue.ToString());

                    //Application.DoEvents();

                    Invalidate();
                }
            }
        }
 
        private int GetThumbWidth()
        {
            int nTrackWidth = (this.Width - (LeftArrowImage.Width + RightArrowImage.Width));
            float fThumbWidth = ((float)LargeChange / (float)Maximum) * nTrackWidth;
            int nThumbWidth = (int)fThumbWidth;

            if (nThumbWidth > nTrackWidth)
            {
                nThumbWidth = nTrackWidth;
                fThumbWidth = nTrackWidth;
            }
            if (nThumbWidth < 56)
            {
                nThumbWidth = 56;
                fThumbWidth = 56;
            }

            return nThumbWidth;
        }
        #endregion

    }

#if WIN32_
    internal class ScrollbarControlDesigner : System.Windows.Forms.Design.ControlDesigner {

        

        public override SelectionRules SelectionRules {
            get {
                SelectionRules selectionRules = base.SelectionRules;
                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(this.Component)["AutoSize"];
                if (propDescriptor != null) {
                    bool autoSize = (bool)propDescriptor.GetValue(this.Component);
                    if (autoSize) {
                        selectionRules = SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable;
                    }
                    else {
                        selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
                    }
                }
                return selectionRules;
            }
        } 
    }
#endif
}