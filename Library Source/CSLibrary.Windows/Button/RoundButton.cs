using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSLibrary.Windows
{
    /// <summary>
    /// Round Button Control
    /// </summary>
    public partial class RoundButton : Control
    {
        private bool bMouseDown = false;//OrangeRed
        private Color mActiveBackColor = Color.LightGreen;
        private Color mDisabledBackColor = Color.Gray;
        private HorizontalAlignment mTextAlign = HorizontalAlignment.Center;
        private bool bDrawBorderLine = false;
        private Color mBorderLineColor = Color.Black;
        private Color mDisableBorderLineColor = Color.Gray;
        private int textMargin;
        private string mText = "";
        /// <summary>
        /// Constructor
        /// </summary>
        public RoundButton()
        {
            this.Size = new Size(120, 35);
            BackColor = Color.ForestGreen;
        }

        /// <summary>
        /// Gets or sets the background color of the control in mouse down state.
        /// </summary>
        public Color ActiveBackColor
        {
            get { return mActiveBackColor; }
            set { mActiveBackColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Gets or sets the background color for the control in inactive state.
        /// </summary>
        public Color DisabledBackColor
        {
            get { return mDisabledBackColor; }
            set { mDisabledBackColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Gets or sets the background color for the control in inactive state.
        /// </summary>
        public Color DisableBorderLineColor
        {
            get { return mDisableBorderLineColor; }
            set { mDisableBorderLineColor = value; }
        }
        /// <summary>
        /// Gets or sets how text is aligned.
        /// </summary>
        public HorizontalAlignment TextAlign
        {
            get { return mTextAlign; }
            set { mTextAlign = value; this.Invalidate(); }
        }
        /// <summary>
        /// Gets or sets the border line color
        /// </summary>
        public Color BorderLineColor
        {
            get { return mBorderLineColor; }
            set { mBorderLineColor = value; this.Invalidate(); }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the control should draw border line.
        /// </summary>
        public bool DrawBorderLine
        {
            get { return bDrawBorderLine; }
            set { bDrawBorderLine = value; this.Invalidate(); }
        }
        /// <summary>
        /// Gets or sets text margin
        /// </summary>
        public int TextMargin
        {
            get { return textMargin; }
            set { textMargin = value; this.Invalidate(); }
        }

        public override string Text
        {
            get { return mText; }
            set { mText = value; this.Invalidate(); }
        }

        #region Protected
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            using (Bitmap BmpBuff = new Bitmap(Width, Height))
            using (Graphics gx = Graphics.FromImage(BmpBuff))
            {
                gx.FillRegion(new SolidBrush(this.Parent.BackColor), e.Graphics.Clip);
                if (Enabled)
                    DrawRoundedRectangle(gx, bDrawBorderLine == true ? new Pen(mBorderLineColor) : new Pen(this.BackColor), bMouseDown == true ? mActiveBackColor : BackColor, new Rectangle(0, 0, Width - 1, Height - 1), new Size(20, 20));
                else
                    DrawRoundedRectangle(gx, bDrawBorderLine == true ? new Pen(mDisableBorderLineColor) : new Pen(this.BackColor), mDisabledBackColor, new Rectangle(0, 0, Width - 1, Height - 1), new Size(20, 20));
                e.Graphics.DrawImage(BmpBuff, 0, 0);
            }
        }

        private void DrawRoundedRectangle(Graphics g, Pen p, Color backColor, Rectangle rc, Size size)
        {
            Point[] points = new Point[8];

            //prepare points for poligon
            points[0].X = rc.Left + size.Width / 2;
            points[0].Y = rc.Top + 1;
            points[1].X = rc.Right - size.Width / 2;
            points[1].Y = rc.Top + 1;

            points[2].X = rc.Right;
            points[2].Y = rc.Top + size.Height / 2;
            points[3].X = rc.Right;
            points[3].Y = rc.Bottom - size.Height / 2;

            points[4].X = rc.Right - size.Width / 2;
            points[4].Y = rc.Bottom;
            points[5].X = rc.Left + size.Width / 2;
            points[5].Y = rc.Bottom;

            points[6].X = rc.Left + 1;
            points[6].Y = rc.Bottom - size.Height / 2;
            points[7].X = rc.Left + 1;
            points[7].Y = rc.Top + size.Height / 2;

            //prepare brush for background
            Brush fillBrush = new SolidBrush(backColor);

            //draw side lines and circles in the corners
            g.DrawLine(p, rc.Left + size.Width / 2, rc.Top, rc.Right - size.Width / 2, rc.Top);
            g.FillEllipse(fillBrush, rc.Right - size.Width, rc.Top, size.Width, size.Height);
            g.DrawEllipse(p, rc.Right - size.Width, rc.Top, size.Width, size.Height);

            g.DrawLine(p, rc.Right, rc.Top + size.Height / 2, rc.Right, rc.Bottom - size.Height / 2);
            g.FillEllipse(fillBrush, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height);
            g.DrawEllipse(p, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height);

            g.DrawLine(p, rc.Right - size.Width / 2, rc.Bottom, rc.Left + size.Width / 2, rc.Bottom);
            g.FillEllipse(fillBrush, rc.Left, rc.Bottom - size.Height, size.Width, size.Height);
            g.DrawEllipse(p, rc.Left, rc.Bottom - size.Height, size.Width, size.Height);

            g.DrawLine(p, rc.Left, rc.Bottom - size.Height / 2, rc.Left, rc.Top + size.Height / 2);
            g.FillEllipse(fillBrush, rc.Left, rc.Top, size.Width, size.Height);
            g.DrawEllipse(p, rc.Left, rc.Top, size.Width, size.Height);
            //fill the background and remove the internal arcs  
            g.FillPolygon(fillBrush, points);
            //dispose the brush
            fillBrush.Dispose();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            List<string> StringLine = new List<string>();//ten line support;///
            int txtwidth = (int)e.Graphics.MeasureString(this.Text, Font).Width;
            int txtheight = (int)e.Graphics.MeasureString(Text, Font).Height;
            float textYpos = (this.Height - txtheight) / 2;
            float textXpos = 0;// (TextAlign == false) ? textMargin : ((this.Width - txtwidth) / 2);
            switch (mTextAlign)
            {
                case HorizontalAlignment.Center:
                    textXpos = textMargin + ((this.Width - txtwidth) / 2);
                    break;
                case HorizontalAlignment.Left:
                    textXpos = textMargin;
                    break;
                case HorizontalAlignment.Right:
                    textXpos = textMargin + (this.Width - txtwidth);
                    break;
            }
            if (txtwidth + textMargin /* Margin*/ > this.Width)
            {
                int line = 0;
                string tempString = Text;
                for (int i = 1; i <= tempString.Length; i++)
                {
                    txtwidth = (int)e.Graphics.MeasureString(tempString.Substring(0, i), Font).Width;
                    if (txtwidth + textMargin /* Margin*/ > Width)
                    {
                        StringLine.Add(tempString.Substring(0, i - 1));
                        line++;
                        tempString = tempString.Substring(i - 1);
                        i = 1;
                    }
                    if (i == tempString.Length)
                    {
                        int totallen = 0;
                        foreach (string temp in StringLine)
                        {
                            totallen += temp.Length;
                        }
                        if (totallen < Text.Length)
                        {
                            StringLine.Add(tempString.Substring(0));
                        }
                    }
                }
                line = StringLine.Count;
                int CurLine = 0;
                //Calc YPos
                int totalHeight = 0;
                foreach (string strLine in StringLine)
                {
                    totalHeight += (int)e.Graphics.MeasureString(strLine, Font).Height;
                }
                int startYPos = (Height - totalHeight) / 2;
                if (startYPos < 0)
                    startYPos = 0;
                foreach (string strLine in StringLine)
                {
                    int startXPos = (Width - (int)e.Graphics.MeasureString(strLine, Font).Width) / 2;
                    if (startXPos < 0)
                        startXPos = 0;
                    e.Graphics.DrawString(strLine, Font, new System.Drawing.SolidBrush(this.ForeColor), startXPos, startYPos + CurLine * txtheight);
                    CurLine++;
                }

            }
            else
                e.Graphics.DrawString(this.Text, Font, new System.Drawing.SolidBrush(this.ForeColor), textXpos, textYpos);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            bMouseDown = true;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            bMouseDown = false;
            this.Invalidate();
            base.OnMouseUp(e);
        }
        
        protected override void OnEnabledChanged(System.EventArgs e)
        {
            this.Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnResize(System.EventArgs e)
        {
            this.Invalidate();
            base.OnResize(e);
        }
        #endregion
    }
}
