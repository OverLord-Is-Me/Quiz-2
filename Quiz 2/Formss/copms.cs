using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Protection.PlayReady;

namespace Quiz_2.Formss
{
    public partial class Copms : Form
    {
        private bool shouldStopBroadcasting = false;
        private Thread broadcastThread;
        public static class ControlID
        {
            public static string Server_name { get; set; }
            public static List<string> selectedClients { get; set; }
            public static int u_id { get; set; }
            public static DataTable searssh { get; set; }
            public static TcpListener tcpListener { get; set; }
            public static Thread listenerThread { get; set; }
            public static List<ClientInfo> connectedClients { get; set; }
        }
        public class ClientInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public TcpClient Client { get; set; }
        }
        public Copms()
        {
            InitializeComponent();
        }
        
        
        
        private void Copms_Load(object sender, EventArgs e)
        {
            if (IsServerRunning())
            {
                button4.Enabled = true;
                button3.Enabled = false;
                textBox1.Text = ControlID.Server_name;
                UpdateUI();
                //MessageBox.Show("Server is already running");
            }
            else
            {
                ControlID.connectedClients = new List<ClientInfo>();
                button4.Enabled = false;
                button3.Enabled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e) //Accept Selected Competitors 
        {
            this.Hide();
            var form2 = new Tchr();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
        private void button2_Click(object sender, EventArgs e) //Stop Server and Return 
        {
            if (button3.Enabled == false)
            {
                ShutdownServer();
            }
            this.Hide();
            var form2 = new Tchr();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }
        private void button3_Click(object sender, EventArgs e) //Server Start 
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter Server Name First");
                return;
            }
            ControlID.selectedClients = new List<string>();
            if (!IsServerRunning())
            {
                button4.Enabled = true;
                button3.Enabled = false;
                button3.Text = "Server Started...";
                flowLayoutPanel1.Controls.Clear();
                StartServer();
            }
            else
            {
                MessageBox.Show("Server is already running");
            }
        }
        private void button4_Click(object sender, EventArgs e) //Server shutdown 
        {
            ShutdownServer();
            MessageBox.Show("Server shutdown successfully.");
            button3.Enabled = true;
            button4.Enabled = false;
            button3.Text = "Start Server";
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox2 = (CheckBox)sender;
            checkBox2.BackColor = checkBox2.Checked ? Color.MediumSeaGreen : SystemColors.Control; // Update background color

            ControlID.selectedClients.Clear(); // Clear existing data
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                CheckBox checkBox = control as CheckBox;
                if (checkBox != null && checkBox.Checked)
                {
                    ClientInfo clientInfo = (ClientInfo)checkBox.Tag;
                    string clientData = $"{clientInfo.Id}<#>{clientInfo.Name}<#>{clientInfo.Address}";
                    ControlID.selectedClients.Add(clientData);
                }
            }
            if (ControlID.selectedClients.Count > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        private void Copms_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Set flags to stop threads
            shouldStopBroadcasting = true;

            ShutdownServer();

            if(ControlID.listenerThread != null)
            {
                // Wait for threads to finish
                ControlID.listenerThread.Join();
            }
            if (broadcastThread != null)
            {
                broadcastThread.Join();
            }
            // Close the application
            Application.Exit();
        }

        private void StartServer()
        {
            shouldStopBroadcasting = false;
            ControlID.tcpListener = new TcpListener(IPAddress.Any, 12347);

            // Start listen server presence

            ControlID.listenerThread = new Thread(new ThreadStart(ListenForClients));
            ControlID.listenerThread.Start();

            // Start broadcasting server presence
            broadcastThread = new Thread(new ThreadStart(BroadcastServerPresence));
            broadcastThread.Start();

            // Start sending server alive message
            Thread aliveMessageThread = new Thread(new ThreadStart(SendServerAliveMessage));
            aliveMessageThread.Start();

            MessageBox.Show("Server started!");
        }
        private void ListenForClients()
        {
            ControlID.tcpListener.Start();
            try
            {
                while (!shouldStopBroadcasting)
                {
                    TcpClient client = ControlID.tcpListener.AcceptTcpClient();
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
            using (UdpClient udpClient = new UdpClient())
            {
                IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, 12348);
                while (!shouldStopBroadcasting)
                {
                    string serverInfo = "";
                    try
                    {
                        if (textBox1.Text == "")
                        {
                            serverInfo = "Server: " + Dns.GetHostName();
                        }
                        else
                        {
                            ControlID.Server_name = textBox1.Text;
                            serverInfo = textBox1.Text + ": " + Dns.GetHostName(); // Use textBox1 for server name
                        }
                    }
                    catch
                    {

                    }
                    byte[] data = Encoding.UTF8.GetBytes(serverInfo);
                    udpClient.Send(data, data.Length, broadcastEndpoint);
                    Thread.Sleep(5000); // Broadcast every 5 seconds (adjust as needed)
                }
            }
        }
        private void HandleClientComm(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream clientStream = tcpClient.GetStream();
            StreamReader reader = new StreamReader(clientStream);
            StreamWriter writer = new StreamWriter(clientStream); // Add writer for sending messages
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
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        break; // Client has disconnected
                    }

                    string receivedMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                    userName = receivedMessage;

                    if (userName != "")
                    {
                        // Check if a client with the same name already exists
                        if (!ControlID.connectedClients.Any(c => c.Name == userName))
                        {
                            // Add the client to the connectedClients list
                            ControlID.connectedClients.Add(new ClientInfo { Id = clientId, Name = userName, Client = tcpClient });

                            // Update UI on the main thread
                            Invoke(new Action(() =>
                            {
                                UpdateUI();
                            }));
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
                var disconnectedClient = ControlID.connectedClients.FirstOrDefault(c => c.Client == tcpClient);
                if (disconnectedClient != null)
                {
                    ControlID.connectedClients.Remove(disconnectedClient);
                }

                // Update UI on the main thread
                Invoke(new Action(() =>
                {
                    UpdateUI();
                }));

                // Close the TCP client
                tcpClient.Close();
            }
        }
        private void UpdateUI()
        {
            // Clear existing controls in FlowLayoutPanel
            flowLayoutPanel1.Controls.Clear();

            // Add new controls for each connected client, enabling multi-selection
            foreach (var client in ControlID.connectedClients)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Text = $"{client.Name} ({client.Id})";
                checkBox.Tag = client; // Store client object for later access

                // Check if the client is in selectedClients
                if (ControlID.selectedClients.Any(selectedClient => selectedClient.Contains(client.Id)))
                {
                    checkBox.Checked = true;
                    checkBox.BackColor = checkBox.Checked ? Color.MediumSeaGreen : SystemColors.Control; // Update background color
                }

                checkBox.CheckedChanged += CheckBox_CheckedChanged; // Attach event handler


                if (ControlID.selectedClients.Count > 0)
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
                flowLayoutPanel1.Controls.Add(checkBox);
            }
        }
        private bool IsServerRunning()
        {
            try
            {
                // Try to bind to the server's address and port
                using (TcpListener testListener = new TcpListener(IPAddress.Any, 12347))
                {
                    testListener.Start();
                    testListener.Stop();
                }

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
                    if(ControlID.tcpListener != null)
                    {
                        // Stop listening for new clients
                        ControlID.tcpListener.Stop();
                    }

                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error  Stop listening for new clients: {ex.Message}");
                }
                // Disconnect existing clients gracefully
                foreach (TcpClient client in ControlID.connectedClients.Select(c => c.Client))
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
                    ControlID.connectedClients = new List<ClientInfo>();
                    ControlID.selectedClients = new List<string>();
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
                MessageBox.Show($"Error shutting down server: {ex.Message}");
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
                MessageBox.Show($"Error sending message to user: {ex.Message}");
            }
        }
        private void SendServerAliveMessage()
        {
            while (!shouldStopBroadcasting)
            {
                Thread.Sleep(10000); // Send the message every 10 seconds (adjust as needed)

                string serverAliveMessage = "SERVER_ALIVE";

                // Iterate through connected clients and send the message
                foreach (var clientInfo in ControlID.connectedClients)
                {
                    SendMessageToUser($"{serverAliveMessage}", clientInfo.Client);
                }
            }
        }
        public void SendMessageToUser(string message, byte[] data, TcpClient userTcpClient)
        {
            try
            {

                NetworkStream stream = userTcpClient.GetStream();
                if (!string.IsNullOrEmpty(message))
                {
                    // Send the message to the user
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine($"MESSAGE:{message}\n");
                    writer.Flush();
                }
                if (data != null && data.Length != 0)
                {
                    // Send the data to the user
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message to user: {ex.Message}");
            }
        }

    }
}
