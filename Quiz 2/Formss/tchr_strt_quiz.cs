using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Quiz_2.Formss.Copms;
using System.Security.Principal;
using System.IO.Abstractions;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Management;
using System.Drawing.Imaging;

namespace Quiz_2.Formss
{
    public partial class tchr_strt_quiz : Form
    {
        
        public tchr_strt_quiz()
        {
            InitializeComponent();
        }

        private void tchr_strt_quiz_Load(object sender, EventArgs e)
        {

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = radioButton1.Checked;
            label2.Enabled = radioButton1.Checked;
            textBox1.Text = "";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = radioButton2.Checked;
            label3.Enabled = radioButton2.Checked;
            textBox2.Text = "";
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = radioButton6.Checked;
            label7.Enabled = radioButton6.Checked;
            textBox4.Text = "";
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = radioButton5.Checked;
            label6.Enabled = radioButton5.Checked;
            textBox3.Text = "";
        }
        string coll = ""; 
        private void button1_Click(object sender, EventArgs e)
        {

            #region MyRegion
            // Check Time System
            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
            {
                if (radioButton1.Checked == true && textBox1.Text == "")
                {
                    MessageBox.Show("Please Write The Time (For Each Question) First"); return;
                }
                if (radioButton2.Checked == true && textBox2.Text == "")
                {
                    MessageBox.Show("Please Write The Time (For All Question) First"); return;
                }
                coll = "Time<#>";
                if (radioButton1.Checked)
                {
                    coll = coll + "Each<#>" + textBox1.Text;
                }
                else if (radioButton2.Checked)
                {
                    coll = coll + "All<#>" + textBox2.Text;
                }
                else if (radioButton3.Checked)
                {
                    coll = coll + radioButton3.Text + "<#>0";
                }
            }
            else
            {
                MessageBox.Show("Please Choose The (Time) System First"); return;
            }

            // Check Points System
            if (radioButton6.Checked || radioButton5.Checked)
            {
                if (radioButton6.Checked == true && textBox4.Text == "")
                {
                    MessageBox.Show("Please Write The Points (For Each Correct Answer) First"); return;
                }
                if (radioButton5.Checked == true && textBox3.Text == "")
                {
                    MessageBox.Show("Please Write The Points (For Each Wrong Answer) First"); return;
                }
                coll = coll + "<##>Points<#>";
                if (radioButton6.Checked)
                {
                    coll = coll + "Correct<#>" + textBox4.Text;
                }
                if (radioButton5.Checked && radioButton6.Checked)
                {
                    coll = coll + "<#>Wrong<#>" + textBox3.Text;
                }
                else if (radioButton5.Checked)
                {
                    coll = coll + "Wrong<#>" + textBox3.Text;
                }
            }
            else
            {
                MessageBox.Show("Please Choose The (Points) System First"); return;
            }

            // Check Answers System
            if (radioButton4.Checked || radioButton9.Checked)
            {
                coll = coll + "<##>Answers<#>";
                if (radioButton9.Checked)
                {
                    coll = coll + "Mark";
                }
                else if (radioButton4.Checked)
                {
                    coll = coll + "Hide";
                }
            }
            else
            {
                MessageBox.Show("Please Choose The (Answers) System First"); return;
            }

            // Check Questions System
            if (radioButton8.Checked || radioButton7.Checked)
            {
                coll = coll + "<##>Questions<#>";
                if (radioButton8.Checked)
                {
                    coll = coll + "Random";
                }
                else if (radioButton7.Checked)
                {
                    coll = coll + "Same";
                }
            }
            else
            {
                MessageBox.Show("Please Choose The (Questions) System First"); return;
            } 
            #endregion


            if(false)
            {
                #region Ckecking
                if (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false)
                {
                    MessageBox.Show("Please Choose The (Time) System First"); return;
                }
                if (radioButton1.Checked == true && textBox1.Text == "")
                {
                    MessageBox.Show("Please Write The Time (For Each Question) First"); return;
                }
                if (radioButton2.Checked == true && textBox2.Text == "")
                {
                    MessageBox.Show("Please Write The Time (For All Question) First"); return;
                }

                if (radioButton6.Checked == false && radioButton5.Checked == false)
                {
                    MessageBox.Show("Please Choose The (Points) System First"); return;
                }
                if (radioButton6.Checked == true && textBox4.Text == "")
                {
                    MessageBox.Show("Please Write The Points (For Each Correct Answer) First"); return;
                }
                if (radioButton5.Checked == true && textBox3.Text == "")
                {
                    MessageBox.Show("Please Write The Points (For Each Wrong Answer) First"); return;
                }

                if (radioButton4.Checked == false && radioButton9.Checked == false)
                {
                    MessageBox.Show("Please Choose The (Answers) System First"); return;
                }

                if (radioButton8.Checked == false && radioButton7.Checked == false)
                {
                    MessageBox.Show("Please Choose The (Questions) System First"); return;
                }
                #endregion}
            }

            SendSelectedPictureBoxesOverNetwork();
            //this.Hide();
            //var form2 = new Tchr();
            //form2.Closed += (s, args) => this.Close();
            //form2.Show();
        }
        private async Task SendSelectedPictureBoxesOverNetwork()
        {

            // Get the names of the selected images
            List<string> imageNames = Questions.ControlID.selectedPictureBoxes.Select(pb => pb.Name).ToList();

            // Handle the conditions
            string result = "";
            if (coll.Contains("Random", StringComparison.OrdinalIgnoreCase))
            {
                // Create a new string with the image names in a random order
                Random random = new Random();
                List<string> randomImageNames = imageNames.OrderBy(_ => random.Next()).ToList();
                result = string.Join("<#>", randomImageNames.Distinct());
            }
            else if (coll.Contains("Same", StringComparison.OrdinalIgnoreCase))
            {
                // Create a new string with the image names in sorted order
                List<string> sortedImageNames = imageNames.OrderBy(name => name).ToList();
                result = string.Join("<#>", sortedImageNames.Distinct());
            }
            string QuestionsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions");

            if (Copms.ControlID.connectedClients != null)
            {
                foreach (var clientInfo in Copms.ControlID.connectedClients)
                {
                    foreach (var pictureBox in Questions.ControlID.selectedPictureBoxes)
                    {
                        //Convert PictureBox data to bytes
                        //byte[] imageData = ConvertPictureBoxToBytes(pictureBox);
                        string imageDataString = ConvertPictureBoxToBase64String(pictureBox);
                        int sda = imageDataString.Length;
                        //Send each picture separately
                        await SendImageDataAsync(pictureBox.Name + ".PNG", imageDataString, clientInfo.Client);
                    }
                    await SendImageDataAsync("Finished" + "<##>" + coll + "<##>" + result, "", clientInfo.Client);
                }
            }
        }
        private string ConvertPictureBoxToBase64String(PictureBox pictureBox)
        {
            MemoryStream memoryStream = new MemoryStream();
            {
                // Save PictureBox image to MemoryStream
                pictureBox.Image.Save(memoryStream, ImageFormat.Png);

                // Convert MemoryStream to byte array
                byte[] imageData = memoryStream.ToArray();

                // Convert byte array to Base64 string
                string base64String = Convert.ToBase64String(imageData);

                return base64String;
            }
        }
        private async Task SendImageDataAsync(string imageName, string imageDataString, TcpClient client)
        {
            // Send image data
            await SendMessageToUserAsync($"IMAGE:{imageName}", imageDataString, client);
        }
        private async Task SendMessageToUserAsync(string message, string imageDataString, TcpClient userTcpClient)
        {
            try
            {
                // Ensure client is connected
                if (userTcpClient.Connected)
                {
                    // Combine message and image data into a single string for transmission
                    string combinedData = message + Environment.NewLine + imageDataString;

                    // Convert combined data to bytes for network transmission
                    byte[] dataBytes = Encoding.UTF8.GetBytes(combinedData);

                    // Send data asynchronously, handling potential exceptions
                    NetworkStream stream = userTcpClient.GetStream();
                    {
                        try
                        {
                            await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
                        }
                        catch (IOException ex)
                        {
                            // Handle write failure gracefully (e.g., log error, notify user)
                            Console.WriteLine("Error sending message: {0}", ex.Message);
                        }
                    }
                }


                if (userTcpClient != null && userTcpClient.Connected)
                {
                    NetworkStream stream = userTcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(stream);
                    {
                        message = message + "<#>" + imageDataString;

                        // Send the Base64 string as a single line
                        await writer.WriteLineAsync($"{message}\n");
                        //await writer.FlushAsync();
                    }
                }
                else
                {
                    MessageBox.Show("The TcpClient is not connected.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message to user: {ex.Message}");
            }
        }
        
        
        
        
        
        private byte[] ConvertPictureBoxToBytes(PictureBox pictureBox)
        {
            MemoryStream ms = new MemoryStream();
            {
                string sd = pictureBox.Name.ToString();
                pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                return ms.ToArray();
            }
        }
        private async Task SendImageDataAsync1(string imageName, byte[] imageData, TcpClient client)
        {
            // Send image data length
            byte[] lengthBytes = BitConverter.GetBytes(imageData.Length);
            await client.GetStream().WriteAsync(lengthBytes, 0, lengthBytes.Length);

            // Send image data
            await SendMessageToUserAsync($"IMAGE:{imageName}", imageData.ToString(), client);
        }
        private async Task SendMessageToUserAsync1(string message, byte[] data, TcpClient userTcpClient)
        {
            try
            {
                NetworkStream stream = userTcpClient.GetStream();
                StreamWriter writer = new StreamWriter(stream);

                // Send the message to the user
                await writer.WriteLineAsync($"{message}\n");
                await writer.FlushAsync();

                // Send the data to the user
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message to user: {ex.Message}");
            }
        }
    }
}
