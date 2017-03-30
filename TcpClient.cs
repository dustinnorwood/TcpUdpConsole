using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;

namespace TcpUdpConsole
{
    // State object for receiving data from remote device.
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousTcpClient
    {
        // The port number for the remote device.
        private IPAddress ip;
        private int port;

        public event EventHandler OnConnected;
        public event EventHandler DataSent;
        public event EventHandler DataReceived;

        // The response from the remote device.
        private string m_ReceivedData;
        public string ReceivedData {  get { return m_ReceivedData; } }

        private string m_SentData;
        public string SentData { get { return m_SentData; } }

        private bool m_Connected;
        public bool Connected {  get { return m_Connected; } }

        Socket client;

        public void StartClient(IPAddress i, int p)
        {
            ip = i;
            port = p;
            StartClient();
        }

        public void StartClient()
        {
            // Connect to a remote device.
            try
            {
                m_Connected = false;

                IPEndPoint remoteEP = new IPEndPoint(ip, port);

                // Create a TCP/IP socket.
                client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket cli = (Socket)ar.AsyncState;

                // Complete the connection.
                cli.EndConnect(ar);

                m_Connected = true;

                Receive(cli);
            }
            catch (Exception)
            {
                m_Connected = false;
            }


            if (this.OnConnected != null)
                this.OnConnected(this, EventArgs.Empty);
        }

        private void Receive(Socket cli)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = cli;

                // Begin receiving the data from the remote device.
                cli.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket cli = state.workSocket;

                // Read data from the remote device.

                int bytesRead = cli.EndReceive(ar);
                m_ReceivedData = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                // There might be more data, so store the data received so far.
                state.sb.Append(m_ReceivedData);

                if (this.DataReceived != null)
                    this.DataReceived(this, EventArgs.Empty);

                // Get the rest of the data.
                Receive(cli);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(string data)
        {
            if (client != null)
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                m_SentData = data;
                // Begin sending the data to the remote device.
                client.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket cli = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = cli.EndSend(ar);

                if (this.DataSent != null)
                    this.DataSent(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StopClient()
        {
            client.Disconnect(false);
            client.Shutdown(SocketShutdown.Both);

            m_Connected = false;
        }

        public AsynchronousTcpClient()
        {
            m_Connected = false;
        }

        public AsynchronousTcpClient(IPAddress i, int p):this()
        {
            ip = i;
            port = p;
        }
    }
}