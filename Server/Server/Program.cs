

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //27018 nolu port dinleniyor...  
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 27018);
            tcpListener.Start();//port dinlemeye başlandı

            Console.WriteLine("Server Başlatıldı...");

            //Client bağlanıncaya kadar döngüde döner   
            while (true)
            {
                //Client bağlantısı eşleşirse   
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                Console.WriteLine("Client Bağlandı");

                StreamReader reader = new StreamReader(tcpClient.GetStream());

                // Dosya boyutu oku    
                string cmdFileSize = reader.ReadLine();

                // Clientten gelen dosyanın adını oku    
                string cmdFileName = reader.ReadLine();

                int length = Convert.ToInt32(cmdFileSize);
                byte[] buffer = new byte[length];
                int received = 0;
                int read = 0;
                int size = 1024;
                int remaining = 0;

                // Gelen veriyi 1024 bit ayırarak okur.  
                while (received < length)
                {
                    remaining = length - received;
                    if (remaining < size)
                    {
                        size = remaining;
                    }

                    read = tcpClient.GetStream().Read(buffer, received, size);
                    received += read;
                }

                // Clientten Gelen dosyayı clientteki adıyla , programın çalıştığı dizine kaydeder.   
                using (FileStream fStream = new FileStream(Path.GetFileName(cmdFileName), FileMode.Create))
                {
                    fStream.Write(buffer, 0, buffer.Length);
                    fStream.Flush();
                    fStream.Close();
                }

                Console.WriteLine("Dosya Kaydedildi , konumu :" + Environment.CurrentDirectory);
            }
        }
    }
}

