using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Shapes;
using Windows.Media.Protection.PlayReady;
using Windows.UI.Composition;
using static Quiz_2.Formss.copm_connect;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using Path = System.IO.Path;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;


namespace Quiz_2.Formss
{
    public partial class copm_connect : Form
    {
        public List<ServerInfo> discoveredServers = new List<ServerInfo>();
        public TcpClient userTcpClient;
        public Thread receiveThread;
        public bool isConnected = false;
        public bool exiit = false;
        // Add a flag to check if the server is still available
        public bool isServerAvailable = true;
        public bool shouldcontinue = true;
        public class ServerInfo
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
        public static class ControlID
        {
            public static string Comp_Names { get; set; }
            public static string selectedClients_Names { get; set; }
            public static string confi { get; set; }
            public static string connected_Server_Names { get; set; }
            public static string connected_Server_Address { get; set; }
            public static string messag { get; set; }
            public static string observ_messag { get; set; }
            public static string login_type { get; set; }
        }
        public copm_connect()
        {
            InitializeComponent();
        }

        private void copm_connect_Load(object sender, EventArgs e)
        {
            ControlID.messag = "";
            ControlID.observ_messag = "";
            if (ControlID.login_type == "Observer")
            {
                label4.Text = "Choose to Observe";
                txt_name.Text = "Observer";
                txt_name.Enabled = false;
            }
        }
        private void copm_connect_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                DisconnectFromServer();
                // Wait for threads to finish
                if (receiveThread != null)
                {
                    receiveThread.Join();
                }

                // Close the application
                Application.Exit();
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error in FormClosing: {ex.Message}");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DiscoverServers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in button4: {ex.Message}");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (copm_connect.ControlID.confi != "")
            {
                SendMessageToAdmin("Ready:" + ControlID.Comp_Names, ControlID.connected_Server_Address);
            }
            return;
            exiit = true;
            if (DisconnectFromServer())
            {
                button3.Enabled = false;
                MessageBox.Show("Disconnected from the server.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (button2.Text == "Exit")
                {
                    try
                    {
                        DisconnectFromServer();
                        // Wait for threads to finish
                        if (receiveThread != null)
                        {
                            receiveThread.Join();
                        }
                        // Close the application
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Error in FormClosing: {ex.Message}");
                    }
                }
                else if (copm_connect.ControlID.confi != "")
                {
                    button2.Enabled = false;
                    SendMessageToAdmin("Ready:" + ControlID.Comp_Names, ControlID.connected_Server_Address);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in button2: {ex.Message}");
            }
        }
        private async void BtnServer_Click(object sender, EventArgs e)
        {
            if (txt_name.Text == "")
            {
                MessageBox.Show("Please Write the team Name First"); return;
            }
            // Handle server connection logic when a server button is clicked
            if (sender is Button btn)
            {
                try
                {
                    shouldcontinue = false;
                    ServerInfo serverInfo = btn.Tag as ServerInfo;
                    if (serverInfo != null)
                    {
                        ControlID.connected_Server_Names = serverInfo.Name;
                        ControlID.connected_Server_Address = serverInfo.Address;
                        // Show "Please wait, Connecting now..." message box
                        using (var waitMessageBox = new Form() { TopMost = true })
                        {
                            waitMessageBox.Size = new Size(200, 80);
                            waitMessageBox.FormBorderStyle = FormBorderStyle.FixedDialog;
                            waitMessageBox.Text = "Connecting...";
                            waitMessageBox.ControlBox = false;

                            // Center the waitMessageBox on the copm_connect form
                            waitMessageBox.StartPosition = FormStartPosition.Manual;
                            waitMessageBox.Location = new Point(this.Left + (this.Width - waitMessageBox.Width) / 2, this.Top + (this.Height - waitMessageBox.Height) / 2);

                            var label = new Label() { Text = "Please wait, Connecting now...", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                            waitMessageBox.Controls.Add(label);

                            // Show the message box
                            waitMessageBox.Show();

                            // Start the asynchronous operation to connect to the server
                            var connectTask = ConnectToServerAsync(serverInfo);

                            // Wait for the connection to complete
                            await connectTask;

                            // Dismiss the message box
                            waitMessageBox.Close();
                        }

                        if (userTcpClient != null)
                        {
                            receiveThread = new Thread(new ThreadStart(async () => await ReceiveAllServerMessagesAsync()));
                            receiveThread.Start();
                            btn.BackColor = Color.MediumSeaGreen;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in Connecting to the Server : {ex.Message}");
                }
            }
        }
        private async Task ConnectToServerAsync(ServerInfo serverInfo)
        {
            try
            {
                // Ping the server to check if it's alive
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(serverInfo.Address);
                    if (reply.Status == IPStatus.Success)
                    {
                        // Server is alive, proceed to connect
                        TcpClient tcpClient = new TcpClient(serverInfo.Address, 12347);

                        isConnected = true;
                        isServerAvailable = true;
                        button3.Enabled = true;

                        string message = txt_name.Text;
                        ControlID.Comp_Names = txt_name.Text;
                        SendMessageToAdmin(message, serverInfo.Address);
                        MessageBox.Show($"Connected to {serverInfo.Name}");
                    }
                    else
                    {
                        // Server is not reachable
                        MessageBox.Show($"Server {serverInfo.Name} is not reachable.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Server is not reachable
                MessageBox.Show($"Server {serverInfo.Name} is not reachable.");
                // Loop through the buttons in flowLayoutPanel1 and find the button with matching text
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    if (control is Button button && button.Text == serverInfo.Name)
                    {
                        // Remove the button from flowLayoutPanel1
                        flowLayoutPanel1.Controls.Remove(button);
                        break; // Exit the loop after removing the button
                    }
                }
            }
            #region MyRegion
            //try
            //{
            //    // Your existing ConnectToServer method logic
            //    TcpClient tcpClient = new TcpClient(serverInfo.Address, 12347);

            //    isConnected = true;
            //    isServerAvailable = true;
            //    button3.Enabled = true;

            //    string message = txt_name.Text;
            //    ControlID.Comp_Names = txt_name.Text;
            //    SendMessageToAdmin(message, serverInfo.Address);
            //    MessageBox.Show($"Connected to {serverInfo.Name}");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Form Error connecting to {serverInfo.Name}: {ex.Message}");
            //} 
            #endregion
        }

        private async void DiscoverServers()
        {
            #region MyRegion
            shouldcontinue = true;
            button4.Enabled = false;
            try
            {
                discoveredServers = new List<ServerInfo>();
                ControlID.connected_Server_Names = "";
                ControlID.connected_Server_Address = "";

                await Task.Run(() =>
                {
                    UdpClient udpClient = new UdpClient(12348);
                    IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Any, 0);

                    udpClient.Client.ReceiveTimeout = 10000;

                    while (shouldcontinue)
                    {
                        try
                        {
                            byte[] data = udpClient.Receive(ref serverEndpoint);
                            //MessageBox.Show(data.Length.ToString());
                            string serverInfo = Encoding.UTF8.GetString(data);
                            if (serverInfo != "")
                            {
                                //MessageBox.Show(serverInfo);
                                AddDiscoveredServer(serverInfo);
                                Thread.Sleep(5000);
                            }
                        }
                        catch (SocketException ex)
                        {
                            if (ex.SocketErrorCode == SocketError.TimedOut)
                            {
                                //MessageBox.Show($"TimedOut");
                            }
                            else if (ex.SocketErrorCode != SocketError.TimedOut)
                            {
                                MessageBox.Show($"Error in mini Discovering Servers: {ex.Message}");
                                break;
                            }
                        }
                    }
                    udpClient.Close();
                    // Enable button4 on the UI thread
                    this.Invoke((MethodInvoker)delegate
                    {
                        button4.Enabled = true;
                    });
                });
            }
            catch (Exception ex)
            {
                shouldcontinue = false;
                button4.Enabled = true;
                DisconnectFromServer();
                MessageBox.Show($"Disconnect From Server in discovering server: {ex.Message}");
            }
            #endregion
        }
        private void AddDiscoveredServer(string serverInfo)
        {
            try
            {
                string[] parts = serverInfo.Split(':');
                if (parts.Length == 2)
                {
                    string serverName = parts[0].Trim();
                    string serverAddress = parts[1].Trim();

                    // Check if the server with the same address already exists
                    if (discoveredServers.Any(s => s.Address == serverAddress))
                    {
                        //MessageBox.Show("Server already exists, skip adding it again");
                        if (flowLayoutPanel1.Controls.Count > 0)
                        {
                            return; // If there are already controls in the flowLayoutPanel, return early
                        }
                    }

                    ServerInfo info = new ServerInfo { Name = serverName, Address = serverAddress };

                    // Update UI on the main thread
                    Invoke(new Action(() =>
                    {
                        AddServerControl(info);
                    }));

                    // Add to the list for later use when connecting
                    discoveredServers.Add(info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Add Discovered Servers: {ex.Message}");
            }
        }
        private void AddServerControl(ServerInfo serverInfo)
        {
            try
            {
                Button btnServer = new Button();
                btnServer.AutoSize = false;
                btnServer.Text = serverInfo.Name;
                btnServer.Font = new System.Drawing.Font("Sakkal Majalla", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnServer.Tag = serverInfo; // Store ServerInfo object as Tag
                btnServer.Click += BtnServer_Click;
                Size btnServer1 = TextRenderer.MeasureText(btnServer.Text, btnServer.Font);
                btnServer.Size = new Size(btnServer1.Width + 20, btnServer1.Height + 6);
                flowLayoutPanel1.Controls.Add(btnServer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Add Server Control: {ex.Message}");
            }
        }
        private bool DisconnectFromServer()
        {
            try
            {
                // Set flags to stop threads
                isConnected = true;
                isServerAvailable = false;
                exiit = true;
                shouldcontinue = false;


                ControlID.connected_Server_Names = "";
                ControlID.connected_Server_Address = "";
                discoveredServers.Clear();
                if ((userTcpClient != null && userTcpClient.Connected) || isServerAvailable == false)
                {
                    isConnected = false;
                    if (userTcpClient != null)
                    {
                        userTcpClient.GetStream().Close(); // Close the stream first
                        userTcpClient.Close();
                    }
                }

                // Wait for the threads to finish gracefully
                Thread.Sleep(3000);

                if (!IsConnected())
                {
                    try
                    {
                        ClearFlowLayoutPanelControls();
                    }
                    catch
                    {

                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    ClearFlowLayoutPanelControls();
                    return true;
                }
                catch
                {
                    MessageBox.Show($"Error disconnecting from the server: {ex.Message}");
                    return false;
                }
            }
        }
        private void ClearFlowLayoutPanelControls()
        {
            try
            {
                if (flowLayoutPanel1.InvokeRequired)
                {
                    // If we're on a different thread than the UI thread, invoke the operation on the UI thread
                    flowLayoutPanel1.Invoke(new MethodInvoker(ClearFlowLayoutPanelControls));
                }
                else
                {
                    // Clear the controls on the UI thread
                    flowLayoutPanel1.Controls.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Clear Flow Layout Panel Controls: {ex.Message}");
            }
        }
        private bool IsConnected()
        {
            try
            {
                if (userTcpClient != null)
                {
                    // Check the connection state
                    return userTcpClient.Client.Connected;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public void SendMessageToAdmin(string message, string Address)
        {
            try
            {
                // Example code in copm_connect form
                userTcpClient = new TcpClient();
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
        private async Task ReceiveAllServerMessagesAsync()
        {
            try
            {
                int cnt = 0;
                NetworkStream stream = userTcpClient.GetStream();
                StreamReader reader = new StreamReader(stream);
                string Corrupted_images = "";

                while (isConnected)
                {
                    try
                    {
                        // Attempt to read server alive message
                        string message = await reader.ReadLineAsync();
                        if (label4.Text == "Results Room")
                        {
                            // Enable button4 on the UI thread
                            this.Invoke((MethodInvoker)delegate
                            {
                                button2.Enabled = true;
                                button2.Text = "Exit";
                            });

                            continue;
                        }
                        if (message != null)
                        {
                            if (message.Trim() == "MESSAGE:SERVER_ALIVE")
                            {
                                cnt = 0;
                            }
                            else if (message.Contains("IMAGE"))
                            {
                                cnt = 0;
                                // Split the header and data using the specified separator
                                // 
                                if (message.Contains("Results"))
                                {
                                    string[] parts3 = message.Split(new[] { "<##>" }, StringSplitOptions.None);
                                    Invoke(new Action(() =>
                                    {
                                        button2.Enabled = true;
                                        button2.Visible = true;
                                        button2.BringToFront();
                                        label4.Text = "Results Room";
                                        txt_name.Visible = false;
                                        label1.Visible = false;
                                        button4.Visible = false;
                                        button3.Visible = false;
                                        flowLayoutPanel1.Controls.Clear();
                                        if (parts3.Length > 0)
                                        {
                                            int count = 1;
                                            foreach (string part2 in parts3)
                                            {
                                                string[] parts = part2.Split(new[] { "<#>" }, StringSplitOptions.None);
                                                if (!parts.Contains("Observer"))
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
                                                    flowLayoutPanel1.Controls.Add(newPanel);
                                                }
                                            }
                                        }
                                    }));
                                }
                                else if (message.Contains("Start"))
                                {
                                    try
                                    {
                                        if (ControlID.login_type != "Observer")
                                        {
                                            if (InvokeRequired)
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    this.Hide();

                                                    // Create a list to store forms to close
                                                    List<Form> formsToClose = new List<Form>();

                                                    // Find and add forms of type quz_tek to the list
                                                    foreach (Form form in Application.OpenForms)
                                                    {
                                                        if (form is quz_tek)
                                                        {
                                                            formsToClose.Add(form);
                                                        }
                                                    }

                                                    // Close forms outside the foreach loop
                                                    foreach (Form form in formsToClose)
                                                    {
                                                        form.Close();
                                                    }

                                                    var form3 = new quz_tek();
                                                    form3.Closed += (s, args) => this.Show(); // Show the current form when form3 is closed
                                                    form3.Show();
                                                }));
                                            }
                                            else
                                            {
                                                this.Hide();

                                                // Create a list to store forms to close
                                                List<Form> formsToClose = new List<Form>();

                                                // Find and add forms of type quz_tek to the list
                                                foreach (Form form in Application.OpenForms)
                                                {
                                                    if (form is quz_tek)
                                                    {
                                                        formsToClose.Add(form);
                                                    }
                                                }

                                                // Close forms outside the foreach loop
                                                foreach (Form form in formsToClose)
                                                {
                                                    form.Close();
                                                }

                                                var form3 = new quz_tek();
                                                form3.Closed += (s, args) => this.Show(); // Show the current form when form3 is closed
                                                form3.Show();
                                            }
                                        }
                                        else
                                        {

                                            if (InvokeRequired)
                                            {
                                                Invoke(new Action(() =>
                                                {
                                                    this.Hide();

                                                    // Create a list to store forms to close
                                                    List<Form> formsToClose = new List<Form>();

                                                    // Find and add forms of type observv to the list
                                                    foreach (Form form in Application.OpenForms)
                                                    {
                                                        if (form is observv)
                                                        {
                                                            formsToClose.Add(form); this.Hide();
                                                        }
                                                    }

                                                    // Close forms outside the foreach loop
                                                    foreach (Form form in formsToClose)
                                                    {
                                                        form.Close(); this.Hide();
                                                    }

                                                    var form3 = new observv();
                                                    form3.Closed += (s, args) => this.Show(); // Show the current form when form3 is closed
                                                    form3.Show();
                                                }));
                                            }
                                            else
                                            {
                                                this.Hide();

                                                // Create a list to store forms to close
                                                List<Form> formsToClose = new List<Form>();

                                                // Find and add forms of type observv to the list
                                                foreach (Form form in Application.OpenForms)
                                                {
                                                    if (form is observv)
                                                    {
                                                        formsToClose.Add(form); this.Hide();
                                                    }
                                                }

                                                // Close forms outside the foreach loop
                                                foreach (Form form in formsToClose)
                                                {
                                                    form.Close(); this.Hide();
                                                }

                                                var form3 = new observv();
                                                form3.Closed += (s, args) => this.Show(); // Show the current form when form3 is closed
                                                form3.Show();
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                else if (message.Contains("Finished"))
                                {
                                    try
                                    {
                                        if (Corrupted_images != "")
                                        {
                                            //SendMessageToAdmin("Corrupted:" + Corrupted_images + ControlID.Comp_Names, ControlID.connected_Server_Address);
                                        }
                                        string imageName = message.Substring("IMAGE:".Length).Trim();
                                        ControlID.confi = imageName;
                                        if (InvokeRequired)
                                        {
                                            Invoke(new Action(() =>
                                            {
                                                timer1.Start();
                                                button2.Enabled = false;
                                                button2.Visible = true;
                                                button2.BringToFront();
                                                label4.Text = "Competitors Room";
                                                txt_name.Visible = false;
                                                label1.Visible = false;
                                                button4.Visible = false;
                                                button3.Visible = false;
                                                if (ControlID.login_type == "Observer")
                                                {
                                                    button2.Enabled = false;
                                                }
                                                else
                                                {
                                                    button2.Enabled = false;
                                                }
                                                if (ControlID.selectedClients_Names != "")
                                                {
                                                    string[] parts = copm_connect.ControlID.selectedClients_Names.Split(new[] { "<#>" }, StringSplitOptions.None);
                                                    flowLayoutPanel1.Controls.Clear();
                                                    foreach (string part in parts)
                                                    {
                                                        if (part != "Observer")
                                                        {
                                                            Label lbl = new Label();
                                                            lbl.AutoSize = false;
                                                            lbl.Size = new Size(814, 74);
                                                            lbl.TextAlign = ContentAlignment.MiddleCenter;
                                                            lbl.Text = part;
                                                            lbl.Font = new System.Drawing.Font("Sakkal Majalla", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                                            flowLayoutPanel1.Controls.Add(lbl);
                                                        }

                                                    }
                                                }

                                            }));
                                        }
                                        else
                                        {

                                            button2.Enabled = false;
                                            button2.Visible = true;
                                            button2.BringToFront();
                                            label4.Text = "Competitors Room";
                                            txt_name.Visible = false;
                                            label1.Visible = false;
                                            button4.Visible = false;
                                            button3.Visible = false;

                                            timer1.Start();

                                            if (ControlID.selectedClients_Names != "")
                                            {
                                                string[] parts = copm_connect.ControlID.selectedClients_Names.Split(new[] { "<#>" }, StringSplitOptions.None);
                                                flowLayoutPanel1.Controls.Clear();
                                                foreach (string part in parts)
                                                {
                                                    Label lbl = new Label();
                                                    lbl.AutoSize = false;
                                                    lbl.Size = new Size(814, 74);
                                                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                                                    lbl.Text = part;
                                                    lbl.Font = new System.Drawing.Font("Sakkal Majalla", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                                    flowLayoutPanel1.Controls.Add(lbl);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error in Finished: {ex.Message}");
                                    }
                                }
                                else if (message.Contains("Names:"))
                                {
                                    try
                                    {
                                        string resultString = message.Replace("IMAGE:Names:", "");
                                        ControlID.selectedClients_Names = resultString;
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error in unique: {ex.Message}");
                                    }
                                }
                                else if (message.Contains("IMAGEDATA:"))
                                {
                                    try
                                    {
                                        #region MyRegion
                                        ////string imageName = "";
                                        ////// Get the directory path
                                        ////string directoryPath = Path.GetDirectoryName(Path.Combine("Temp", imageName));
                                        ////// Get the directory path
                                        ////string directoryPath2 = Path.GetDirectoryName(directoryPath);
                                        ////// If the directory doesn't exist, create it
                                        ////if (!Directory.Exists(directoryPath2))
                                        ////{
                                        ////    Directory.CreateDirectory(directoryPath2);
                                        ////}
                                        ////// Read the length of the incoming image data
                                        ////byte[] lengthBuffer = new byte[4];
                                        ////stream.Read(lengthBuffer, 0, 4);
                                        ////int imageLength = BitConverter.ToInt32(lengthBuffer, 0);

                                        ////// Read the actual image data
                                        ////byte[] imageData = new byte[imageLength];
                                        ////stream.Read(imageData, 0, imageLength);
                                        ////File.WriteAllBytes(directoryPath2, imageData);


                                        ////continue; 
                                        //
                                        //try
                                        //{
                                        //    #region MyRegion
                                        //    //using (var output = File.Create(directoryPath2))
                                        //    //{
                                        //    //    // read the file divided by 1KB
                                        //    //    var buffer = new byte[1024];
                                        //    //    int bytesRead;
                                        //    //    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                        //    //    {
                                        //    //        output.Write(buffer, 0, bytesRead);
                                        //    //    }
                                        //    //}

                                        //    //// This is an image imageName
                                        //    //imageName = parts[0].Substring("IMAGE:".Length).Trim();

                                        //    //// This is an image imageData
                                        //    //imageData = Convert.FromBase64String(parts[1]);
                                        //    //SavePictureToFile(Path.Combine("Temp", imageName), imageData); 
                                        //    #endregion
                                        //}
                                        //catch
                                        //{
                                        //    //Corrupted_images = Corrupted_images + imageName + "<#>";
                                        //}
                                        #endregion
                                        string[] parts = message.Split(new[] { "<#>" }, StringSplitOptions.None);
                                        string destinationFolder = Path.Combine(Application.StartupPath, "Temp");

                                        if (!Directory.Exists(destinationFolder))
                                        {
                                            Directory.CreateDirectory(destinationFolder);
                                        }
                                        string destinationPath = Path.Combine(destinationFolder, parts[1]);
                                        string lastFourCharacterss2 = parts[2].Substring(parts[2].Length - 50);

                                        string ress2 = parts[2].Replace("\n", "");
                                        ress2 = ress2.Replace("\r", "");

                                        ress2 = ress2.Trim();
                                        ress2 = ress2.Replace("MESSAGE:SERVER_ALIVE", "");
                                        string result = string.Join("\n", ress2.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)));
                                        string lastFourCharacters2 = result.Substring(result.Length - 50);

                                        try
                                        {
                                            if (result.Length % 2 != 0)
                                            {
                                                //MessageBox.Show("dd");
                                                result = result + "0";
                                                //result = result.Substring(0, result.Length - 1);
                                            }
                                            // Convert the hexadecimal string to a byte array
                                            byte[] imageData = Enumerable.Range(0, result.Length)
                                                .Where(x => x % 2 == 0)
                                                .Select(x => Convert.ToByte(result.Substring(x, 2), 16))
                                                .ToArray();

                                            File.WriteAllBytes(destinationPath, imageData);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show($"Error in hex (IMAGE:): {ex.Message}");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error in Contains (IMAGE:): {ex.Message}");
                                    }
                                }
                            }
                            else if (message.Contains("Submit"))
                            {
                                ControlID.observ_messag = message;
                            }
                            else if (message == "")
                            {
                                cnt = 0;
                            }
                            else if (message == "MESSAGE:Skip" || message == "MESSAGE:Next" || message == "MESSAGE:Exit")
                            {
                                cnt = 0;
                                string messageType = message.Substring("MESSAGE:".Length);
                                ControlID.messag = messageType;
                            }
                        }
                        else
                        {
                            cnt++;
                        }

                        if (cnt > 20)
                        {
                            MessageBox.Show("Didn't Recieve any data from Server For 20 Times,Disconnecting From Server...");
                            // Server alive message not received, Disconnect and close functions
                            isServerAvailable = false; isConnected = false;
                            DisconnectFromServer();
                            break; // Exit the loop to terminate the thread
                        }
                    }
                    catch (IOException)
                    {
                        //MessageBox.Show("Error reading from the stream,Disconnecting From Server...");
                        //Error reading from the stream, disconnect and close functions

                        DisconnectFromServer();
                    }
                    finally
                    {
                        // Notify the user or perform any other UI-related operations
                        if (isServerAvailable == false && exiit == false)
                        {
                            DisconnectFromServer();
                            MessageBox.Show("Server is not Available");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ReceiveAllServerMessagesAsync: {ex.Message}");
            }
        }
        private async Task<byte[]> ReceiveImageDataAsync(NetworkStream stream)
        {
            // Receive image data length
            byte[] lengthBytes = new byte[4];
            await stream.ReadAsync(lengthBytes, 0, lengthBytes.Length);
            int imageDataLength = BitConverter.ToInt32(lengthBytes, 0);

            // Receive image data
            byte[] imageData = new byte[imageDataLength];
            int bytesRead = 0;
            while (bytesRead < imageDataLength)
            {
                bytesRead += await stream.ReadAsync(imageData, bytesRead, imageDataLength - bytesRead);
            }
            return imageData;
        }
        private static void SavePictureToFile(string filePath, byte[] imageData)
        {
            try
            {
                // Get the directory path
                string directoryPath = Path.GetDirectoryName(filePath);

                // If the directory doesn't exist, create it
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Set the directory attributes to hidden and system
                // File.SetAttributes(filePath, FileAttributes.Hidden | FileAttributes.System);

                // Delete the existing file if it exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Write the bytes to the file
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(imageData, 0, imageData.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in SavePictureToFile : {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            shouldcontinue = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            if (ControlID.login_type == "Observer")
            {
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }
    }
}

   