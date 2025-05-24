namespace Quiz_2.Formss
{
    partial class Questions
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
            panel1 = new Panel();
            lng = new Button();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            qshns = new Button();
            panel2 = new Panel();
            button4 = new Button();
            button3 = new Button();
            button1 = new Button();
            button2 = new Button();
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
            panel1.Size = new Size(1055, 82);
            panel1.TabIndex = 7;
            // 
            // lng
            // 
            lng.FlatAppearance.BorderSize = 0;
            lng.FlatStyle = FlatStyle.Flat;
            lng.Font = new Font("Arial", 12F);
            lng.Location = new Point(2, 2);
            lng.Margin = new Padding(4, 3, 4, 3);
            lng.Name = "lng";
            lng.Size = new Size(40, 42);
            lng.TabIndex = 6;
            lng.Text = "EN";
            lng.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 27.75F, FontStyle.Bold);
            label4.Location = new Point(412, 19);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(304, 44);
            label4.TabIndex = 3;
            label4.Text = "Questions Bank";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = Properties.Resources.quiz_logo;
            pictureBox1.Location = new Point(338, 2);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(66, 78);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 82);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1055, 522);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // qshns
            // 
            qshns.BackColor = Color.FromArgb(52, 152, 219);
            qshns.Font = new Font("Arial", 14.25F);
            qshns.Location = new Point(135, 6);
            qshns.Margin = new Padding(4, 3, 4, 3);
            qshns.Name = "qshns";
            qshns.Size = new Size(298, 40);
            qshns.TabIndex = 12;
            qshns.Text = "Import From File Or Folder";
            qshns.UseVisualStyleBackColor = false;
            qshns.Click += qshns_Click;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(button4);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(qshns);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 604);
            panel2.Name = "panel2";
            panel2.Size = new Size(1055, 100);
            panel2.TabIndex = 13;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(52, 152, 219);
            button4.Font = new Font("Arial", 14.25F);
            button4.Location = new Point(135, 6);
            button4.Margin = new Padding(4, 3, 4, 3);
            button4.Name = "button4";
            button4.Size = new Size(298, 40);
            button4.TabIndex = 17;
            button4.Text = "Import From File Or Folder";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(52, 152, 219);
            button3.Font = new Font("Arial", 14.25F);
            button3.Location = new Point(135, 52);
            button3.Margin = new Padding(4, 3, 4, 3);
            button3.Name = "button3";
            button3.Size = new Size(298, 40);
            button3.TabIndex = 16;
            button3.Text = "Import From Valut";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.MediumSeaGreen;
            button1.Font = new Font("Arial", 14.25F);
            button1.Location = new Point(499, 20);
            button1.Margin = new Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new Size(176, 58);
            button1.TabIndex = 15;
            button1.Text = "Continue";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.IndianRed;
            button2.Font = new Font("Arial", 14.25F);
            button2.Location = new Point(741, 20);
            button2.Margin = new Padding(4, 3, 4, 3);
            button2.Name = "button2";
            button2.Size = new Size(176, 58);
            button2.TabIndex = 14;
            button2.Text = "Return";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // Questions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1055, 704);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "Questions";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Questions Bank";
            Load += Questions_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button lng;
        private Label label4;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button qshns;
        private Panel panel2;
        private Button button2;
        private Button button1;
        private Button button3;
        private Button button4;
    }
}