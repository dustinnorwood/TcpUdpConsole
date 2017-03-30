using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpUdpConsole
{

    public class AsynchronousUdpListener
    {
        UdpClient m_Client;
        IPEndPoint m_IPEndPoint;
        private int m_Port;
        public int Port { get { return m_Port; } set { m_Port = value; } }
        const string BroadcastMessage = "HI,CJPRINTER";
        bool m_Run = true;
        Thread m_ListenThread;
        public bool BroadcastResponse = true;

        public event EventHandler ResponseReceived;

        private string m_Data;
        public string Data { get { return m_Data; } }

        public AsynchronousUdpListener(int port)
        {
            m_Port = port;
        }

        public void StartListen()
        {
            m_IPEndPoint = new IPEndPoint(IPAddress.Any, m_Port);
            if(m_Client == null)
                m_Client = new UdpClient(m_Port);
            else
            {

            }
            m_Run = true;
            m_ListenThread = new Thread(new ThreadStart(OnListening));
            m_ListenThread.Start();
        }

        protected void OnListening()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, m_Port);
            while (m_Run)
            {
                try
                {
                  //  m_Client.BeginReceive(ListenCallback, ipe);
                    byte[] data = m_Client.Receive(ref ipe);
                    m_Data = Encoding.ASCII.GetString(data, 0, data.Length);
                    if (this.ResponseReceived != null)
                        this.ResponseReceived(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        //protected void ListenCallback(IAsyncResult ia)
        //{
        //    byte[] data = m_Client.EndReceive(ia, )
        //    m_Data = Encoding.ASCII.GetString(data, 0, data.Length);
        //    if (this.ResponseReceived != null)
        //        this.ResponseReceived(this, EventArgs.Empty);
        //}

        public void Close()
        {
            try
            {
                m_Run = false;
                m_Client.Close();
                m_ListenThread.Join();
            }
            catch { };
            try
            {
                if (m_ListenThread != null)
                    m_ListenThread.Abort();
            }
            catch { }
        }

        static public IPAddress GetIPAddress()
        {
            IPAddress[] localIPs = Dns.GetHostEntry("").AddressList;
            foreach (IPAddress a in localIPs)
            {
                if (a.AddressFamily == AddressFamily.InterNetwork)
                {
                    return a;
                }
            }
            return IPAddress.None;
        }
    }

    public class AsynchronousUdpBroadcaster
    {
        private int m_Port;
        public int Port { get { return m_Port; } set { m_Port = value; } }
        const string BroadcastMessage = "HI,CJPRINTER";
        List<string> m_ResponseList;
        byte[] m_ReceiveBuffer;

        public event EventHandler BroadcastReceived;
        public event EventHandler ResponseReceived;
        public event EventHandler CommandReceived;

        // The response from the remote device.
        private string m_BroadcastData;
        public string BroadcastData { get { return m_BroadcastData; } }
        

        public AsynchronousUdpBroadcaster()
        {
            m_Port = 50000;
            m_ResponseList = new List<string>();
            m_ReceiveBuffer = new byte[1024];
        }

        public List<string> NetworkPrinterList { get { return m_ResponseList; } }

        public bool Broadcast(string message)
        {
            try
            {
                Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                IPEndPoint iep1 = new IPEndPoint(IPAddress.Broadcast, m_Port);
                sendSocket.SendTo(Encoding.ASCII.GetBytes(message), iep1);

                Thread.Sleep(500);
                sendSocket.Close();
            }
            catch { };
            return true;
        }

        static public IPAddress GetIPAddress()
        {
            IPAddress[] localIPs = Dns.GetHostEntry("").AddressList;
            foreach (IPAddress a in localIPs)
            {
                if (a.AddressFamily == AddressFamily.InterNetwork)
                {
                    return a;
                }
            }
            return IPAddress.None;
        }
    }
}
