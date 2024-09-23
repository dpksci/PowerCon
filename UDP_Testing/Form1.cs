using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace UDP_Testing
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            udp = new UDP_Connection(tb_hPort.Text, tb_IP.Text, tb_Port.Text);
        }
        bool bPara = false, bStart = false;
        
        private void btn_Start_Click(object sender, EventArgs e)
        {
            SendStart();
        }
        private void SendToUDP(ref UDP_Connection UDP, ref byte[] byteArray)
        {
            try
            {
                if (UDP.udpClient == null)
                {
                    throw new Exception();
                }
                UDP.udpClient.Send(byteArray, byteArray.Length, UDP.ipEndPoint);
                UDP.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), UDP.udpState);
                Array.Clear(byteArray, 0, byteArray.Length);
            }
            catch (Exception)
            {
                MessageBox.Show("Please establish a connection..");
            }
        }
        
        private void SendStart()
        {
            string s = "##START$$";
            byteArray = Encoding.ASCII.GetBytes(s);
            SendToUDP(ref udp, ref byteArray);
            string[] sA = new string[2];
            sA[0] = "Start Sent";
            sA[1] = "( " + DateTime.Now.ToString("HH:mm:ss") + " )";

            int i = sArray.Count();
            sArray.Append("Start Sent");
           
            lb_Notification.Items.Add("(" + DateTime.Now.ToString("HH:mm:ss") + ")" + "Start Sent" );
        }
        private void SendStop()
        {
            string s = "##STOP$$";
            byteArray = Encoding.ASCII.GetBytes(s);
            SendToUDP(ref udp, ref byteArray);
            string[] sA = new string[2];
            sA[0] = "Stop Sent";
            sA[1] = "( " + DateTime.Now.ToString("HH:mm:ss") + " )";

            sArray.Append("Stop Sent");

            lb_Notification.Items.Add("(" + DateTime.Now.ToString("HH:mm:ss") + ")" + "Stop Sent" );
        }
        public void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            bReceiveData = u.EndReceive(ar, ref e);

            DecodeReceivedData(ref bReceiveData);

            Array.Clear(bReceiveData, 0, bReceiveData.Length);

            udp.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), udp.udpState);
        }
        private void DecodeReceivedData(ref byte[] bArray)
        {
            string receivedString = BitConverter.ToString(bArray);
            string ss = Encoding.ASCII.GetString(bArray);
            Action action1 = delegate
            {
                if(ss.Length>=4) lb_Notification.Items.Add("(" + DateTime.Now.ToString("HH:mm:ss") + ")" + ss);
                if (ss.Contains("OK")) bPara = false;
            };
            lb_Notification.Invoke(action1);
        }
        byte[] byteArray, bReceiveData;
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            SendStop();
        }

        private string[] sArray = new string[] { } ;
        private string filePath;
        private UDP_Connection udp;
    }
}
