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


namespace CSLibrary.Windows
{
    /// <summary>
    /// ScrollBarEx control
    /// </summary>
    public class VScrollBarEx : Control
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
        private Image moUpArrowImage = null;
        //private Image moUpArrowImage_Over = null;
        //private Image moUpArrowImage_Down = null;
        private Image moDownArrowImage = null;
        //private Image moDownArrowImage_Over = null;
        //private Image moDownArrowImage_Down = null;
        //private Image moThumbArrowImage = null;
        private Image moThumbTopImage = null;
        private Image moThumbTopSpanImage = null;
        private Image moThumbBottomImage = null;
        private Image moThumbBottomSpanImage = null;
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

                int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
                float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
                int nThumbHeight = (int)fThumbHeight;

                if (nThumbHeight > nTrackHeight)
                {
                    nThumbHeight = nTrackHeight;
                    fThumbHeight = nTrackHeight;
                }
                if (nThumbHeight < 56)
                {
                    nThumbHeight = 56;
                    fThumbHeight = 56;
                }

                //figure out value
                int nPixelRange = nTrackHeight - nThumbHeight;
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
        /// Get or Set up arrow image
        /// </summary>
        public Image UpArrowImage
        {
            get { return moUpArrowImage; }
            set { moUpArrowImage = value; }
        }

        /// <summary>
        /// Get or Set down arrow image
        /// </summary>
        public Image DownArrowImage
        {
            get { return moDownArrowImage; }
            set { moDownArrowImage = value; }
        }

        /// <summary>
        /// Get or Set thumb top image
        /// </summary>
        public Image ThumbTopImage
        {
            get { return moThumbTopImage; }
            set { moThumbTopImage = value; }
        }

        /// <summary>
        /// Get or Set top span image
        /// </summary>
        public Image ThumbTopSpanImage
        {
            get { return moThumbTopSpanImage; }
            set { moThumbTopSpanImage = value; }
        }

        /// <summary>
        /// Get or Set thumb bottom image
        /// </summary>
        public Image ThumbBottomImage
        {
            get { return moThumbBottomImage; }
            set { moThumbBottomImage = value; }
        }

        /// <summary>
        /// Get or Set thumb bottom span image
        /// </summary>
        public Image ThumbBottomSpanImage
        {
            get { return moThumbBottomSpanImage; }
            set { moThumbBottomSpanImage = value; }
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
        public VScrollBarEx()
        {
            this.Name = "ScrollbarEx";
#if WIN32
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
#endif
            moChannelColor = Color.FromArgb(51, 166, 3);
            UpArrowImage = CSLibrary.Windows.Properties.Resources.VUpArrow;
            DownArrowImage = CSLibrary.Windows.Properties.Resources.VDownArrow;


            ThumbBottomImage = CSLibrary.Windows.Properties.Resources.VThumbBottom;
            ThumbBottomSpanImage = CSLibrary.Windows.Properties.Resources.VThumbSpanBottom;
            ThumbTopImage = CSLibrary.Windows.Properties.Resources.VThumbTop;
            ThumbTopSpanImage = CSLibrary.Windows.Properties.Resources.VThumbSpanTop;
            ThumbMiddleImage = CSLibrary.Windows.Properties.Resources.VThumbMiddle;

            this.mMinimumSize = new Size(UpArrowImage.Width + 1, UpArrowImage.Height + DownArrowImage.Height + GetThumbHeight());
            this.Size = new Size(UpArrowImage.Width + 1, 100);
        }
        /// <summary>
		/// Allows an instance of the BatteryLife class to attempt to free resources and perform other cleanup operations.
		/// </summary>
        ~VScrollBarEx()
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
            //bufferGraphics.FillRectangle(oBrush, new Rectangle(1, UpArrowImage.Height, this.Width - 2, (this.Height - DownArrowImage.Height)));

            //draw channel left and right border colors
            GdiPlus.RECT oWhiteBrushRectLeft = new GdiPlus.RECT(0, UpArrowImage.Height, new Size(1, (this.Height - DownArrowImage.Height)));
            GdiPlus.RECT oWhiteBrushRectRight = new GdiPlus.RECT(this.Width - 1, UpArrowImage.Height, new Size(1, (this.Height - DownArrowImage.Height)));
            GdiPlus.SelectObject(dc, oWhiteBrush);
            GdiPlus.FillRect(dc, ref oWhiteBrushRectLeft, oWhiteBrush);
            GdiPlus.FillRect(dc, ref oWhiteBrushRectRight, oWhiteBrush);
            //bufferGraphics.FillRectangle(oWhiteBrush, new Rectangle(0, UpArrowImage.Height, 1, (this.Height - DownArrowImage.Height)));
            //bufferGraphics.FillRectangle(oWhiteBrush, new Rectangle(this.Width - 1, UpArrowImage.Height, 1, (this.Height - DownArrowImage.Height)));

            
            if (UpArrowImage != null)
            {
                //bufferGraphics.DrawImage(UpArrowImage, new Rectangle(0, 0, this.Width, UpArrowImage.Height), new Rectangle(0, 0, UpArrowImage.Width, UpArrowImage.Height), GraphicsUnit.Pixel);
                GdiPlus.SelectObject(memBitmapDC, ((Bitmap)UpArrowImage).GetHbitmap());
                GdiPlus.StretchBlt(dc, 0, 0, this.Width, UpArrowImage.Height, memBitmapDC, 0, 0, UpArrowImage.Width, UpArrowImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);
            }


            //draw thumb
            int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
            float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            //Debug.WriteLine(nThumbHeight.ToString());

            float fSpanHeight = (fThumbHeight - (ThumbMiddleImage.Height + ThumbTopImage.Height + ThumbBottomImage.Height)) / 2.0f;
            int nSpanHeight = (int)fSpanHeight;

            int nTop = moThumbTop;
            nTop += UpArrowImage.Height;

            //draw top
            //bufferGraphics.DrawImage(ThumbTopImage, new Rectangle(1, nTop, this.Width - 2, ThumbTopImage.Height), new Rectangle(0, 0, ThumbTopImage.Width, ThumbTopImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbTopImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, 1, nTop, this.Width - 2, ThumbTopImage.Height, memBitmapDC, 0, 0, ThumbTopImage.Width, ThumbTopImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += ThumbTopImage.Height;
            //draw top span
            //Rectangle rect = new Rectangle(1, nTop, this.Width - 2, (int)(fSpanHeight * 2));

            //bufferGraphics.DrawImage(ThumbTopSpanImage, rect, new Rectangle(0, 0, ThumbTopSpanImage.Width, ThumbTopSpanImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbTopSpanImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, 1, nTop, this.Width - 2, (int)(fSpanHeight * 2), memBitmapDC, 0, 0, ThumbTopSpanImage.Width, ThumbTopSpanImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += nSpanHeight;
            //draw middle
            //bufferGraphics.DrawImage(ThumbMiddleImage, new Rectangle(1, nTop, this.Width - 2, ThumbMiddleImage.Height), new Rectangle(0, 0, ThumbMiddleImage.Width, ThumbMiddleImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbMiddleImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, 1, nTop, this.Width - 2, ThumbMiddleImage.Height, memBitmapDC, 0, 0, ThumbMiddleImage.Width, ThumbMiddleImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += ThumbMiddleImage.Height;
            //draw bottom span
            //rect = new Rectangle(1, nTop, this.Width - 2, nSpanHeight * 2);
            //bufferGraphics.DrawImage(ThumbBottomSpanImage, rect, new Rectangle(0, 0, ThumbBottomSpanImage.Width, ThumbBottomSpanImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbBottomSpanImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, 1, nTop, this.Width - 2, nSpanHeight, memBitmapDC, 0, 0, ThumbBottomSpanImage.Width, ThumbBottomSpanImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            nTop += nSpanHeight;
            //draw bottom
            //bufferGraphics.DrawImage(ThumbBottomImage, new Rectangle(1, nTop, this.Width - 2, nSpanHeight), new Rectangle(0, 0, ThumbBottomImage.Width, ThumbBottomImage.Height), GraphicsUnit.Pixel);
            GdiPlus.SelectObject(memBitmapDC, ((Bitmap)ThumbBottomImage).GetHbitmap());
            GdiPlus.StretchBlt(dc, 1, nTop, this.Width - 2, ThumbBottomImage.Height, memBitmapDC, 0, 0, ThumbBottomImage.Width, ThumbBottomImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);

            if (DownArrowImage != null)
            {
                //bufferGraphics.DrawImage(DownArrowImage, new Rectangle(0, this.Height - DownArrowImage.Height, this.Width, DownArrowImage.Height), new Rectangle(0, 0, DownArrowImage.Width, DownArrowImage.Height), GraphicsUnit.Pixel);
                GdiPlus.SelectObject(memBitmapDC, ((Bitmap)DownArrowImage).GetHbitmap());
                GdiPlus.StretchBlt(dc, 0, this.Height - DownArrowImage.Height, this.Width, DownArrowImage.Height, memBitmapDC, 0, 0, DownArrowImage.Width, DownArrowImage.Height, GdiPlus.TernaryRasterOperations.SRCCOPY);
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
            int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
            float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            int nTop = moThumbTop;
            nTop += UpArrowImage.Height;

            Rectangle thumbrect = new Rectangle(1, nTop, /*ThumbMiddleImage.*/Width, nThumbHeight);
            Rectangle uparrowrect = new Rectangle(1, 0, /*UpArrowImage.*/Width, UpArrowImage.Height);
            Rectangle downarrowrect = new Rectangle(1, UpArrowImage.Height + nTrackHeight, /*UpArrowImage.*/Width, UpArrowImage.Height);
            Rectangle backrect = new Rectangle(1, UpArrowImage.Height, Width, this.Height - UpArrowImage.Height - DownArrowImage.Height);

            if (thumbrect.Contains(ptPoint))
            {
                m_State = State.SCROLL_WIDGET;

                Debug.WriteLine(string.Format("thumbrect{0}.{1}.{2}.{3} @ hit the thumb", 1, nTop, ThumbMiddleImage.Width, nThumbHeight));
                //hit the thumb
                nClickPoint = (ptPoint.Y - nTop);
                //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                this.moThumbDown = true;
            }
            else if (uparrowrect.Contains(ptPoint))
            {
                m_State = State.UP_ARROW;

                int nRealRange = (Maximum - Minimum) - LargeChange;
                int nPixelRange = (nTrackHeight - nThumbHeight);
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
                int nPixelRange = (nTrackHeight - nThumbHeight);
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
                if (ptPoint.Y < thumbrect.Top)
                {
                    m_State = State.BACK_UP;

                    int nRealRange = (Maximum - Minimum) - LargeChange;
                    int nPixelRange = (nTrackHeight - nThumbHeight);
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
                else if (ptPoint.Y >= thumbrect.Bottom)
                {
                    m_State = State.BACK_DOWN;
                    int nRealRange = (Maximum - Minimum) - LargeChange;
                    int nPixelRange = (nTrackHeight - nThumbHeight);
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

            int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
            float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;
            
            Rectangle uparrowrect = new Rectangle(1, 0, /*UpArrowImage.*/Width, UpArrowImage.Height);
            Rectangle downarrowrect = new Rectangle(1, UpArrowImage.Height + nTrackHeight, /*UpArrowImage.*/Width, UpArrowImage.Height);
            Rectangle backrect = new Rectangle(1, UpArrowImage.Height, Width, this.Height - UpArrowImage.Height - DownArrowImage.Height);

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            Point pos = m_ClickPos;

            switch (m_State)
            {
                case State.UP_ARROW:
                    {
                        if (uparrowrect.Contains(pos))
                        {
                            int nRealRange = (Maximum - Minimum) - LargeChange;
                            int nPixelRange = (nTrackHeight - nThumbHeight);
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
                            int nPixelRange = (nTrackHeight - nThumbHeight);
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
                            int nPixelRange = (nTrackHeight - nThumbHeight);
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
                            int nPixelRange = (nTrackHeight - nThumbHeight);
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

                MoveThumb(e.Y);

                OnValueChanged(EventArgs.Empty);
            }

            base.OnMouseMove(e);
        }
  
        private void MoveThumb(int y)
        {
            int nRealRange = Maximum - Minimum;
            int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
            float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            int nSpot = nClickPoint;

            int nPixelRange = (nTrackHeight - nThumbHeight);
            if (moThumbDown && nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (UpArrowImage.Height + nSpot);

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
                        moThumbTop = y - (UpArrowImage.Height + nSpot);
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
 
        private int GetThumbHeight()
        {
            int nTrackHeight = (this.Height - (UpArrowImage.Height + DownArrowImage.Height));
            float fThumbHeight = ((float)LargeChange / (float)Maximum) * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;

            if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = nTrackHeight;
            }
            if (nThumbHeight < 56)
            {
                nThumbHeight = 56;
                fThumbHeight = 56;
            }

            return nThumbHeight;
        }
        #endregion
    }
#if WIN32
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