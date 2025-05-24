using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Protection.PlayReady;
using static Quiz_2.Formss.quz_tek;
using Font = System.Drawing.Font;
using iText.IO.Image;
using System.Security.Policy;



namespace Quiz_2.Formss
{
    public partial class Tchr : Form
    {
        public List<ClientInfo> connectedClients = new List<ClientInfo>();
        public List<ClientInfo> selectedClients = new List<ClientInfo>();
        public List<ClientInfo> ObserverClients = new List<ClientInfo>();
        public List<string> results = new List<string>();
        List<(string, int)> resultList = new List<(string, int)>();
        public class ClientInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public TcpClient Client { get; set; }
        }
        public static List<PictureBox> selectedPictureBoxes = new List<PictureBox>();


        private bool shouldStopBroadcasting = false;
        private Thread broadcastThread;
        private Thread aliveMessageThread;
        public Thread listenerThread;

        //public int u_id = new int();
        public DataTable searssh = new DataTable();
        public TcpListener tcpListener { get; set; }



        //private string[] timeeary;
        //private string[] Questionss;

        string result = "";
        string coll = "";
        public string Server_name = "";
       

        public Tchr()
        {
            InitializeComponent();
        }
        private void Tchr_Load(object sender, EventArgs e)
        {
            //tabControl1.TabPages.Remove(tabPage1);
            //strt.Enabled = true;
            return;
        }
        private void Tchr_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Set flags to stop threads
            shouldStopBroadcasting = true;

            ShutdownServer();

            if (listenerThread != null)
            {
                // Wait for threads to finish
                listenerThread.Join();
            }
            if (broadcastThread != null)
            {
                broadcastThread.Join();
            }
            // Close the application
            Application.Exit();
        }

        #region Questions
        private void button3_Click(object sender, EventArgs e)
        {
            string QuestionsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions");

            if (!Directory.Exists(QuestionsFolderPath))
            {
                Directory.CreateDirectory(QuestionsFolderPath);
                MessageBox.Show("Questions folder not found in app directory.", "Questions folder has been Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (string imageFileName in Directory.GetFiles(QuestionsFolderPath, "*.jpg", SearchOption.AllDirectories)
                                                       .Union(Directory.GetFiles(QuestionsFolderPath, "*.png", SearchOption.AllDirectories))
                                                       .Union(Directory.GetFiles(QuestionsFolderPath, "*.jpeg", SearchOption.AllDirectories)))
            {
                string imageName = Path.GetFileNameWithoutExtension(imageFileName);

                // Check if a PictureBox with the same name already exists
                if (flowLayoutPanel1.Controls.Find(imageName, true).Length == 0)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imageFileName);
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Image = image;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Size = new Size(200, 200);
                    pictureBox.Click += pictureBox1_Click; // Add click event for selecting images

                    // Set the PictureBox name
                    pictureBox.Name = imageName;

                    CheckBox checkBox = new CheckBox();
                    checkBox.Name = "selectionCheckbox";
                    checkBox.Visible = false; // Initially hide the checkbox
                    checkBox.Location = new Point(0, 0);
                    checkBox.Size = new Size(100, 30);
                    checkBox.Text = "Selected";
                    checkBox.ForeColor = Color.Red;
                    checkBox.Font = new Font("Arial", 12F, FontStyle.Bold);
                    pictureBox.Controls.Add(checkBox);
                    flowLayoutPanel1.Controls.Add(pictureBox);
                }
            }

            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("Questions folder is empty", "No Questions Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = (PictureBox)sender;
            CheckBox checkBox = (CheckBox)clickedPictureBox.Controls["selectionCheckbox"];

            if (checkBox.Checked)
            {
                // Deselect the image
                checkBox.Visible = false;
                selectedPictureBoxes.Remove(clickedPictureBox);
                checkBox.Checked = false;
                clickedPictureBox.BorderStyle = BorderStyle.None;
            }
            else
            {
                // Select the image
                checkBox.Visible = true;
                selectedPictureBoxes.Add(clickedPictureBox);
                checkBox.Checked = true;
                clickedPictureBox.BorderStyle = BorderStyle.FixedSingle;
                clickedPictureBox.BackColor = Color.MediumSeaGreen;

            }
        }
        #endregion

        #region Compititors
        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter Server Name First");
                return;
            }
            selectedClients = new List<ClientInfo>();
            ObserverClients = new List<ClientInfo>();
            if (!IsServerRunning())
            {
                label16.ForeColor = Color.MediumSeaGreen;
                label16.Text = "Server " + textBox1.Text + " Started";

                label16.AutoSize = false;

                // Calculate the center of the parent control
                int centerX = (this.ClientSize.Width - label16.Width) / 2;
                int centerY = (this.ClientSize.Height - label16.Height) / 2;

                // Set the label's location to the calculated center
                label16.Location = new Point(centerX, centerY);

                button6.Enabled = true;
                button7.Enabled = false;
                button7.Text = "Server Started...";
                flowLayoutPanel2.Controls.Clear();
                StartServer();
            }
            else
            {
                MessageBox.Show("Server is already running");
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ShutdownServer();
            // MessageBox.Show("Server shutdown successfully.");
            button7.Enabled = true;
            button6.Enabled = false;
            label16.ForeColor = Color.Red;
            label16.Text = "Server Stopped";
            label16.AutoSize = false;

            // Calculate the center of the parent control
            int centerX = (this.ClientSize.Width - label16.Width) / 2;
            int centerY = (this.ClientSize.Height - label16.Height) / 2;

            // Set the label's location to the calculated center
            label16.Location = new Point(centerX, centerY);
            button7.Text = "Start Server";
        }
        private void lbl_selectedClients_Click(object sender, EventArgs e)
        {
            Label checkBox2 = (Label)sender;
            //checkBox2.BackColor = checkBox2.Checked ? Color.MediumSeaGreen : SystemColors.Control; // Update background color
            if (checkBox2.BackColor == Color.LightBlue)
            {
                checkBox2.BackColor = Color.White;
            }
            else
            {
                checkBox2.BackColor = Color.LightBlue;
            }
            selectedClients.Clear(); // Clear existing data
            foreach (Control control in flowLayoutPanel2.Controls)
            {
                Label checkBox = control as Label;
                if (checkBox != null && checkBox.BackColor == Color.LightBlue)
                {
                    ClientInfo clientInfo = (ClientInfo)checkBox.Tag;

                    // Create a new ClientInfo object with the same data
                    ClientInfo newClientInfo = new ClientInfo
                    {
                        Id = clientInfo.Id,
                        Name = clientInfo.Name,
                        Address = clientInfo.Address,
                        Client = clientInfo.Client
                        // Add other properties as needed
                    };

                    // Add the new ClientInfo object to the selectedClients list
                    selectedClients.Add(newClientInfo);
                }
            }
            update_ready_panel();

            #region backup
            //CheckBox checkBox2 = (CheckBox)sender;
            //checkBox2.BackColor = checkBox2.Checked ? Color.MediumSeaGreen : SystemColors.Control; // Update background color

            //selectedClients.Clear(); // Clear existing data
            //foreach (Control control in flowLayoutPanel2.Controls)
            //{
            //    CheckBox checkBox = control as CheckBox;
            //    if (checkBox != null && checkBox.Checked)
            //    {
            //        ClientInfo clientInfo = (ClientInfo)checkBox.Tag;
            //        string clientData = $"{clientInfo.Id}<#>{clientInfo.Name}<#>{clientInfo.Address}";
            //        selectedClients.Add(clientData);
            //    }
            //} 
            #endregion
        }
        private void StartServer()
        {
            shouldStopBroadcasting = false;
            tcpListener = new TcpListener(IPAddress.Any, 12347);

            // Start listen server presence

            listenerThread = new Thread(new ThreadStart(ListenForClients));
            listenerThread.Start();

            // Start broadcasting server presence
            broadcastThread = new Thread(new ThreadStart(BroadcastServerPresence));
            broadcastThread.Start();

            // Start sending server alive message
            aliveMessageThread = new Thread(new ThreadStart(SendServerAliveMessage));
            aliveMessageThread.Start();

            //MessageBox.Show("Server started!");
        }
        private void ListenForClients()
        {
            tcpListener.Start();
            try
            {
                while (!shouldStopBroadcasting)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);

                    // Delay for 5 seconds
                    Thread.Sleep(5000);
                }
            }
            catch
            {

            }
        }
        private void BroadcastServerPresence()
        {
            #region MyRegion
            UdpClient udpClient = new UdpClient();
            {
                IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, 12348);
                while (!shouldStopBroadcasting)
                {
                    try
                    {
                        string serverInfo = "";
                        if (textBox1.Text == "")
                        {
                            serverInfo = "Server: " + Dns.GetHostName();
                        }
                        else
                        {
                            Server_name = textBox1.Text;
                            serverInfo = textBox1.Text + ": " + Dns.GetHostName(); // Use textBox1 for server name
                        }
                        byte[] data = Encoding.UTF8.GetBytes(serverInfo);
                        udpClient.Send(data, data.Length, broadcastEndpoint);
                        Thread.Sleep(5000); // Broadcast every 5 seconds (adjust as needed)
                    }
                    catch
                    {

                    }
                }
            }
            #endregion
        }
        private async void HandleClientComm(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream clientStream = tcpClient.GetStream();
            string userName = "";
            // Get client information (you can customize this part)
            string clientId = Guid.NewGuid().ToString();
            byte[] message = new byte[4096];
            int bytesRead;
            try
            {
                while (!shouldStopBroadcasting)
                {
                    bytesRead = 0;
                    try
                    {
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        // break;
                    }

                    if (bytesRead == 0)
                    {
                        // break; // Client has disconnected
                    }

                    string receivedMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                    if (receivedMessage.Contains("Ready"))
                    {
                        string[] parts = receivedMessage.Split(':');
                        string clientName = parts[1];
                        bool labelFound = false;

                        // Use Invoke to ensure the UI update is done on the UI thread
                        flowLayoutPanel4.Invoke((MethodInvoker)delegate
                        {
                            // Iterate through each panel in flowLayoutPanel4
                            foreach (Control control in flowLayoutPanel4.Controls)
                            {
                                if (control is Panel panel)
                                {
                                    // Find the label in the current panel
                                    Label label = panel.Controls.OfType<Label>().FirstOrDefault();
                                    PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();

                                    if (label != null && pictureBox != null && clientName.Contains(label.Text))
                                    {
                                        // Update the PictureBox in the existing panel
                                        labelFound = true;
                                        // Assuming "ready.png" is an image in your application resources
                                        pictureBox.Image = Properties.Resources.pngegg__3_;
                                        break;
                                    }
                                }
                            }

                            // If the label was not found, create a new panel
                            if (!labelFound)
                            {
                                // Create a new panel
                                Panel newPanel = new Panel();
                                newPanel.Size = new Size(819, 60);

                                // Create a new label
                                Label newLabel = new Label();
                                newLabel.AutoSize = false;
                                newLabel.Location = new Point(65, 2);
                                newLabel.Font = new System.Drawing.Font("Sakkal Majalla", 27f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                newLabel.Text = clientName; // Use clientInfo.Name here
                                Size textSize = TextRenderer.MeasureText(newLabel.Text, newLabel.Font);
                                newLabel.Size = new Size(textSize.Width + 20, textSize.Height + 10);
                                newLabel.TextAlign = ContentAlignment.MiddleLeft;

                                // Create a new picture box and set the image
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Image = Properties.Resources.pngegg__3_;
                                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox.Location = new Point(10, 6);
                                pictureBox.Size = new Size(40, 40);

                                // Add label and picture box to the new panel
                                newPanel.Controls.Add(newLabel);
                                newPanel.Controls.Add(pictureBox);

                                // Add the new panel to flowLayoutPanel4
                                flowLayoutPanel4.Controls.Add(newPanel);
                            }



                            int counter = 0;
                            int ready = 0; ;
                            foreach (Control control in flowLayoutPanel4.Controls)
                            {
                                if (control is Panel panel)
                                {
                                    PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();
                                    if (pictureBox.Image != Properties.Resources.pngegg__3_)
                                    {
                                        ready++;
                                    }
                                    counter++;
                                }
                            }
                            label52.Text = ready.ToString() + "/" + counter.ToString();
                            if (ready == counter && readyflag == true)
                            {
                                button1.Enabled = true;
                            }
                            else
                            {
                                button1.Enabled = false;
                            }
                        });
                    }
                    else if (receivedMessage.Contains("Submit"))
                    {
                        foreach (var clientInfo in ObserverClients)
                        {
                            await Task.Run(() => SendMessageToUserAsync(receivedMessage, "", clientInfo.Client));
                        }
                        receivedMessage = receivedMessage.Replace("Submit:", "");
                        results.Add(receivedMessage);
                        string[] parts = receivedMessage.Split("<#>");
                        string Comp_Names = parts[0];
                        string pnts = parts[4];
                        pnts = pnts.Replace("\r", "");
                        pnts = pnts.Replace("\n", "");
                        // Search and update the list
                        for (int i = 0; i < resultList.Count; i++)
                        {
                            if (resultList[i].Item1.Contains(Comp_Names))
                            {
                                resultList[i] = (resultList[i].Item1, Convert.ToInt32(pnts));
                            }
                        }
                        // Use Invoke to ensure the UI update is done on the UI thread
                        flowLayoutPanel4.Invoke((MethodInvoker)delegate
                        {
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
                            label25.Text = count.ToString() + "/" + all.ToString();
                        });
                    }
                    else if (receivedMessage.Contains("Ansr"))
                    {
                        foreach (var clientInfo in ObserverClients)
                        {
                            await Task.Run(() => SendMessageToUserAsync(receivedMessage, "", clientInfo.Client));
                        }
                        receivedMessage = receivedMessage.Replace("Ansr:", "");
                        results.Add(receivedMessage);
                        string[] parts = receivedMessage.Split("<#>");
                        string Comp_Names = parts[0];
                        string pnts = parts[4];
                        pnts = pnts.Replace("\r", "");
                        pnts = pnts.Replace("\n", "");
                        // Search and update the list
                        for (int i = 0; i < resultList.Count; i++)
                        {
                            if (resultList[i].Item1.Contains(Comp_Names))
                            {
                                resultList[i] = (resultList[i].Item1, Convert.ToInt32(pnts));
                            }
                        }
                        // Use Invoke to ensure the UI update is done on the UI thread
                        flowLayoutPanel4.Invoke((MethodInvoker)delegate
                        {
                            // Iterate through each panel in flowLayoutPanel4
                            foreach (Control control in flowLayoutPanel3.Controls)
                            {
                                if (control is Panel panel)
                                {
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
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        });
                    }
                    else if (receivedMessage != "" && !receivedMessage.Contains("Ready"))
                    {
                        userName = receivedMessage;
                        // Check if a client with the same name already exists
                        if (!connectedClients.Any(c => c.Name == userName))
                        {
                            // Add the client to the connectedClients list
                            connectedClients.Add(new ClientInfo { Id = clientId, Name = userName, Client = tcpClient });
                            
                            // Update UI on the main thread
                            Invoke(new Action(() =>
                            {
                                UpdateUI();
                            }));
                        }
                        if (!ObserverClients.Any(c => c.Name == userName) && userName.Contains("Observer"))
                        {
                            ObserverClients.Add(new ClientInfo { Id = clientId, Name = userName, Client = tcpClient });
                            //selectedClients.Add(new ClientInfo { Id = clientId, Name = userName, Client = tcpClient });
                        }
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (optional)
                MessageBox.Show($"Error handling client communication: {ex.Message}");
            }
            finally
            {
                // Client has disconnected, remove it from connectedClients list
                var disconnectedClient = connectedClients.FirstOrDefault(c => c.Client == tcpClient);
                if (disconnectedClient != null)
                {
                    connectedClients.Remove(disconnectedClient);
                }

                try
                {
                    // Update UI on the main thread
                    Invoke(new Action(() =>
                    {
                        UpdateUI();
                    }));
                }
                catch (Exception)
                {

                }

                // Close the TCP client
                tcpClient.Close();
            }
        }
       
        private void update_ready_panel()
        {
            // Find the label in flowLayoutPanel4 in selectedClients If not ready delete
            foreach (Control control in flowLayoutPanel4.Controls)
            {
                if (control is Panel panel)
                {
                    Label label = panel.Controls.OfType<Label>().FirstOrDefault();
                    bool foundMatch = false;
                    foreach (var namesss in selectedClients)
                    {
                        if (namesss.Name.Trim().Contains(label.Text.Trim()))
                        {
                            foundMatch = true;
                            break;
                        }
                    }
                    if (!foundMatch)
                    {
                        // If not ready, delete the panel
                        panel.Dispose();
                    }
                }
            }
            // Find the label and pictureBox in flowLayoutPanel4 in selectedClients If not ready delete
            foreach (var clientData in selectedClients)
            {
                if (!clientData.Name.Contains("Observer"))
                {
                    // Extract clientName pieces of information
                    bool flagg = true;

                    foreach (Control control in flowLayoutPanel4.Controls)
                    {
                        if (control is Panel panel)
                        {
                            // Find the label in flowLayoutPanel4 
                            Label label = panel.Controls.OfType<Label>().FirstOrDefault();
                            PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();

                            if (label != null && label.Text == clientData.Name)
                            {
                                flagg = false;
                            }
                        }
                    }
                    if (flagg)
                    {
                        // Create a new panel
                        Panel newPanel = new Panel();
                        newPanel.Size = new Size(819, 60);

                        // Create a new label
                        Label newLabel = new Label();
                        newLabel.AutoSize = false;
                        newLabel.Location = new Point(65, 2);
                        newLabel.Font = new System.Drawing.Font("Sakkal Majalla", 27f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        newLabel.Text = clientData.Name; // Use clientInfo.Name here
                        Size textSize = TextRenderer.MeasureText(newLabel.Text, newLabel.Font);
                        newLabel.Size = new Size(textSize.Width + 20, textSize.Height + 10);

                        newLabel.TextAlign = ContentAlignment.MiddleLeft;

                        // Create a new picture box and set the image
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Image = Properties.Resources.pngegg__4_;
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Location = new Point(10, 6);
                        pictureBox.Size = new Size(40, 40);

                        // Add label and picture box to the new panel
                        newPanel.Controls.Add(newLabel);
                        newPanel.Controls.Add(pictureBox);

                        // Add the new panel to flowLayoutPanel4
                        flowLayoutPanel4.Controls.Add(newPanel);
                    }
                }
                    
            }

            int counter = 0;
            int ready = 0; ;
            foreach (Control control in flowLayoutPanel4.Controls)
            {
                if (control is Panel panel)
                {
                    PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();
                    if (pictureBox.Image == Properties.Resources.pngegg__3_)
                    {
                        ready++;
                    }
                    counter++;
                }
            }
            label52.Text = ready.ToString() + "/" + counter.ToString();
            if (ready == counter && readyflag == true)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        private void UpdateUI()
        {
            // Clear existing controls in FlowLayoutPanel
            flowLayoutPanel2.Controls.Clear();

            // Add new controls for each connected client, enabling multi-selection
            foreach (var client in connectedClients)
            {
                if(!client.Name.Contains("Observer"))
                {
                    Label checkBox = new Label();
                    checkBox.AutoSize = false;
                    checkBox.Font = new System.Drawing.Font("Sakkal Majalla", 27f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    checkBox.Text = ($"{client.Name}").Trim();
                    Size textSize = TextRenderer.MeasureText(checkBox.Text, checkBox.Font);
                    checkBox.Size = new Size(textSize.Width + 20, textSize.Height + 6);
                    checkBox.Tag = client; // Store client object for later access
                    checkBox.TextAlign = ContentAlignment.MiddleCenter;

                    // Check if the client is in selectedClients
                    if (selectedClients.Any(selectedClient => selectedClient.Id.Contains(client.Id)))
                    {
                        checkBox.BackColor = Color.LightBlue;
                        //checkBox.BackColor = checkBox.Checked ? Color.MediumSeaGreen : SystemColors.Control; // Update background color
                    }

                    checkBox.Click += lbl_selectedClients_Click; // Attach event handler

                    flowLayoutPanel2.Controls.Add(checkBox);
                }
                #region backup
                //CheckBox checkBox = new CheckBox();
                //// Increase the size of the checkbox box
                //// checkBox.ClientSize = new Size(50, 50);
                //checkBox.Font = new System.Drawing.Font("Sakkal Majalla", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //checkBox.Text = $"{client.Name}";
                //Size textSize = TextRenderer.MeasureText(checkBox.Text, checkBox.Font);
                //checkBox.Size = new Size(textSize.Width + 20, textSize.Height + 6);
                //checkBox.Tag = client; // Store client object for later access


                //// Check if the client is in selectedClients
                //if (selectedClients.Any(selectedClient => selectedClient.Contains(client.Id)))
                //{
                //    checkBox.Checked = true;
                //    checkBox.BackColor = checkBox.Checked ? Color.MediumSeaGreen : SystemColors.Control; // Update background color
                //}

                //checkBox.CheckedChanged += CheckBox_CheckedChanged; // Attach event handler

                //flowLayoutPanel2.Controls.Add(checkBox); 
                #endregion
            }
        }
        private bool IsServerRunning()
        {
            try
            {
                // Try to bind to the server's address and port
                TcpListener testListener = new TcpListener(IPAddress.Any, 12347);
                testListener.Start();
                testListener.Stop();

                // If binding is successful, no server is running
                return false;
            }
            catch (SocketException)
            {
                // If binding fails, another process is using the address and port
                return true;
            }
        }
        private void ShutdownServer()
        {
            try
            {
                shouldStopBroadcasting = true; // Signal the thread to stop

                try
                {
                    if (tcpListener != null)
                    {
                        // Stop listening for new clients
                        tcpListener.Stop();
                    }

                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error  Stop listening for new clients: {ex.Message}");
                }
                // Disconnect existing clients gracefully
                try
                {
                    foreach (TcpClient client in connectedClients.Select(c => c.Client))
                    {
                        try
                        {
                            client.Close();
                        }
                        catch (Exception ex)
                        {
                            // Log or handle exception if client disconnection fails
                            //MessageBox.Show($"Error disconnecting client: {ex.Message}");
                        }
                    }
                }
                catch (Exception)
                {

                }
                try
                {
                    // Check if the broadcastThread is active before waiting for it to finish
                    if (broadcastThread != null && broadcastThread.IsAlive)
                    {
                        broadcastThread.Join(); // Wait for it to finish gracefully
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error Waiting to finish: {ex.Message}");
                }
                try
                {
                    // Clear the client list
                    connectedClients = new List<ClientInfo>();
                    selectedClients = new List<ClientInfo>();
                    ObserverClients = new List<ClientInfo>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Clearing: {ex.Message}");
                }
                try
                {
                    // Update UI to reflect server shutdown
                    Invoke(new Action(() =>
                    {
                        UpdateUI();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Updating UI to reflect server shutdown: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error shutting down server: {ex.Message}");
            }
        }
        private void SendMessageToUser(string message, TcpClient userTcpClient)
        {
            try
            {
                NetworkStream stream = userTcpClient.GetStream();
                StreamWriter writer = new StreamWriter(stream);

                // Send the message to the user
                writer.WriteLine($"MESSAGE:{message}\n");
                writer.Flush();
            }
            catch (Exception ex)
            {
                // Remove the user from connectedClients and selectedClients using the client's name
                ClientInfo connected_clientToRemove = connectedClients.FirstOrDefault(client => client.Client == userTcpClient);
                ClientInfo selected_userNameToRemove = selectedClients.FirstOrDefault(client => client.Client == userTcpClient);
                ClientInfo observers = ObserverClients.FirstOrDefault(client => client.Client == userTcpClient);

                try
                {
                    if (selected_userNameToRemove != null)
                    {
                        MessageBox.Show(selected_userNameToRemove.Name.Replace("\r\n", "").Trim() + " Has been Disconnected");
                    }
                }
                catch (Exception)
                {
                    
                }

                if (connected_clientToRemove != null)
                {
                    connectedClients.Remove(connected_clientToRemove);
                }
                if (selected_userNameToRemove != null)
                {
                    selectedClients.Remove(selected_userNameToRemove);
                }
                if (observers != null)
                {
                    ObserverClients.Remove(observers);
                }


                // Update UI on the main thread
                Invoke(new Action(() =>
                {
                    UpdateUI();
                }));                
                // Update UI on the main thread
                Invoke(new Action(() =>
                {
                    update_ready_panel();
                }));
            }
        }
        private void SendServerAliveMessage()
        {
            while (!shouldStopBroadcasting)
            {
                try
                {
                    Thread.Sleep(7000); // Send the message every 5 seconds (adjust as needed)

                    string serverAliveMessage = "SERVER_ALIVE";

                    // Iterate through connected clients and send the message
                    foreach (var clientInfo in selectedClients)
                    {
                        SendMessageToUser($"{serverAliveMessage}", clientInfo.Client);
                    }
                }
                catch (Exception)
                {

                   
                }
            }
        }
        #endregion


        #region Start_Quiz
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = radioButton1.Checked;
            label5.Enabled = radioButton1.Checked;
            textBox3.Text = "";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = radioButton2.Checked;
            label3.Enabled = radioButton2.Checked;
            textBox2.Text = "";
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox5.Enabled = radioButton6.Checked;
            label10.Enabled = radioButton6.Checked;
            textBox5.Text = "";
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = radioButton5.Checked;
            label7.Enabled = radioButton5.Checked;
            textBox4.Text = "";
        }
        bool readyflag = false;
        private void button10_Click(object sender, EventArgs e)
        {
            if (selectedPictureBoxes.Count > 0 && selectedClients.Count > 0)
            {
                #region MyRegion
                // Check Time System
                if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
                {
                    if (radioButton1.Checked == true && textBox3.Text == "")
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
                        coll = coll + "Each<#>" + textBox3.Text;
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
                    if (radioButton6.Checked == true && textBox5.Text == "")
                    {
                        MessageBox.Show("Please Write The Points (For Each Correct Answer) First"); return;
                    }
                    if (radioButton5.Checked == true && textBox4.Text == "")
                    {
                        MessageBox.Show("Please Write The Points (For Each Wrong Answer) First"); return;
                    }
                    coll = coll + "<##>Points<#>";
                    if (radioButton6.Checked)
                    {
                        coll = coll + "Correct<#>" + textBox5.Text;
                    }
                    if (radioButton5.Checked && radioButton6.Checked)
                    {
                        coll = coll + "<#>Wrong<#>" + textBox4.Text;
                    }
                    else if (radioButton5.Checked)
                    {
                        coll = coll + "Wrong<#>" + textBox4.Text;
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
                SendSelectedPictureBoxesOverNetwork();
                update_ready_panel();
                tabControl1.SelectedIndex = 3;
                readyflag = true;
            }
            else
            {
                MessageBox.Show("Please Select Questions and Competitors First");
            }
        }

        private async Task SendSelectedPictureBoxesOverNetwork()
        {
            //byte[] sad = new byte[16];

            // Get the names of the selected images
            #region Random
            List<string> imageNames = selectedPictureBoxes.Select(pb => pb.Name).ToList();

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
            #endregion


            shouldStopBroadcasting = true;
            broadcastThread.Join();
            aliveMessageThread.Join();

            string selectedClients_Names = "";
            if (selectedClients.Count > 0)
            {
                foreach (var clientInfo in selectedClients)
                {
                    resultList.Add((clientInfo.Name,0));
                    selectedClients_Names = selectedClients_Names + clientInfo.Name + "<#>";

                    foreach (var pictureBox in selectedPictureBoxes)
                    {
                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", pictureBox.Name + ".png");
                        await SendImageDataAsync("IMAGEDATA:<#>" + pictureBox.Name + ".png", imagePath, clientInfo.Client);
                    }
                    await SendImageDataAsync("Names:" + selectedClients_Names, "", clientInfo.Client);
                    await SendImageDataAsync("Finished" + "<##>" + coll + "<##>" + result, "", clientInfo.Client);
                    confi = "Finished" + "<##>" + coll + "<##>" + result;
                }
            }
            if (ObserverClients.Count > 0)
            {
                foreach (var clientInfo in ObserverClients)
                {
                    foreach (var pictureBox in selectedPictureBoxes)
                    {
                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", pictureBox.Name + ".png");
                        await SendImageDataAsync("IMAGEDATA:<#>" + pictureBox.Name + ".png", imagePath, clientInfo.Client);
                    }
                    await SendImageDataAsync("Names:" + selectedClients_Names, "", clientInfo.Client);
                    await SendImageDataAsync("Finished" + "<##>" + coll + "<##>" + result, "", clientInfo.Client);
                }
            }

            shouldStopBroadcasting = false;
            broadcastThread = new Thread(new ThreadStart(BroadcastServerPresence));
            broadcastThread.Start();

            // Start sending server alive message
            aliveMessageThread = new Thread(new ThreadStart(SendServerAliveMessage));
            aliveMessageThread.Start();
        }
        private async Task SendImageDataAsync(string imageName, string imageDataString, TcpClient client)
        {
            // Send image data
            await SendMessageToUserAsync($"IMAGE:{imageName}", imageDataString, client);
        }
        private async Task SendMessageToUserAsync(string message, string imageData2, TcpClient clientInfo)
        {
            try
            {
                byte[] imageData = new byte[0];
                string hexString = "";
                if (imageData2.Length > 1)
                {
                    imageData = File.ReadAllBytes(imageData2);
                    hexString = BitConverter.ToString(imageData).Replace("-", "");
                }

                NetworkStream stream = clientInfo.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                string meds = message + "<#>" + hexString;
                if (message.Contains("IMAGEDATA"))
                {
                    meds = meds + "\r\n\r";
                }
                // Send the hexadecimal-encoded image data
                await writer.WriteLineAsync($"{meds}\n");
                await writer.FlushAsync();

                //// Send the Base64 string as a single line
                // writer.Write($"{message + "<#>" + hexString + "\n"}");
                // writer.Flush();

                // Send the hexadecimal-encoded image data
                //await writer.WriteLineAsync($"{message + imageData2}\n");
                //await writer.FlushAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message to user: {ex.Message}");
            }
        }
        private string ConvertPictureBoxToBase64String(PictureBox pictureBox,string pictureBoxxName)
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", pictureBoxxName);

            //byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
            //var base64String = Convert.ToBase64String(imageArray);
            //return base64String;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);

                    // Validate the Base-64 string
                    //if (!Regex.IsMatch(base64String, @"^[A-Za-z0-9+/]*={0,2}$"))
                    //{
                    //    throw new ArgumentException("Invalid Base-64 string");
                    //}

                    //// Remove any potential URL-unsafe characters
                    //base64String = base64String.Replace('+', '-')
                    //                           .Replace('/', '_');

                    // Ensure correct padding (multiples of 4 characters)
                    //int padding = (base64String.Length % 4);
                    //if (padding > 0)
                    //{
                    //    base64String += new string('=', 4 - padding);
                    //}

                    //string converted = base64String.Replace('-', '+');
                    //converted = converted.Replace('_', '/');

                    //Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                    //converted = regex.Replace(converted, string.Empty);
                    return base64String;
                }
            }

            //MemoryStream memoryStream = new MemoryStream();
            //// Save PictureBox image to MemoryStream
            //pictureBox.Image.Save(memoryStream, ImageFormat.Png);

            //// Convert MemoryStream to byte array
            //byte[] imageData = memoryStream.ToArray();

            //// Convert byte array to Base64 string
            //base64String = Convert.ToBase64String(imageData);

            ////try
            ////{
            ////    int padding = (base64String.Length % 4);
            ////    if (padding > 0)
            ////    {
            ////        base64String += new string('=', 4 - padding);
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    MessageBox.Show("ConvertPictureBoxToBase64String " + ex);
            ////}
            //base64String = base64String.Replace('+', '-').Replace('/', '_');

            //// Remove non-base64 characters (e.g., line breaks) from the string
            //base64String = RemoveNonBase64Characters(base64String);

            //
        }
        private string RemoveNonBase64Characters(string input)
        {
            // Remove characters that are not part of the Base64 character set
            return new string(input.Where(c => char.IsLetterOrDigit(c) || c == '+' || c == '/' || c == '=').ToArray());
        }
        private async Task SendMessageToUserAsync2(string message, string imageDataString, TcpClient userTcpClient)
        {
            try
            {
                if (userTcpClient != null && userTcpClient.Connected)
                {
                    NetworkStream stream = userTcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(stream);
                    message = message + "<#>" + imageDataString;

                    // Send the Base64 string as a single line
                    await writer.WriteLineAsync($"{message}\n");
                    await writer.FlushAsync();
                }
                else
                {
                    //MessageBox.Show("The TcpClient is not connected.");
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error sending message to user: {ex.Message}");
            }
        }
        private async Task SendMessageToUserAsync(List<string> pictureBoxxName, List<string> imageDataStrings, TcpClient userTcpClient)
        {
            try
            {
                if (userTcpClient != null && userTcpClient.Connected)
                {
                    NetworkStream stream = userTcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(stream);
                    string message = "";
                    for (int i = 0; i < pictureBoxxName.Count; i++)
                    {
                        message = $"IMAGE:" + pictureBoxxName[i] + "<#>" + imageDataStrings[i];
                        // Send the Base64 string as a single line
                        await writer.WriteLineAsync($"{message}\n");
                        await writer.FlushAsync();
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
        #endregion
        int dsf = 0;
        

        private async void timer1_Tick(object sender, EventArgs e)
        {
            // Parse the current time from count_Label
            TimeSpan currentTime = TimeSpan.Parse(label24.Text);

            // Add one second to the current time
            currentTime = currentTime.Add(TimeSpan.FromSeconds(-1));

            // Update count_Label with the new time
            label24.Text = currentTime.ToString(@"hh\:mm\:ss");

            // Parse the current time from count_Label
            TimeSpan currentTim2 = TimeSpan.Parse(label26.Text);

            // Add one second to the current time
            currentTim2 = currentTim2.Add(TimeSpan.FromSeconds(1));

            // Update count_Label with the new time
            label26.Text = currentTim2.ToString(@"hh\:mm\:ss");

            try
            {
                if (currentTime.ToString() == "00:00:00")
                {
                    try
                    {
                        if (currentImageIndex < 0)
                        {
                            currentImageIndex = 0;
                        }
                        if (currentImageIndex < Questionss.Length && button12.Text != "Finish")
                        {
                            if (currentImageIndex == Questionss.Length - 2)
                            {
                                button12.Text = "Finish";
                            }

                            currentImageIndex++;

                            string formattedNumber = (pngFileCount.ToString().Length == 2) ? (currentImageIndex + 1).ToString("D2") : (currentImageIndex + 1).ToString("D1");
                            label27.Text = formattedNumber + "/" + pngFileCount.ToString();

                            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", Questionss[currentImageIndex]);
                            pictureBox5.Image = System.Drawing.Image.FromFile(imagePath + ".png");


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
                                                lbl.ForeColor = Color.Red;
                                                lbl.Text = "No";
                                            }
                                            if (lbl.Name == "Select2")
                                            {
                                                lbl.Text = "-";
                                            }
                                        }
                                    }
                                }
                            }

                            // The checked RadioButton is found
                            string fileName = pictureBox2.Name;

                            // Find the last occurrence of "_" in the string
                            int lastIndex = fileName.LastIndexOf('_');

                            if (lastIndex != -1 && lastIndex < fileName.Length - 1)
                            {
                                label28.Text = fileName[lastIndex + 1].ToString();
                            }


                            if (timeeary[1].Contains("Each"))
                            {
                                Timee = -1;
                                put_time(Convert.ToInt32(timeeary[2]));
                            }
                        }
                        else
                        {
                            Quiz_Finished();
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception)
            {
               
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog instance
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Set properties to allow multiple file selection and filter for image files
                Multiselect = true,
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files|*.*"
            };

            // Show the dialog and check if the user clicked OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Create a directory named "Questions" in the application directory
                string destinationFolder = Path.Combine(Application.StartupPath, "Questions");

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                try
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        // Load the image
                        System.Drawing.Image image = System.Drawing.Image.FromFile(filePath);

                        // Get the file name without the path and change the extension to .png
                        string fileName = Path.GetFileNameWithoutExtension(filePath) + ".png";

                        // Build the destination path
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        // Save the image as PNG
                        image.Save(destinationPath, ImageFormat.Png);

                        // Extract the filename without extension and set it as PictureBox name
                        string imageName = Path.GetFileNameWithoutExtension(fileName);

                        // Check if a PictureBox with the same name already exists
                        if (flowLayoutPanel1.Controls.Find(imageName, true).Length == 0)
                        {
                            // Create a new PictureBox and add it to flowLayoutPanel1
                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Image = image;
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox.Size = new Size(200, 200);
                            pictureBox.Click += pictureBox1_Click; // Add click event for selecting images
                            pictureBox.Name = imageName;

                            CheckBox checkBox = new CheckBox();
                            checkBox.Name = "selectionCheckbox";
                            checkBox.Visible = false; // Initially hide the checkbox
                            checkBox.Location = new Point(0, 0);
                            checkBox.Size = new Size(100, 30);
                            checkBox.Text = "Selected";
                            checkBox.ForeColor = Color.Red;
                            checkBox.Font = new Font("Arial", 12F, FontStyle.Bold);
                            pictureBox.Controls.Add(checkBox);
                            flowLayoutPanel1.Controls.Add(pictureBox);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., file format not supported)
                    MessageBox.Show("Error loading images: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //button1.Enabled = true;
            //tabControl1.SelectedTab.HeaderForeColor = newColor;

            //if (tabControl1.SelectedIndex == 2)
            //{
            //    if (selectedPictureBoxes.Count > 0 && connectedClients.Count > 0)
            //    {
            //        button10.Enabled = true;
            //        button10.BackColor = Color.MediumSeaGreen;
            //    }
            //    else
            //    {
            //        button10.ForeColor = Color.Black;
            //        button10.Enabled = false;
            //    }
            //}
            //if (tabControl1.SelectedIndex == 3)
            //{
            //    if (selectedPictureBoxes.Count > 0 && connectedClients.Count > 0)
            //    {
            //        button1.Enabled = true;
            //        button1.BackColor = Color.MediumSeaGreen;
            //    }
            //    else
            //    {
            //        button1.ForeColor = Color.Black;
            //        button1.Enabled = false;
            //    }
            //}
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            button11.Enabled = false;
            foreach(var selectedClientsss in selectedClients)
            {
                await Task.Run(() => SendMessageToUser("Skip", selectedClientsss.Client));
            }
            foreach (var ObserverClientsss in ObserverClients)
            {
                await Task.Run(() => SendMessageToUser("Skip", ObserverClientsss.Client));
            }

            try
            {
                if (currentImageIndex < 0)
                {
                    currentImageIndex = 0;
                }
                if (currentImageIndex < Questionss.Length && button12.Text != "Finish")
                {
                    if (currentImageIndex == Questionss.Length - 2)
                    {
                        button12.Text = "Finish";
                    }

                    currentImageIndex++;

                    string formattedNumber = (pngFileCount.ToString().Length == 2) ? (currentImageIndex + 1).ToString("D2") : (currentImageIndex + 1).ToString("D1");
                    label27.Text = formattedNumber + "/" + pngFileCount.ToString();

                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", Questionss[currentImageIndex]);
                    pictureBox5.Image = System.Drawing.Image.FromFile(imagePath + ".png");


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
                                        lbl.ForeColor = Color.Red;
                                        lbl.Text = "No";
                                    }
                                    if (lbl.Name == "Select2")
                                    {
                                        lbl.Text = "-";
                                    }
                                }
                            }
                        }
                    }

                    // The checked RadioButton is found
                    string fileName = pictureBox2.Name;

                    // Find the last occurrence of "_" in the string
                    int lastIndex = fileName.LastIndexOf('_');

                    if (lastIndex != -1 && lastIndex < fileName.Length - 1)
                    {
                        label28.Text = fileName[lastIndex + 1].ToString();
                    }


                    if (timeeary[1].Contains("Each"))
                    {
                        Timee = -1;
                        put_time(Convert.ToInt32(timeeary[2]));
                    }
                }
                else
                {
                    Quiz_Finished();

                }
                button11.Enabled = true;
            }
            catch (Exception)
            {

            }
        }
        private async void button12_Click(object sender, EventArgs e)
        {
            button12.Enabled = false;
            foreach (var selectedClientsss in selectedClients)
            {
                await Task.Run(() => SendMessageToUser("Next", selectedClientsss.Client));
            }
            foreach (var ObserverClientsss in ObserverClients)
            {
                await Task.Run(() => SendMessageToUser("Next", ObserverClientsss.Client));
            }
            try
            {
                if (currentImageIndex < 0)
                {
                    currentImageIndex = 0;
                }
                if (currentImageIndex < Questionss.Length && button12.Text != "Finish")
                {
                    if (currentImageIndex == Questionss.Length - 2)
                    {
                        button12.Text = "Finish";
                    }

                    currentImageIndex++;

                    string formattedNumber = (pngFileCount.ToString().Length == 2) ? (currentImageIndex + 1).ToString("D2") : (currentImageIndex + 1).ToString("D1");
                    label27.Text = formattedNumber + "/" + pngFileCount.ToString();

                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", Questionss[currentImageIndex]);
                    pictureBox5.Image = System.Drawing.Image.FromFile(imagePath + ".png");


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
                                        lbl.ForeColor = Color.Red;
                                        lbl.Text = "No";
                                    }
                                    if (lbl.Name == "Select2")
                                    {
                                        lbl.Text = "-";
                                    }
                                }
                            }
                        }
                    }

                    // The checked RadioButton is found
                    string fileName = pictureBox2.Name;

                    // Find the last occurrence of "_" in the string
                    int lastIndex = fileName.LastIndexOf('_');

                    if (lastIndex != -1 && lastIndex < fileName.Length - 1)
                    {
                        label28.Text = fileName[lastIndex + 1].ToString();
                    }


                    if (timeeary[1].Contains("Each"))
                    {
                        Timee = -1;
                        put_time(Convert.ToInt32(timeeary[2]));
                    }
                }
                else
                {
                    Quiz_Finished();
                }
                button12.Enabled = true;
            }
            catch (Exception)
            {

            }
        }
        private async void Quiz_Finished()
        {
            //string result = "";
            try
            {
                // Sort the list by the int value in descending order
                resultList = resultList.OrderByDescending(item => item.Item2).ToList();

                 result = string.Join("<##>", resultList.Select(item => $"{item.Item1}<#>{item.Item2}"));
                result = result.Replace("\n", "");
                result = result.Replace("\r", "");

                foreach (var clientInfo in selectedClients)
                {
                    await SendImageDataAsync("Results:" + result, "", clientInfo.Client);
                }
                foreach (var clientInfo in ObserverClients)
                {
                    await SendImageDataAsync("Results:" + result, "", clientInfo.Client);
                }
            }
            catch (Exception)
            {

                throw;
            }
            currentImageIndex = 0;
            readyflag = false;
            button12.Text = "Next";
            timer1.Stop();
            MessageBox.Show("Quiz Has Finished");
            tabControl1.SelectedIndex = 5;

            string[] parts3 = result.Split(new[] { "<##>" }, StringSplitOptions.None);
            flowLayoutPanel5.Controls.Clear();
            if (parts3.Length > 0)
            {
                int count = 1;
                foreach (string part2 in parts3)
                {
                    string[] parts = part2.Split(new[] { "<#>" }, StringSplitOptions.None);
                    if (!parts[0].Contains("Observer"))
                    {
                        Panel newPanel = new Panel();
                        newPanel.Size = new Size(788, 68);

                        Label newLabel = new Label();
                        newLabel.AutoSize = false;
                        newLabel.Location = new Point(11, 3);
                        newLabel.Font = new System.Drawing.Font("Sakkal Majalla", 27f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        newLabel.Text = count + "-";
                        newLabel.Size = new Size(50, 63);
                        newLabel.TextAlign = ContentAlignment.MiddleCenter;
                        count++;

                        Label newLabel1 = new Label();
                        newLabel1.AutoSize = false;
                        newLabel1.Location = new Point(622, 3);
                        newLabel1.Font = new System.Drawing.Font("Sakkal Majalla", 27f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        newLabel1.Text = parts[1] + "Points"; //Points
                        newLabel1.Size = new Size(163, 63);
                        newLabel1.TextAlign = ContentAlignment.MiddleCenter;

                        Label newLabel2 = new Label();
                        newLabel2.AutoSize = false;
                        newLabel2.Location = new Point(67, 3);
                        newLabel2.Font = new System.Drawing.Font("Sakkal Majalla", 27f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        parts[0] = parts[0].Replace("IMAGE:", "");
                        parts[0] = parts[0].Replace("Results:", "");
                        newLabel2.Text = parts[0]; //Name 
                        newLabel2.Size = new Size(549, 63);
                        newLabel2.TextAlign = ContentAlignment.MiddleLeft;

                        newPanel.Controls.Add(newLabel);
                        newPanel.Controls.Add(newLabel1);
                        newPanel.Controls.Add(newLabel2);
                        flowLayoutPanel5.Controls.Add(newPanel);
                    }

                }
            }

        }
        private async void button13_Click(object sender, EventArgs e)
        {
            try
            {
                button13.Enabled = false;
                foreach (var selectedClientsss in selectedClients)
                {
                    await Task.Run(() => SendMessageToUser("Exit", selectedClientsss.Client));
                }
                foreach (var ObserverClientsss in ObserverClients)
                {
                    await Task.Run(() => SendMessageToUser("Exit", ObserverClientsss.Client));
                }
                readyflag = false;
                timer1.Stop();
                MessageBox.Show("Quiz Has Aborted");
                button13.Enabled = true;
                // Set flags to stop threads
                shouldStopBroadcasting = true;

                ShutdownServer();

                if (listenerThread != null)
                {
                    // Wait for threads to finish
                    listenerThread.Join();
                }
                if (broadcastThread != null)
                {
                    broadcastThread.Join();
                }
                // Close the application
                Application.Exit();
            }
            catch (Exception)
            {
                // Set flags to stop threads
                shouldStopBroadcasting = true;

                ShutdownServer();

                if (listenerThread != null)
                {
                    // Wait for threads to finish
                    listenerThread.Join();
                }
                if (broadcastThread != null)
                {
                    broadcastThread.Join();
                }
                // Close the application
                Application.Exit();

            }
        }

        int Timee = 1;
        private string[] timeeary;
        private string[] Pointss;
        private string[] Answerss;
        private string[] Questionss;
        private int currentImageIndex = 0;
        int pngFileCount;
        string confi = "";
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //resultList = new List<(string, int)>();
                button12.Text = "       Next";
                currentImageIndex = 0;
                if (selectedClients == null)
                {
                    MessageBox.Show("Please Select Competitors First");
                    return;
                }
                if (selectedClients.Count == 0)
                {
                    MessageBox.Show("Please Select Competitors First");
                    return;
                }
                foreach (var clientInfo in selectedClients)
                {
                    await Task.Run(() => SendImageDataAsync("Start", "", clientInfo.Client));
                }
                foreach (var clientInfo in ObserverClients)
                {
                    await Task.Run(() => SendImageDataAsync("Start", "", clientInfo.Client));
                }
                tabControl1.SelectedIndex = 4;
                flowLayoutPanel3.Controls.Clear();
                foreach (var comp in selectedClients)
                {
                    Panel pnl = new Panel();
                    pnl.Size = new Size(200, 164);
                    pnl.BorderStyle = BorderStyle.FixedSingle;

                    Label name1 = new Label();
                    name1.AutoSize = false;
                    name1.Location = new Point(11, 5);
                    name1.Text = "Name:";
                    name1.Name = "Name1";
                    Size name1size = TextRenderer.MeasureText(name1.Text, name1.Font);
                    name1.Size = new Size(name1size.Width + 20, name1size.Height + 6);

                    Label name2 = new Label();
                    name2.AutoSize = false;
                    name2.Location = new Point(11, 25);
                    name2.Size = new Size(161, 60);
                    name2.Text = comp.Name;
                    name2.Name = "Name2";
                    name1size = TextRenderer.MeasureText(name2.Text, name2.Font);
                    name2.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    Label Points1 = new Label();
                    Points1.AutoSize = false;
                    Points1.Location = new Point(11, 72);
                    Points1.Text = "Points:";
                    Points1.Name = "Points1";
                    name1size = TextRenderer.MeasureText(Points1.Text, Points1.Font);
                    Points1.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    Label Points2 = new Label();
                    Points2.AutoSize = false;
                    Points2.Location = new Point(138, 72);
                    Points2.Text = "0";
                    Points2.Name = "Points2";
                    name1size = TextRenderer.MeasureText(Points2.Text, Points2.Font);
                    Points2.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    Label Select1 = new Label();
                    Select1.AutoSize = false;
                    Select1.Location = new Point(11, 101);
                    Select1.Text = "Select:";
                    Select1.Name = "Select1";
                    name1size = TextRenderer.MeasureText(Select1.Text, Select1.Font);
                    Select1.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    Label Select2 = new Label();
                    Select2.AutoSize = false;
                    Select2.Location = new Point(138, 101);
                    Select2.Text = "-";
                    Select2.Name = "Select2";
                    name1size = TextRenderer.MeasureText(Select2.Text, Select2.Font);
                    Select2.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    Label Submited1 = new Label();
                    Submited1.AutoSize = false;
                    Submited1.Location = new Point(11, 130);
                    Submited1.Text = "Submited:";
                    Submited1.Name = "Submited1";
                    name1size = TextRenderer.MeasureText(Submited1.Text, Submited1.Font);
                    Submited1.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    Label Submited2 = new Label();
                    Submited2.AutoSize = false;
                    Submited2.Location = new Point(131, 130);
                    Submited2.Text = "No";
                    Submited2.ForeColor = Color.Red;
                    Submited2.Name = "Submited2";
                    name1size = TextRenderer.MeasureText(Submited2.Text, Submited2.Font);
                    Submited2.Size = new Size(name1size.Width + 40, name1size.Height + 16);

                    pnl.Controls.Add(name1);
                    pnl.Controls.Add(name2);
                    pnl.Controls.Add(Points1);
                    pnl.Controls.Add(Points2);
                    pnl.Controls.Add(Select1);
                    pnl.Controls.Add(Select2);
                    pnl.Controls.Add(Submited1);
                    pnl.Controls.Add(Submited2);

                    flowLayoutPanel3.Controls.Add(pnl);
                }


                string[] type1 = System.Text.RegularExpressions.Regex.Split(confi, "<##>");
                timeeary = System.Text.RegularExpressions.Regex.Split(type1[1], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                Pointss = System.Text.RegularExpressions.Regex.Split(type1[2], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                Answerss = System.Text.RegularExpressions.Regex.Split(type1[3], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();
                Questionss = System.Text.RegularExpressions.Regex.Split(type1[5], "<#>").Where(s => !string.IsNullOrEmpty(s)).ToArray();

                pngFileCount = Questionss.Count();
                if (pngFileCount > 9)
                {
                    label27.Text = "01/" + pngFileCount.ToString();
                }
                else
                {
                    label27.Text = "1/" + pngFileCount.ToString();
                }
                currentImageIndex = 0;
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Questions", Questionss[currentImageIndex]);
                pictureBox5.Image = System.Drawing.Image.FromFile(imagePath + ".png");

                panel10.Enabled = true;
                if (type1[1].Contains("Each"))
                {
                    Timee = -1;
                    put_time(Convert.ToInt32(timeeary[2]));
                }
                label26.Text = "00:00:00";
                timer1.Start();
            }
            catch (Exception)
            {

            }
        }
        private void put_time(int sec)
        {
            // Convert seconds to TimeSpan
            TimeSpan time = TimeSpan.FromSeconds(sec);

            // Format TimeSpan as string in hh:mm:ss format
            string formattedTime = time.ToString(@"hh\:mm\:ss");

            // Update the text of the Label
            label24.Text = formattedTime;
        }


    }
}
