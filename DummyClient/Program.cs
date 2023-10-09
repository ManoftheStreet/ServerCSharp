using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //식당
            string host = Dns.GetHostName();//현재 컴퓨터의 호스트 이름을 가져온다
            IPHostEntry ipHost = Dns.GetHostEntry(host);//해당 이름 호스트의 IP정보를 가져온다
            IPAddress ipAddr = ipHost.AddressList[0];// 첫번째 ip주소를 서택
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);//현재 IP 주소와 포트번호로 엔드포인트 생성 (숫자만 가능)

            while (true)
            {
                //휴대폰 설정
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    //문지기에게 입장 문의
                    socket.Connect(endPoint);
                    Console.WriteLine($"Connected To{socket.RemoteEndPoint.ToString()}");

                    //보낸다
                    for(int i = 0; i < 5; i++)
                    {
                        byte[] sendBuff = Encoding.UTF8.GetBytes($"Hello World! {i} ! ");
                        int sendBytes = socket.Send(sendBuff);
                    }
                    

                    //받는다
                    byte[] recvBuff = new byte[1024];
                    int reevBytes = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, reevBytes);
                    Console.WriteLine($"[from Server] {recvData}");

                    //나간다
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }
        }
    }
}