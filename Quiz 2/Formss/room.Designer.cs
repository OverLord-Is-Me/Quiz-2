namespace Quiz_2.Formss
{
    partial class room
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel22 = new Panel();
            label52 = new Label();
            label55 = new Label();
            flowLayoutPanel4 = new FlowLayoutPanel();
            panel21 = new Panel();
            button1 = new Button();
            panel1 = new Panel();
            lng = new Button();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            panel22.SuspendLayout();
            panel21.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel22
            // 
            panel22.BackColor = Color.White;
            panel22.Controls.Add(label52);
            panel22.Controls.Add(label55);
            panel22.Dock = DockStyle.Bottom;
            panel22.Location = new Point(0, 78);
            panel22.Name = "panel22";
            panel22.Size = new Size(854, 94);
            panel22.TabIndex = 23;
            // 
            // label52
            // 
            label52.AutoSize = true;
            label52.Font = new Font("Sakkal Majalla", 24.25F, FontStyle.Bold);
            label52.Location = new Point(387, 48);
            label52.Name = "label52";
            label52.Size = new Size(80, 42);
            label52.TabIndex = 22;
            label52.Text = "00/00";
            // 
            // label55
            // 
            label55.AutoSize = true;
            label55.Font = new Font("Sakkal Majalla", 24.25F, FontStyle.Bold);
            label55.Location = new Point(305, 8);
            label55.Name = "label55";
            label55.Size = new Size(244, 42);
            label55.TabIndex = 21;
            label55.Text = "Competitor Counter:";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoScroll = true;
            flowLayoutPanel4.BackColor = Color.White;
            flowLayoutPanel4.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel4.Dock = DockStyle.Bottom;
            flowLayoutPanel4.Location = new Point(0, 172);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(854, 326);
            flowLayoutPanel4.TabIndex = 24;
            // 
            // panel21
            // 
            panel21.Controls.Add(button1);
            panel21.Dock = DockStyle.Bottom;
            panel21.Location = new Point(0, 498);
            panel21.Name = "panel21";
            panel21.Size = new Size(854, 94);
            panel21.TabIndex = 22;
            // 
            // button1
            // 
            button1.BackColor = Color.PaleTurquoise;
            button1.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(292, 13);
            button1.Name = "button1";
            button1.Size = new Size(270, 69);
            button1.TabIndex = 18;
            button1.Text = "Ready";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(229, 242, 247);
            panel1.Controls.Add(lng);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(854, 82);
            panel1.TabIndex = 25;
            // 
            // lng
            // 
            lng.FlatAppearance.BorderSize = 0;
            lng.FlatStyle = FlatStyle.Flat;
            lng.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lng.Location = new Point(2, 2);
            lng.Margin = new Padding(4, 3, 4, 3);
            lng.Name = "lng";
            lng.Size = new Size(40, 42);
            lng.TabIndex = 6;
            lng.Text = "EN";
            lng.UseVisualStyleBackColor = true;
            lng.Visible = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(351, 19);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(226, 44);
            label4.TabIndex = 3;
            label4.Text = "Quiz Quest!";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = Properties.Resources.quiz_logo;
            pictureBox1.Location = new Point(277, 2);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(74, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // room
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(854, 592);
            Controls.Add(panel1);
            Controls.Add(panel22);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(panel21);
            Name = "room";
            Text = "room";
            Load += room_Load;
            panel22.ResumeLayout(false);
            panel22.PerformLayout();
            panel21.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel22;
        private Label label52;
        private Label label55;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label label50;
        private Panel panel21;
        private Button button1;
        private Panel panel1;
        private Button lng;
        private Label label4;
        private PictureBox pictureBox1;
    }
}