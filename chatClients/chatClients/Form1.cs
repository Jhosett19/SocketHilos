using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace chatClients
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox2.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            readData = "Conected to Chat Server ...";
            msg();
            clientSocket.Connect("127.0.0.1", 8888);
            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox3.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            Thread ctThread = new Thread(getMessage);
            ctThread.Start();
        }

        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[10025];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, 10024);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(msg));
            else
                textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + readData;
        }

        private void button3_Click(object sender, EventArgs e)
     }
}
            //FILE TRANSFER USING C#.NET SOCKET - CLIENT
class FTClientCode
        {
            public static string curMsg = "Idle";
            public static void SendFile(string fileName)
            {
                try
                {
                    IPAddress[] ipAddress = Dns.GetHostAddresses("localhost");
                    IPEndPoint ipEnd = new IPEndPoint(ipAddress[0], 5656);
                    /* Make IP end point same as Server. */
                    Socket clientSock = new Socket(AddressFamily.InterNetwork,
               SocketType.Stream, ProtocolType.IP);
                    /* Make a client socket to send data to server. */
                    string filePath = "";
                    /* File reading operation. */
                    fileName = fileName.Replace("\\", "/");
                    while (fileName.IndexOf("/") > -1)
                    {
                        filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                    }
                    byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                    if (fileNameByte.Length > 850 * 1024)
                    {
                        curMsg = "File size is more than 850kb, please try with small file.";
                        return;
                    }
                    curMsg = "Buffering ...";
                    byte[] fileData = File.ReadAllBytes(filePath + fileName);
                    /* Read & store file byte data in byte array. */
                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                    /* clientData will store complete bytes which will store file name length, 
                    file name & file data. */
                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                    /* File name length’s binary data. */
                    fileNameLen.CopyTo(clientData, 0);
                    fileNameByte.CopyTo(clientData, 4);
                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                    /* copy these bytes to a variable with format line [file name length]
                    [file name] [ file content] */
                    curMsg = "Connection to server ...";
                    clientSock.Connect(ipEnd);
                    /* Trying to connection with server. /
                    curMsg = "File sending...";
                    clientSock.Send(clientData);
                    /* Now connection established, send client data to server. */
                    curMsg = "Disconnecting...";
                    clientSock.Close();
                    /* Data send complete now close socket. */
                    curMsg = "File transferred.";
                }
                catch (Exception ex)
                {
                    if (ex.Message == "No connection could be made because the target machine 
                       actively refused it")
                         curMsg = "File Sending fail. Because server not running.";
             else
                        curMsg = "File Sending fail." + ex.Message;
                }
            }
}
