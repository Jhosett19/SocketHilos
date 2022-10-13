using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace FileTransferClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter a full file path");
                string fileName = Console.ReadLine();

                TcpClient tcpClient = new TcpClient("127.0.0.1", 1234);
                Console.WriteLine("Connected. Sending file.");

                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());

                byte[] bytes = File.ReadAllBytes(fileName);

                sWriter.WriteLine(bytes.Length.ToString());
                sWriter.Flush();

                sWriter.WriteLine(fileName);
                sWriter.Flush();

                Console.WriteLine("Sending file");
                tcpClient.Client.SendFile(fileName);

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.Read();
        }
    }
}