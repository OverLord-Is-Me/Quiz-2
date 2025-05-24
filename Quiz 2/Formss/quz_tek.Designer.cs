namespace Quiz_2.Formss
{
    partial class quz_tek
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(quz_tek));
            panel1 = new Panel();
            button3 = new Button();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel2 = new Panel();
            button4 = new Button();
            button2 = new Button();
            button1 = new Button();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton4 = new RadioButton();
            panel3 = new Panel();
            panel4 = new Panel();
            lbl_wrong = new Label();
            lbl_corr = new Label();
            lbl_pont = new Label();
            lbl_qus_num = new Label();
            count_Label = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(229, 242, 247);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1173, 95);
            panel1.TabIndex = 6;
            // 
            // button3
            // 
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.Location = new Point(4, 3);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(40, 42);
            button3.TabIndex = 6;
            button3.Text = "EN";
            button3.UseVisualStyleBackColor = true;
            button3.Visible = false;
            button3.Click += button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(510, 25);
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
            pictureBox1.Location = new Point(436, 8);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(66, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Top;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(0, 95);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(1059, 453);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 675);
            panel2.Name = "panel2";
            panel2.Size = new Size(1173, 120);
            panel2.TabIndex = 8;
            // 
            // button4
            // 
            button4.BackColor = Color.MediumSeaGreen;
            button4.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            button4.Location = new Point(486, 24);
            button4.Name = "button4";
            button4.Size = new Size(200, 72);
            button4.TabIndex = 2;
            button4.Text = "Submit";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.MediumSeaGreen;
            button2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            button2.Location = new Point(15, 24);
            button2.Name = "button2";
            button2.Size = new Size(200, 72);
            button2.TabIndex = 1;
            button2.Text = "Next";
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.LightCoral;
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            button1.Location = new Point(221, 24);
            button1.Name = "button1";
            button1.Size = new Size(200, 72);
            button1.TabIndex = 0;
            button1.Text = "Previous";
            button1.UseVisualStyleBackColor = false;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // radioButton1
            // 
            radioButton1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            radioButton1.Location = new Point(151, 11);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(125, 105);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "A";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radio_answer_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            radioButton2.Location = new Point(430, 11);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(115, 100);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "B";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radio_answer_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            radioButton3.Location = new Point(699, 11);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(115, 100);
            radioButton3.TabIndex = 2;
            radioButton3.TabStop = true;
            radioButton3.Text = "C";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += radio_answer_CheckedChanged;
            // 
            // radioButton4
            // 
            radioButton4.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            radioButton4.Location = new Point(968, 11);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(115, 100);
            radioButton4.TabIndex = 3;
            radioButton4.TabStop = true;
            radioButton4.Text = "D";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radio_answer_CheckedChanged;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(radioButton3);
            panel3.Controls.Add(radioButton1);
            panel3.Controls.Add(radioButton4);
            panel3.Controls.Add(radioButton2);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 548);
            panel3.Name = "panel3";
            panel3.Size = new Size(1173, 127);
            panel3.TabIndex = 9;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(229, 242, 247);
            panel4.Controls.Add(lbl_wrong);
            panel4.Controls.Add(lbl_corr);
            panel4.Controls.Add(lbl_pont);
            panel4.Controls.Add(lbl_qus_num);
            panel4.Controls.Add(count_Label);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(label2);
            panel4.Controls.Add(label1);
            panel4.Dock = DockStyle.Right;
            panel4.Location = new Point(1059, 95);
            panel4.Margin = new Padding(4, 3, 4, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(114, 453);
            panel4.TabIndex = 10;
            // 
            // lbl_wrong
            // 
            lbl_wrong.Font = new Font("Segoe UI", 14.25F);
            lbl_wrong.Location = new Point(5, 424);
            lbl_wrong.Name = "lbl_wrong";
            lbl_wrong.Size = new Size(105, 25);
            lbl_wrong.TabIndex = 7;
            lbl_wrong.Text = "0";
            lbl_wrong.TextAlign = ContentAlignment.MiddleCenter;
            lbl_wrong.Visible = false;
            // 
            // lbl_corr
            // 
            lbl_corr.Font = new Font("Segoe UI", 14.25F);
            lbl_corr.Location = new Point(5, 399);
            lbl_corr.Name = "lbl_corr";
            lbl_corr.Size = new Size(105, 25);
            lbl_corr.TabIndex = 6;
            lbl_corr.Text = "0";
            lbl_corr.TextAlign = ContentAlignment.MiddleCenter;
            lbl_corr.Visible = false;
            // 
            // lbl_pont
            // 
            lbl_pont.Font = new Font("Segoe UI", 14.25F);
            lbl_pont.Location = new Point(5, 374);
            lbl_pont.Name = "lbl_pont";
            lbl_pont.Size = new Size(105, 25);
            lbl_pont.TabIndex = 5;
            lbl_pont.Text = "0";
            lbl_pont.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_qus_num
            // 
            lbl_qus_num.Font = new Font("Segoe UI", 14.25F);
            lbl_qus_num.Location = new Point(5, 228);
            lbl_qus_num.Name = "lbl_qus_num";
            lbl_qus_num.Size = new Size(105, 25);
            lbl_qus_num.TabIndex = 4;
            lbl_qus_num.Text = "40/40";
            lbl_qus_num.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // count_Label
            // 
            count_Label.Font = new Font("Segoe UI", 14.25F);
            count_Label.Location = new Point(5, 82);
            count_Label.Name = "count_Label";
            count_Label.Size = new Size(105, 25);
            count_Label.TabIndex = 3;
            count_Label.Text = "00:00:00";
            count_Label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label3.Location = new Point(5, 345);
            label3.Name = "label3";
            label3.Size = new Size(105, 25);
            label3.TabIndex = 2;
            label3.Text = "Points:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label2.Location = new Point(5, 199);
            label2.Name = "label2";
            label2.Size = new Size(105, 25);
            label2.TabIndex = 1;
            label2.Text = "Questions:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label1.Location = new Point(5, 53);
            label1.Name = "label1";
            label1.Size = new Size(105, 25);
            label1.TabIndex = 0;
            label1.Text = "Time:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // quz_tek
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1173, 795);
            Controls.Add(pictureBox2);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "quz_tek";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "quz_tek";
            FormClosing += quz_tek_FormClosing;
            Load += quz_tek_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button3;
        private Label label4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel2;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Panel panel3;
        private Button button2;
        private Button button1;
        private Panel panel4;
        private Label countdownLabel;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label lbl_pont;
        private Label lbl_qus_num;
        private Label count_Label;
        private System.Windows.Forms.Timer timer1;
        private Label lbl_wrong;
        private Label lbl_corr;
        private Button button4;
    }
}