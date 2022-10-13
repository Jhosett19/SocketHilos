using System;
using System.Threading; //Espacio de nombres
using System.Net.Sockets; //Espacio de nombres para Sockets
using System.Text;
using System.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        /// crea una lista de usuarios con una estructura de datos Hash
        public static Hashtable clientsList = new Hashtable(); 
        {
            /// crea un socket TCP y escucha en el puerto 8888
            TcpListener serverSocket = new TcpListener(8888);

            // crea un socket para los clientes
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            ///inicia a escuchar el servidor
            serverSocket.Start();
            Console.WriteLine("Chat Server Started ....");
            counter = 0;
            //bucle infinito
            while ((true))
            {
                counter += 1;

                //cliente es aceptado para conectar
                clientSocket = serverSocket.AcceptTcpClient();

                //buffer de mensajes
                byte[] bytesFrom = new byte[10025];
                string dataFromClient = null;

                //se crea un flujo de datos hacia el cliente
                NetworkStream networkStream = clientSocket.GetStream();
                //leemos del stream 10024 bytes y lo ponemos en el buffer
                networkStream.Read(bytesFrom, 0, 10024);

                // codificamos los bytes como codigo ASCII
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                // buscar el simbolo $ y copia hasta esa parte
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                //se agrega a la tabla Hash el usuario y el socket respectivo
                clientsList.Add(dataFromClient, clientSocket);

                // enviamos un mensaje a todos los conectados
                broadcast(dataFromClient + " Joined ", dataFromClient, false);

                // mensaje en el servidor
                Console.WriteLine(dataFromClient + " Joined chat room ");

                handleClinet client = new handleClinet();

                client.startClient(clientSocket, dataFromClient, clientsList);
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }

        public static void broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value; //socket
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(uName + " says : " + msg);
                }
                else
                {
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }  //end broadcast function
    }//end Main class

//FILE TRANSFER USING C#.NET SOCKET - SERVER
class FTServerCode
{
    IPEndPoint ipEnd; 
    Socket sock;
    public FTServerCode()
    {
        ipEnd = new IPEndPoint(IPAddress.Any, 5656); 
        //Make IP end point to accept any IP address with port no 5656.
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        //Here creating new socket object with protocol type and transfer data type
        sock.Bind(ipEnd); 
        //Bind end point with newly created socket.
    }
    public static string receivedPath;
    public static string curMsg = "Stopped";
    public void StartServer()
    {
        try
        {
            curMsg = "Starting...";
            sock.Listen(100);
            /* That socket object can handle maximum 100 client connection at a time & 
            waiting for new client connection /
            curMsg = "Running and waiting to receive file.";
            Socket clientSock = sock.Accept();
            /* When request comes from client that accept it and return 
            new socket object for handle that client. */
            byte[] clientData = new byte[1024 * 5000];
            int receivedBytesLen = clientSock.Receive(clientData);
            curMsg = "Receiving data...";    
            int fileNameLen = BitConverter.ToInt32(clientData, 0); 
            /* I've sent byte array data from client in that format like 
            [file name length in byte][file name] [file data], so need to know 
            first how long the file name is. /
            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            /* Read file name */
            BinaryWriter bWrite = new BinaryWriter(File.Open
		(receivedPath +"/"+ fileName, FileMode.Append)); ; 
            /* Make a Binary stream writer to saving the receiving data from client. /
            bWrite.Write(clientData, 4 + fileNameLen, 
		receivedBytesLen - 4 - fileNameLen);
            /* Read remain data (which is file content) and 
            save it by using binary writer. */
            curMsg = "Saving file...";
            bWrite.Close();
            clientSock.Close(); 
            /* Close binary writer and client socket */
            curMsg = "Received & Saved file; Server Stopped.";
        }
        catch (Exception ex)
        {
            curMsg = "File Receiving error.";
        }
    }
}





    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;

            //aqui inicia los hilos o subprocesos
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, 10024);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);

                    Program.broadcast(dataFromClient, clNo, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }   
            }//end while
        }//end doChat
    } //end class handleClinet
}//end namespace
