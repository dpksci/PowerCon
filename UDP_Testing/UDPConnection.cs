using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace UDP_Testing
{
    internal class UDP_Connection
    {
        public UDP_Connection()
        {

        }
        public UDP_Connection(string hPort, string remoteIP, string rPort)
        {
            //hPort = "5003"; // as per scicom software.
            int Host_port = int.Parse(hPort);
            string Remote_IP = remoteIP;
            int Remote_port = int.Parse(rPort);

            udpClient = new UdpClient(Host_port);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(Remote_IP), Remote_port);

            udpState = new UdpState(ipEndPoint, udpClient);
            //client.BeginReceive(new AsyncCallback(ReceiveCallback), s);
            //SendData(ref bTx_buf);
        }
        public void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            Array.Clear(bAReceivedData, 0, bAReceivedData.Length);

            bAReceivedData = u.EndReceive(ar, ref e);

            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), udpState);
        }
        public void Send_Data(ref byte[] bTx_buf)
        {
            try
            {
                if (udpClient == null)
                {
                    throw new Exception();
                }
                udpClient.Send(bTx_buf, bTx_buf.Length, ipEndPoint);
                udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), udpState);

            }
            catch (Exception)
            {
                MessageBox.Show("Please establish a connection..");
            }
        }


        public byte[] bAReceivedData;
        public UdpState udpState;
        public UdpClient udpClient;
        public IPEndPoint ipEndPoint;
    }

    

    public class UdpState
    {
        public UdpState(IPEndPoint e, UdpClient u)
        {
            this.e = e;
            this.u = u;
        }
        public IPEndPoint e;
        public UdpClient u;
    }
}
