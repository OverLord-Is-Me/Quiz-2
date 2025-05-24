using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Quiz_2.Formss.copm_connect;

namespace Quiz_2.Formss
{
    public partial class room : Form
    {
        public room()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.BackColor = Color.MediumSeaGreen;
            if (copm_connect.ControlID.confi != "")
            {
                SendMessageToAdmin("Ready:" + ControlID.Comp_Names, ControlID.connected_Server_Address);
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
        private void room_Load(object sender, EventArgs e)
        {
            if (copm_connect.ControlID.selectedClients_Names != "")
            {
                string[] parts = copm_connect.ControlID.selectedClients_Names.Split(new[] { "<#>" }, StringSplitOptions.None);
                int que_count = parts.Length;
                // Use conditional formatting based on the length of pngFileCount
                string formattedNumber = (que_count.ToString().Length == 2) ? (que_count ).ToString("D2") : (que_count ).ToString("D1");
                label52.Text = formattedNumber + "/" + que_count.ToString();

                foreach (string part in parts)
                {
                    Label lbl = new Label();
                    lbl.AutoSize = false;
                    lbl.Size = new Size(814, 74);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Text = part;
                    lbl.Font = new System.Drawing.Font("Sakkal Majalla", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    flowLayoutPanel4.Controls.Add(lbl);
                }
            }
        }
    }
}
