namespace Quiz_2.Formss
{
    partial class copm_connect
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(copm_connect));
            panel1 = new Panel();
            lng = new Button();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            button1 = new Button();
            txt_name = new TextBox();
            button4 = new Button();
            label1 = new Label();
            button2 = new Button();
            button3 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
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
            panel1.Size = new Size(821, 82);
            panel1.TabIndex = 9;
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
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(305, 19);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(374, 44);
            label4.TabIndex = 3;
            label4.Text = "Choose the Teacher";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = Properties.Resources.quiz_logo;
            pictureBox1.Location = new Point(231, 2);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(66, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(button1);
            panel2.Controls.Add(txt_name);
            panel2.Controls.Add(button4);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button3);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 566);
            panel2.Name = "panel2";
            panel2.Size = new Size(821, 102);
            panel2.TabIndex = 16;
            // 
            // button1
            // 
            button1.BackColor = Color.LightCoral;
            button1.Font = new Font("Arial", 14.25F);
            button1.Location = new Point(366, 49);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(117, 40);
            button1.TabIndex = 20;
            button1.Text = "Stop";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // txt_name
            // 
            txt_name.Font = new Font("Segoe UI", 12F);
            txt_name.Location = new Point(211, 14);
            txt_name.Name = "txt_name";
            txt_name.Size = new Size(272, 29);
            txt_name.TabIndex = 17;
            txt_name.TextAlign = HorizontalAlignment.Center;
            // 
            // button4
            // 
            button4.BackColor = Color.MediumSeaGreen;
            button4.Font = new Font("Arial", 14.25F);
            button4.Location = new Point(66, 48);
            button4.Margin = new Padding(4, 3, 4, 3);
            button4.Name = "button4";
            button4.Size = new Size(292, 40);
            button4.TabIndex = 19;
            button4.Text = "Search For Online Servers";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(66, 17);
            label1.Name = "label1";
            label1.Size = new Size(142, 21);
            label1.TabIndex = 18;
            label1.Text = "Competitor Name :";
            // 
            // button2
            // 
            button2.BackColor = Color.MediumTurquoise;
            button2.Font = new Font("Arial", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.DarkGoldenrod;
            button2.Location = new Point(321, 13);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(176, 75);
            button2.TabIndex = 14;
            button2.Text = "Ready";
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.MediumSeaGreen;
            button3.Font = new Font("Arial", 14.25F);
            button3.Location = new Point(587, 13);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(176, 75);
            button3.TabIndex = 16;
            button3.Text = "Disconnect";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 82);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(821, 586);
            flowLayoutPanel1.TabIndex = 15;
            // 
            // timer1
            // 
            timer1.Interval = 15000;
            timer1.Tick += timer1_Tick;
            // 
            // copm_connect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(821, 668);
            Controls.Add(panel2);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "copm_connect";
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += copm_connect_FormClosing;
            Load += copm_connect_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button lng;
        private Label label4;
        private PictureBox pictureBox1;
        private Panel panel2;
        private Button button2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button3;
        private TextBox txt_name;
        private Button button4;
        private Label label1;
        private ProgressBar progressBar1;
        private Button button1;
        private System.Windows.Forms.Timer timer1;
    }
}