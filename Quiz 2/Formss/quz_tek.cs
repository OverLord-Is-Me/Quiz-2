using iText.IO.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Quiz_2.Formss.quz_tek;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Timer = System.Threading.Timer;

namespace Quiz_2.Formss
{
    public partial class quz_tek : Form
    {
        int Timee = 1;
        private string[] timeeary;
        private string[] Pointss;
        private string[] Answerss;
        private string[] Questionss;
        private int currentImageIndex = 0;
        int pngFileCount;
        private System.Windows.Forms.Timer messageCheckTimer;

        string temp_corr_point = "0";
        string temp_wrong_point = "0";
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

        public quz_tek()
        {
            InitializeComponent();
        }
        public void load_image (string imagePath)
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
        private void quz_tek_Load(object sender, EventArgs e)
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
                lbl_pont.Text = "0";

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
                                imageTimes.Add(Questionss[xsxcs], new ImageInfo { Image_Name = Questionss[xsxcs], Time = Convert.ToInt32(timeeary[2]), Selected_Answer = "-1" ,corr_point = Convert.ToInt32(Pointss[2]), wrong_point = 0 });
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
                if (currentImageIndex == 0)
                {
                    button1.Enabled = false;
                }
            }
            // Initialize the Timer
            messageCheckTimer = new System.Windows.Forms.Timer();
            messageCheckTimer.Interval = 1000; // Set the interval in milliseconds (adjust as needed)
            messageCheckTimer.Tick += MessageCheckTimer_Tick;
            // Start the timer
            messageCheckTimer.Start();
        }
        private void MessageCheckTimer_Tick(object sender, EventArgs e)
        {
            // Check if the message has changed
            if (copm_connect.ControlID.messag != "")
            {
                timer1.Stop();
                button4.Enabled = false;
                if (copm_connect.ControlID.messag == "Next")
                {
                    button4.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = true;
                    radioButton1.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton2.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton3.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton4.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton1.CheckedChanged += radio_answer_CheckedChanged;
                    radioButton2.CheckedChanged += radio_answer_CheckedChanged;
                    radioButton3.CheckedChanged += radio_answer_CheckedChanged;
                    radioButton4.CheckedChanged += radio_answer_CheckedChanged;
                    calculate_result();
                    button2_Click(this, e);
                }
                else if (copm_connect.ControlID.messag == "Skip")
                {
                    imageTimes[Questionss[currentImageIndex]].corr_point = 0;
                    imageTimes[Questionss[currentImageIndex]].wrong_point = 0;
                    button4.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = true;
                    radioButton1.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton2.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton3.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton4.CheckedChanged -= radio_answer_CheckedChanged;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton1.CheckedChanged += radio_answer_CheckedChanged;
                    radioButton2.CheckedChanged += radio_answer_CheckedChanged;
                    radioButton3.CheckedChanged += radio_answer_CheckedChanged;
                    radioButton4.CheckedChanged += radio_answer_CheckedChanged;
                    calculate_result();
                    button2_Click(this, e);
                }
                else if(copm_connect.ControlID.messag == "Exit")
                {
                    // Close the form
                    copm_connect.ControlID.messag = "";
                    try
                    {
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Error in FormClosing: {ex.Message}");
                    }
                    return;
                }
                button4.Enabled = true;
                timer1.Start();
                copm_connect.ControlID.messag = "";
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(count_Label.Text != "")
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

                if (button2.Enabled == false)
                {
                    button2.Enabled = true;
                    // Update time for the current image in the dictionary
                    //imageTimes[Questionss[currentImageIndex]].Time_to_answer = imageTimes[Questionss[currentImageIndex]].Time_to_answer + 1;
                }
                else
                {
                    // Update time for the current image in the dictionary
                    imageTimes[Questionss[currentImageIndex]].Time_elapsed = imageTimes[Questionss[currentImageIndex]].Time_elapsed + 1;
                }

                // Update count_Label with the new time
                count_Label.Text = currentTime.ToString(@"hh\:mm\:ss");
                if (timeeary[1].Contains("Each") || timeeary[1].Contains("All"))
                {
                    if (currentTime.ToString() == "00:00:00")
                    {
                        button4.Enabled = true;
                        radioButton1.Enabled = true;
                        radioButton2.Enabled = true;
                        radioButton3.Enabled = true;
                        radioButton4.Enabled = true;
                        radioButton1.CheckedChanged -= radio_answer_CheckedChanged;
                        radioButton2.CheckedChanged -= radio_answer_CheckedChanged;
                        radioButton3.CheckedChanged -= radio_answer_CheckedChanged;
                        radioButton4.CheckedChanged -= radio_answer_CheckedChanged;
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        radioButton3.Checked = false;
                        radioButton4.Checked = false;
                        radioButton1.CheckedChanged += radio_answer_CheckedChanged;
                        radioButton2.CheckedChanged += radio_answer_CheckedChanged;
                        radioButton3.CheckedChanged += radio_answer_CheckedChanged;
                        radioButton4.CheckedChanged += radio_answer_CheckedChanged;
                        calculate_result();
                        button2_Click(this, e);

                    }
                }
            }
        }
        private async void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            calculate_result();
            if (copm_connect.ControlID.confi != "")
            {
                string report = "<#>" + imageTimes[Questionss[currentImageIndex]].Image_Name + "<#>" + imageTimes[Questionss[currentImageIndex]].Selected_Answer
                    + "<#>" + imageTimes[Questionss[currentImageIndex]].Time_to_select_answer + "<#>" + imageTimes[Questionss[currentImageIndex]].Points;

                await Task.Run(() => SendMessageToAdmin("Submit:" + copm_connect.ControlID.Comp_Names + report, copm_connect.ControlID.connected_Server_Address));


                //"Submit:copm_connect.Comp_Names<#>Image_Name <#>Selected_Answer <#> Time_to_select_answer <#>Points"
                //quz_tek_Load(sender, e);
            }
            if(button4.Text == "Finish")
            {
                timer1.Stop(); 
                messageCheckTimer.Stop();
                MessageBox.Show("Quiz Has Finished \nYou Scored: " + lbl_pont.Text + " Points"); //quz_tek_Load(sender, e);
                this.Close();
            }
        }
        public void SendMessageToAdmin(string message, string Address)
        {
            try
            {
                // Example code in copm_connect form
                TcpClient userTcpClient = new TcpClient();
                userTcpClient.Connect(Address, 12347);

                if (userTcpClient != null && userTcpClient.Connected)
                {

                    NetworkStream stream = userTcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(stream);

                    // Send the message to the admin
                    writer.WriteLine(message);
                    writer.Flush();
                }
                else
                {
                    MessageBox.Show("Not connected to the server.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message to admin: {ex.Message}");
            }
        }
        private async void radio_answer_CheckedChanged(object sender, EventArgs e)
        {
            imageTimes[Questionss[currentImageIndex]].Time_to_select_answer = imageTimes[Questionss[currentImageIndex]].Time_elapsed;
            foreach (Control control in panel3.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked == true)
                {
                    imageTimes[Questionss[currentImageIndex]].Selected_Answer = radioButton.Text;
                    break; // only want the first checked RadioButton
                }
            }
            if(radioButton4.Checked || radioButton3.Checked || radioButton2.Checked || radioButton1.Checked)
            {
                int correctCount = 0;
                int wrongCount = 0;

                foreach (var kvp in imageTimes)
                {
                    ImageInfo imageInfo = kvp.Value;

                    // The checked RadioButton is found
                    string fileName = imageInfo.Image_Name;

                    // Find the last occurrence of "_" in the string
                    int lastIndex = fileName.LastIndexOf('_');

                    if (lastIndex != -1 && lastIndex < fileName.Length - 1)
                    {
                        // Get the character before "_"
                        char lastLetter = fileName[lastIndex + 1];

                        // Compare with Selected_Answer
                        if (lastLetter.ToString() == imageInfo.Selected_Answer)
                        {
                            correctCount = correctCount + imageInfo.corr_point;
                        }
                        else if (Pointss.Length == 5/* && imageInfo.Selected_Answer != "-1"*/)
                        {
                            wrongCount = wrongCount + imageInfo.wrong_point;
                        }
                    }
                }
                imageTimes[Questionss[currentImageIndex]].Points = correctCount - wrongCount;
                if (copm_connect.ControlID.confi != "")
                {
                    string report = "<#>" + imageTimes[Questionss[currentImageIndex]].Image_Name + "<#>" + imageTimes[Questionss[currentImageIndex]].Selected_Answer
                        + "<#>" + imageTimes[Questionss[currentImageIndex]].Time_to_select_answer + "<#>" + imageTimes[Questionss[currentImageIndex]].Points;

                    await Task.Run(() => SendMessageToAdmin("Ansr:" + copm_connect.ControlID.Comp_Names + report, copm_connect.ControlID.connected_Server_Address));
                }
            }
        }

        private void quz_tek_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            copm_connect.ControlID.selectedClients_Names = "";
            copm_connect.ControlID.Comp_Names = "";
            copm_connect.ControlID.confi = "";              
            copm_connect.ControlID.messag = "";
            copm_connect.ControlID.observ_messag = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //calculate_result();
            if (currentImageIndex < 0)
            {
                currentImageIndex = 0;
            }
            //button1.Enabled = true;
            if (currentImageIndex < Questionss.Length && button4.Text != "Finish")
            {
                if (currentImageIndex == Questionss.Length - 2)
                {
                    button4.Text = "Finish";
                }

                currentImageIndex++;
                //if (timeeary[1].Contains("Each"))
                //{
                //    // Check if the time for the current image is 0
                //    if (imageTimes[Questionss[currentImageIndex]].Time <= 0)
                //    {
                //        // Skip to the next available image
                //        button2.PerformClick(); return;
                //        // SkipToNextAvailableImage();
                //    }
                //}

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
                //get_selected_answer();
            }
            else
            {
                button2.Text = "Next";
                timer1.Stop(); 
                messageCheckTimer.Stop();
                MessageBox.Show("Quiz Has Finished \nYou Scored: " + lbl_pont.Text + " Points"); //quz_tek_Load(sender, e);
                button4.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
                this.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            calculate_result();

            if (currentImageIndex == Questionss.Length)
            {
                currentImageIndex--;
            }
            if (currentImageIndex <= 0)
            {
                button1.Enabled = false;
            }
            if (button2.Text == "Finish")
            {
                button2.Text = "Next";
            }
            if (currentImageIndex > 0)
            {
                --currentImageIndex;

                if (timeeary[1].Contains("Each"))
                {
                    if (currentImageIndex >= 0)
                    {
                        if (imageTimes[Questionss[0]].Time == 0)
                        {
                            button2.PerformClick(); return;
                        }
                        if (imageTimes[Questionss[currentImageIndex]].Time == 0)
                        {
                            // Skip to the previos available image
                            button1.PerformClick(); return;
                        }
                    }
                    else
                    {
                        button2.PerformClick(); return;
                    }
                }

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

                get_selected_answer();

                if (currentImageIndex == 0)
                {
                    button1.Enabled = false;
                }
            }
            else
            {
                button1.Enabled = false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form2 = new Tchr();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
        private void calculate_result1()
        {
            int correctCount = 0;
            int wrongCount = 0;
            #region MyRegion
            //foreach (Control control in panel3.Controls)
            //{
            //    if (control is RadioButton radioButton && radioButton.Checked)
            //    {
            //        // The checked RadioButton is found
            //        string fileName = imageTimes[Questionss[currentImageIndex]].Image_Name;

            //        // Find the last occurrence of "_" in the string
            //        int lastIndex = fileName.LastIndexOf('_');

            //        // Check if "_" is found in the string
            //        if (lastIndex != -1 && lastIndex < fileName.Length - 1)
            //        {
            //            // Get the character before "_"
            //            char lastLetter = fileName[lastIndex + 1];
            //            if (lastLetter.ToString() == radioButton.Text)
            //            {
            //                lbl_pont.Text = Convert.ToString(Convert.ToInt32(lbl_pont.Text) + Convert.ToInt32(Pointss[2]));
            //            }
            //            else if (Pointss.Length == 5)
            //            {
            //                lbl_pont.Text = Convert.ToString(Convert.ToInt32(lbl_pont.Text) - Convert.ToInt32(Pointss[4]));
            //            }
            //            imageTimes[Questionss[currentImageIndex]].Selected_Answer = radioButton.Text;
            //        }
            //        break; // only want the first checked RadioButton
            //    }
            //} 
            #endregion
            foreach (var kvp in imageTimes)
            {
                ImageInfo imageInfo = kvp.Value;

                // The checked RadioButton is found
                string fileName = imageInfo.Image_Name;

                // Find the last occurrence of "_" in the string
                int lastIndex = fileName.LastIndexOf('_');

                if (lastIndex != -1 && lastIndex < fileName.Length - 1)
                {
                    // Get the character before "_"
                    char lastLetter = fileName[lastIndex + 1];

                    // Compare with Selected_Answer
                    if (lastLetter.ToString() == imageInfo.Selected_Answer )
                    {
                        correctCount = correctCount + Convert.ToInt32(Pointss[2]);
                    }
                    else if (Pointss.Length == 5/* && imageInfo.Selected_Answer != "-1"*/)
                    {
                        wrongCount = wrongCount + Convert.ToInt32(Pointss[4]);
                    }
                }
            }
            lbl_pont.Text = Convert.ToString(correctCount - wrongCount);
            imageTimes[Questionss[currentImageIndex]].Points = correctCount - wrongCount;
            #region MyRegion
            //if (Answerss.Contains("Mark"))
            //{
            //    button1.Visible = false;
            //}
            //else
            //{
            //    label3.Visible = false;
            //    lbl_pont.Visible = false;
            //}

            //lbl_corr.Text = correctCount.ToString(); 
            #endregion
        }

        private void calculate_result()
        {
            int correctCount = 0;
            int wrongCount = 0;
            
            foreach (var kvp in imageTimes)
            {
                ImageInfo imageInfo = kvp.Value;

                // The checked RadioButton is found
                string fileName = imageInfo.Image_Name;

                // Find the last occurrence of "_" in the string
                int lastIndex = fileName.LastIndexOf('_');

                if (lastIndex != -1 && lastIndex < fileName.Length - 1)
                {
                    // Get the character before "_"
                    char lastLetter = fileName[lastIndex + 1];

                    // Compare with Selected_Answer
                    if (lastLetter.ToString() == imageInfo.Selected_Answer)
                    {
                        correctCount = correctCount + imageInfo.corr_point;
                    }
                    else if (Pointss.Length == 5/* && imageInfo.Selected_Answer != "-1"*/)
                    {
                        wrongCount = wrongCount + imageInfo.wrong_point;
                    }
                }
            }
            lbl_pont.Text = Convert.ToString(correctCount - wrongCount);
            imageTimes[Questionss[currentImageIndex]].Points = correctCount - wrongCount;
            #region MyRegion
            //if (Answerss.Contains("Mark"))
            //{
            //    button1.Visible = false;
            //}
            //else
            //{
            //    label3.Visible = false;
            //    lbl_pont.Visible = false;
            //}

            //lbl_corr.Text = correctCount.ToString(); 
            #endregion
        }
        private void get_selected_answer()
        {
            //set the Selected Answer
            foreach (Control control in panel3.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Text == imageTimes[Questionss[currentImageIndex]].Selected_Answer)
                {
                    radioButton.Checked = true;
                }
            }
        }
    }
}
