using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;

namespace AsteriskClient
{
    class AsteriskClient
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Подключение к серверу Asterisk
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.20"), 5038);
            clientSocket.Connect(serverEndPoint);

            // Отправка сообщения
            clientSocket.Send(Encoding.ASCII.GetBytes("Action: Login\r\nUsername: mark\r\nSecret: mysecret\r\nActionID: 1\r\n\r\n"));

            int bytesRead = 0;

            //Чтение байтов от сервера Астериск
            do
            {
                byte[] buffer = new byte[1024];
                bytesRead = clientSocket.Receive(buffer);

             
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(response);

                if (Regex.Match(response, "Message: Authentication accepted", RegexOptions.IgnoreCase).Success)
                {
                    // Отправка ping-запроса на сервер Asterisk
                    clientSocket.Send(Encoding.ASCII.GetBytes("Action: Ping\r\nActionID: 2\r\n\r\n"));
                }
            } while (bytesRead != 0);

            Console.WriteLine("Connection to server lost.");
            Console.ReadLine();
        }
    }
}