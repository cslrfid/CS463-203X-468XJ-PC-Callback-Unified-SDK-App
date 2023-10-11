namespace CSLibrary.Windows.UI
{
    partial class SplashScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            IsDisposed = true;
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.lb_text = new System.Windows.Forms.Label();
            this.updateTextTimer = new System.Windows.Forms.Timer(this.components);
            this.lb_dot = new System.Windows.Forms.Label();
            this.pnl_text = new System.Windows.Forms.Panel();
            this.pic_cs203 = new System.Windows.Forms.PictureBox();
            this.picSchmidt = new System.Windows.Forms.PictureBox();
            this.picCSL = new System.Windows.Forms.PictureBox();
            this.pic_cs101 = new System.Windows.Forms.PictureBox();
            this.picSavi = new System.Windows.Forms.PictureBox();
            this.lbStatus = new System.Windows.Forms.TextBox();
            this.pnl_text.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_cs203)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSchmidt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_cs101)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSavi)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_text
            // 
            this.lb_text.Font = new System.Drawing.Font("Tahoma", 18F);
            this.lb_text.Location = new System.Drawing.Point(3, 0);
            this.lb_text.Name = "lb_text";
            this.lb_text.Size = new System.Drawing.Size(102, 36);
            this.lb_text.TabIndex = 0;
            this.lb_text.Text = "Loading";
            this.lb_text.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // updateTextTimer
            // 
            this.updateTextTimer.Interval = 500;
            this.updateTextTimer.Tick += new System.EventHandler(this.updateTextTimer_Tick);
            // 
            // lb_dot
            // 
            this.lb_dot.Font = new System.Drawing.Font("Tahoma", 18F);
            this.lb_dot.Location = new System.Drawing.Point(108, 0);
            this.lb_dot.Name = "lb_dot";
            this.lb_dot.Size = new System.Drawing.Size(57, 36);
            this.lb_dot.TabIndex = 1;
            // 
            // pnl_text
            // 
            this.pnl_text.BackColor = System.Drawing.Color.White;
            this.pnl_text.Controls.Add(this.lb_text);
            this.pnl_text.Controls.Add(this.lb_dot);
            this.pnl_text.Location = new System.Drawing.Point(103, 72);
            this.pnl_text.Name = "pnl_text";
            this.pnl_text.Size = new System.Drawing.Size(165, 37);
            this.pnl_text.TabIndex = 1;
            // 
            // pic_cs203
            // 
            this.pic_cs203.Image = ((System.Drawing.Image)(resources.GetObject("pic_cs203.Image")));
            this.pic_cs203.Location = new System.Drawing.Point(3, 3);
            this.pic_cs203.Name = "pic_cs203";
            this.pic_cs203.Size = new System.Drawing.Size(94, 63);
            this.pic_cs203.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_cs203.TabIndex = 2;
            this.pic_cs203.TabStop = false;
            // 
            // picSchmidt
            // 
            this.picSchmidt.Image = ((System.Drawing.Image)(resources.GetObject("picSchmidt.Image")));
            this.picSchmidt.Location = new System.Drawing.Point(103, 3);
            this.picSchmidt.Name = "picSchmidt";
            this.picSchmidt.Size = new System.Drawing.Size(165, 63);
            this.picSchmidt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSchmidt.TabIndex = 4;
            this.picSchmidt.TabStop = false;
            this.picSchmidt.Visible = false;
            // 
            // picCSL
            // 
            this.picCSL.Image = ((System.Drawing.Image)(resources.GetObject("picCSL.Image")));
            this.picCSL.Location = new System.Drawing.Point(103, 3);
            this.picCSL.Name = "picCSL";
            this.picCSL.Size = new System.Drawing.Size(165, 63);
            this.picCSL.TabIndex = 5;
            this.picCSL.TabStop = false;
            this.picCSL.Visible = false;
            // 
            // pic_cs101
            // 
            this.pic_cs101.Image = ((System.Drawing.Image)(resources.GetObject("pic_cs101.Image")));
            this.pic_cs101.Location = new System.Drawing.Point(3, 3);
            this.pic_cs101.Name = "pic_cs101";
            this.pic_cs101.Size = new System.Drawing.Size(94, 106);
            this.pic_cs101.TabIndex = 6;
            this.pic_cs101.TabStop = false;
            // 
            // picSavi
            // 
            this.picSavi.Location = new System.Drawing.Point(103, 3);
            this.picSavi.Name = "picSavi";
            this.picSavi.Size = new System.Drawing.Size(165, 63);
            this.picSavi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSavi.TabIndex = 3;
            this.picSavi.TabStop = false;
            this.picSavi.Visible = false;
            // 
            // lbStatus
            // 
            this.lbStatus.Location = new System.Drawing.Point(3, 120);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(285, 20);
            this.lbStatus.TabIndex = 7;
            // 
            // SplashScreen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(289, 145);
            this.ControlBox = false;
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.pnl_text);
            this.Controls.Add(this.pic_cs203);
            this.Controls.Add(this.picSavi);
            this.Controls.Add(this.picSchmidt);
            this.Controls.Add(this.picCSL);
            this.Controls.Add(this.pic_cs101);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.Text = "SplashScreen";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SplashScreen_Closing);
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            this.pnl_text.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_cs203)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSchmidt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCSL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_cs101)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSavi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_cs101;
        private System.Windows.Forms.PictureBox picCSL;
        private System.Windows.Forms.Label lb_text;
        private System.Windows.Forms.Timer updateTextTimer;
        private System.Windows.Forms.Label lb_dot;
        private System.Windows.Forms.PictureBox pic_cs203;
        private System.Windows.Forms.Panel pnl_text;
        private System.Windows.Forms.PictureBox picSchmidt;
        private System.Windows.Forms.PictureBox picSavi;
        private System.Windows.Forms.TextBox lbStatus;
    }
}