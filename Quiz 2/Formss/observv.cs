using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_2.Formss
{
    public partial class observv : Form
    {
        int Timee = 1;
        private string[] timeeary;
        private string[] Pointss;
        private string[] Answerss;
        private string[] Questionss;
        private int currentImageIndex = 0;
        int pngFileCount;
        private System.Windows.Forms.Timer messageCheckTimer;
        Dictionary<string, ImageInfo> imageTimes = new Dictionary<string, ImageInfo>();
        public class ImageInfo
        {
            public string Image_Name { get; set; }
            public int Time { get; set; }//20sec
            public string Selected_Answer { get; set; }//-1 ABCD
            public int Time_elapsed { get; set; }//20sec
            public int Time_to_select_answer { get; set; } //Time_to_select_answer
            public int Points { get; set; } //Points so far
            public int corr_point { get; set; } //Correct Points 
            public int wrong_point { get; set; } //Wrong Points 
        }
        public observv()
        {
            InitializeComponent();
        }

        private void observv_Load(object sender, EventArgs e)
        {
            currentImageIndex = 0;
            string tempDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");

            if (Directory.Exists(tempDirPath))
            {
                //cong.confi = "Finished<##>Time<#>No<#>0<##>Points<#>Correct<#>1<#>Wrong<#>2<##>Answers<#>Mark<##>Questions<#>Random<##>Slide_0_A.PNG<#>Slide_1_B.PNG<#>Slide_2_C.PNG<#>Slide_3_D.PNG";
                string[] type1 = System.Text.RegularExpressions.Regex.Split(copm_connect.ControlID.confi, "<##>");
                timeeary = System.Text.RegularExpressions.Regex.Split(type1[1], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                Pointss = System.Text.RegularExpressions.Regex.Split(type1[2], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                Answerss = System.Text.RegularExpressions.Regex.Split(type1[3], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                Questionss = System.Text.RegularExpressions.Regex.Split(type1[5], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();

                pngFileCount = Questionss.Count();
                if (pngFileCount > 9)
                {
                    lbl_qus_num.Text = "01/" + pngFileCount.ToString();
                }
                else
                {
                    lbl_qus_num.Text = "1/" + pngFileCount.ToString();
                }

                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", Questionss[currentImageIndex]);
                load_image(imagePath + ".png");

                #region Time Config
                imageTimes = new Dictionary<string, ImageInfo>();
                if (type1[1].Contains("Each"))
                {
                    imageTimes = new Dictionary<string, ImageInfo>();

                    for (int xsxcs = 0; xsxcs < Questionss.Length; xsxcs++)
                    {
                        if (!imageTimes.ContainsKey(Questionss[xsxcs]))
                        {
                            if (Pointss.Length == 3)
                            {
                                imageTimes.Add(Questionss[xsxcs], new ImageInfo { Image_Name = Questionss[xsxcs], Time = Convert.ToInt32(timeeary[2]), Selected_Answer = "-1", corr_point = Convert.ToInt32(Pointss[2]), wrong_point = 0 });
                            }
                            else
                            {
                                imageTimes.Add(Questionss[xsxcs], new ImageInfo { Image_Name = Questionss[xsxcs], Time = Convert.ToInt32(timeeary[2]), Selected_Answer = "-1", corr_point = Convert.ToInt32(Pointss[2]), wrong_point = Convert.ToInt32(Pointss[4]) });

                            }
                        }
                        else
                        {
                            // Key exists, update the value
                            imageTimes[Questionss[xsxcs]].Time = Convert.ToInt32(timeeary[2]);
                        }
                    }
                }
                else
                {
                    for (int xsxcs = 0; xsxcs < Questionss.Length; xsxcs++)
                    {
                        if (!imageTimes.ContainsKey(Questionss[xsxcs]))
                        {
                            imageTimes.Add(Questionss[xsxcs], new ImageInfo { Image_Name = Questionss[xsxcs], Time = Convert.ToInt32(0), Selected_Answer = "-1" });
                        }
                        else
                        {
                            // Key exists, update the value
                            imageTimes[Questionss[xsxcs]].Time = 0;
                        }
                    }
                }
                if (type1[1].Contains("Each"))
                {
                    Timee = -1;
                    put_time(imageTimes[Questionss[currentImageIndex]].Time);
                }
                else if (type1[1].Contains("All"))
                {
                    Timee = -1;
                    put_time(Convert.ToInt32(timeeary[2]));
                }
                else
                {
                    Timee = 1;
                    put_time(Convert.ToInt32(timeeary[2]));
                }
                #endregion
                timer1.Start();
            }
            // Initialize the Timer
            messageCheckTimer = new System.Windows.Forms.Timer();
            messageCheckTimer.Interval = 1000; // Set the interval in milliseconds (adjust as needed)
            messageCheckTimer.Tick += MessageCheckTimer_Tick;
            // Start the timer
            messageCheckTimer.Start();
            string[] parts = copm_connect.ControlID.selectedClients_Names.Split(new[] { "<#>" }, StringSplitOptions.None);
            flowLayoutPanel3.Controls.Clear();
            foreach (var comp in parts)
            {
                Panel pnl = new Panel();
                pnl.Size = new Size(289, 322);
                pnl.BorderStyle = BorderStyle.FixedSingle;

                Label name1 = new Label();
                name1.AutoSize = false;
                name1.Location = new Point(6, 14);
                name1.Text = "Name:";
                name1.Name = "Name1";
                Size name1size = TextRenderer.MeasureText(name1.Text, name1.Font);
                name1.Size = new Size(name1size.Width + 70, name1size.Height + 50);
                //name1.BackColor = Color.Red;

                Label name2 = new Label();
                name2.AutoSize = false;
                name2.Location = new Point(6, 64);
                name2.Size = new Size(278, 112);
                name2.Text = comp;
                name2.Name = "Name2";
                //name1size = TextRenderer.MeasureText(name2.Text, name2.Font);
                //name1.AutoSize = false;
                //name1.Size = new Size(name1size.Width + 20, name1size.Height + 6);
                //name2.BackColor = Color.Gray;

                Label Points1 = new Label();
                Points1.AutoSize = false;
                Points1.Location = new Point(6, 193);
                Points1.Text = "Points:";
                Points1.Name = "Points1";
                name1size = TextRenderer.MeasureText(Points1.Text, Points1.Font);
                Points1.Size = new Size(name1size.Width + 70, name1size.Height + 50);
               // Points1.BackColor = Color.Bisque;

                Label Points2 = new Label();
                Points2.AutoSize = false;
                Points2.Location = new Point(152, 193);
                Points2.Text = "0";
                Points2.Name = "Points2";
                name1size = TextRenderer.MeasureText(Points2.Text, Points2.Font);
                Points2.Size = new Size(name1size.Width + 70, name1size.Height + 50);
                //Points2.BackColor = Color.SaddleBrown;

                Label Select1 = new Label();
                Select1.AutoSize = false;
                Select1.Location = new Point(6, 262);
                Select1.Text = "Selected:";
                Select1.Name = "Select1";
                name1size = TextRenderer.MeasureText(Select1.Text, Select1.Font);
                Select1.Size = new Size(name1size.Width + 70, name1size.Height + 50);
                //Select1.BackColor = Color.AliceBlue;

                Label Select2 = new Label();
                Select2.AutoSize = false;
                Select2.Location = new Point(152, 262);
                Select2.Text = "-";
                Select2.Name = "Select2";
                name1size = TextRenderer.MeasureText(Select2.Text, Select2.Font);
                Select2.Size = new Size(name1size.Width + 70, name1size.Height + 50);
                //Select2.BackColor = Color.BurlyWood;

                pnl.Controls.Add(name1);
                pnl.Controls.Add(name2);
                pnl.Controls.Add(Points1);
                pnl.Controls.Add(Points2);
                pnl.Controls.Add(Select1);
                pnl.Controls.Add(Select2);

                flowLayoutPanel3.Controls.Add(pnl);
            }
        }
        public void load_image(string imagePath)
        {
            // Read the image file into a MemoryStream
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);

                // Create a new Image object from the MemoryStream
                Image imageCopy = Image.FromStream(memoryStream);

                // Assign the imageCopy to pictureBox2
                pictureBox2.Image = imageCopy;
            }
        }
        private void put_time(int sec)
        {
            // Convert seconds to TimeSpan
            TimeSpan time = TimeSpan.FromSeconds(sec);

            // Format TimeSpan as string in hh:mm:ss format
            string formattedTime = time.ToString(@"hh\:mm\:ss");

            // Update the text of the Label
            count_Label.Text = formattedTime;
        }
        private void MessageCheckTimer_Tick(object sender, EventArgs e)
        {
            // Check if the message has changed
            if (copm_connect.ControlID.messag != "")
            {
                timer1.Stop();
                if (copm_connect.ControlID.messag == "Next")
                {
                    button2_Click(this, e);
                }
                else if (copm_connect.ControlID.messag == "Skip")
                {
                    button2_Click(this, e);
                }
                else if (copm_connect.ControlID.messag == "Exit")
                {
                    // Close the form
                    copm_connect.ControlID.messag = "";
                    this.Close();
                    return;
                }
                timer1.Start();
                copm_connect.ControlID.messag = "";
            }


            if (copm_connect.ControlID.observ_messag != "")
            {
                string messs = copm_connect.ControlID.observ_messag;
                messs = messs.Replace("Submit:", "");
                string[] parts = messs.Split("<#>");
                string Comp_Names = parts[0];
                string pnts = parts[4];
                pnts = pnts.Replace("\r", "");
                pnts = pnts.Replace("\n", "");
                int all = 0;
                // Iterate through each panel in flowLayoutPanel4
                foreach (Control control in flowLayoutPanel3.Controls)
                {
                    if (control is Panel panel)
                    {
                        all++;
                        foreach (Control controll in panel.Controls)
                        {
                            if (controll is Label lbl)
                            {
                                if (lbl.Name == "Name2")
                                {
                                    if (lbl.Text.Contains(Comp_Names))
                                    {
                                        foreach (Control controlls in panel.Controls)
                                        {
                                            if (controlls is Label lbls)
                                            {
                                                if (lbls.Name == "Points2")
                                                {
                                                    lbls.Text = pnts;
                                                }
                                                if (lbls.Name == "Select2")
                                                {
                                                    lbls.Text = parts[2];
                                                }
                                                if (lbls.Name == "Submited2")
                                                {
                                                    lbls.Text = "Yes";
                                                    lbls.ForeColor = Color.Green;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                int count = 0;
                // Iterate through each panel in flowLayoutPanel4
                foreach (Control control in flowLayoutPanel3.Controls)
                {
                    if (control is Panel panel)
                    {
                        foreach (Control controll in panel.Controls)
                        {
                            if (controll is Label lbl)
                            {
                                if (lbl.Name == "Submited2")
                                {
                                    if (lbl.Text == "Yes")
                                    {
                                        count++;
                                    }
                                }
                            }

                        }
                    }
                }

                copm_connect.ControlID.observ_messag = "";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentImageIndex < 0)
            {
                currentImageIndex = 0;
            }
            if (currentImageIndex < Questionss.Length)
            {
                currentImageIndex++;

                if(currentImageIndex < Questionss.Length)
                {
                    // Use conditional formatting based on the length of pngFileCount
                    string formattedNumber = (pngFileCount.ToString().Length == 2) ? (currentImageIndex + 1).ToString("D2") : (currentImageIndex + 1).ToString("D1");
                    lbl_qus_num.Text = formattedNumber + "/" + pngFileCount.ToString();

                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", Questionss[currentImageIndex]);
                    load_image(imagePath + ".png");
                    if (timeeary[1].Contains("Each"))
                    {
                        Timee = -1;
                        put_time(imageTimes[Questionss[currentImageIndex]].Time);
                    }
                }
                else
                {
                    timer1.Stop();
                    messageCheckTimer.Stop();
                    MessageBox.Show("Quiz Has Finished");
                    this.Close();
                }

            }
            else
            {
                timer1.Stop();
                messageCheckTimer.Stop();
                MessageBox.Show("Quiz Has Finished"); 
                this.Close();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count_Label.Text != "")
            {
                // Parse the current time from count_Label
                TimeSpan currentTime = TimeSpan.Parse(count_Label.Text);

                // Add one second to the current time
                currentTime = currentTime.Add(TimeSpan.FromSeconds(Timee));
                if (timeeary[1].Contains("Each"))
                {
                    imageTimes[Questionss[currentImageIndex]].Time = imageTimes[Questionss[currentImageIndex]].Time - 1;
                }
                else
                {
                    imageTimes[Questionss[currentImageIndex]].Time = imageTimes[Questionss[currentImageIndex]].Time + 1;
                }

                // Update count_Label with the new time
                count_Label.Text = currentTime.ToString(@"hh\:mm\:ss");
                if (timeeary[1].Contains("Each") || timeeary[1].Contains("All"))
                {
                    if (currentTime.ToString() == "00:00:00")
                    {
                        button2_Click(this, e);
                    }
                }
            }
        }

        private void observv_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}
