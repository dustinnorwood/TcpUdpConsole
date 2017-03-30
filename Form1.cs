using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace TcpUdpConsole
{
    public partial class Form1 : Form
    {
        private AsynchronousTcpClient m_Tcp;
        private AsynchronousUdpBroadcaster m_UdpB;
        private AsynchronousUdpListener m_UdpL, m_UdpL_Com;

        public Form1()
        {
            InitializeComponent();
            m_Tcp = new AsynchronousTcpClient();
            m_UdpB = new AsynchronousUdpBroadcaster();
            m_UdpL = new AsynchronousUdpListener(50001);
            m_UdpL_Com = new AsynchronousUdpListener(50002);

            m_Tcp.OnConnected += M_Tcp_OnConnected;
            m_Tcp.DataSent += M_Tcp_DataSent;
            m_Tcp.DataReceived += M_Tcp_DataReceived;

            m_UdpL.ResponseReceived += M_Udp_ResponseReceived;
            m_UdpL_Com.ResponseReceived += M_UdpL_Com_ResponseReceived;
        }

        private void M_UdpL_Com_ResponseReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(on_com_receive));
            else
                on_com_receive();
        }

        private void M_Udp_ResponseReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(on_b_receive));
            else
                on_b_receive();
        }

        private void on_b_receive()
        {
            this.textBoxUdpResponse.Text += m_UdpL.Data.Replace("\n","\r\n");
            this.textBoxUdpResponse.Invalidate();
        }

        private void on_com_receive()
        {
            this.textBoxUdpCommandResponse.Text += m_UdpL_Com.Data.Replace("\n", "\r\n");
            this.textBoxUdpCommandResponse.Invalidate();
        }

        private void M_Tcp_DataReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(on_receive));
            else
                on_receive();
        }

        private void on_receive()
        {
            textBoxTcpReceive.Text += m_Tcp.ReceivedData;
            textBoxTcpReceive.Invalidate();
        }

        private void M_Tcp_DataSent(object sender, EventArgs e)
        {
            // Cool story, bro
        }

        private void M_Tcp_OnConnected(object sender, EventArgs e)
        {
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(on_connect));
            else
                on_connect();
        }

        private void on_connect()
        {
            if (m_Tcp.Connected)
                button1.Text = "Disconnect";
            else
                button1.Text = "Connect";
            button1.Enabled = true;
            button1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!m_Tcp.Connected)
            {
                IPAddress ip;
                int port;
                if (!IPAddress.TryParse(textBoxIp.Text, out ip)) return;
                if (!int.TryParse(textBoxTcpPort.Text, out port)) return;
                button1.Enabled = false;
                m_Tcp.StartClient(ip, port);
            }
            else
            {
                m_Tcp.StopClient();
                button1.Text = "Connect";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_Tcp.Send(textBoxTcpSend.Text + '\n');
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // m_Udp.WaitForResponse(IPAddress.Any, textBoxUpdBroadcast.Text);
            m_UdpB.Broadcast(textBoxUpdBroadcast.Text);
            m_UdpL.StartListen();
        }
    }
}
