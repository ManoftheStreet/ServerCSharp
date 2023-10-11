using ServerCore;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{

    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            //보낸다
            for (int i = 0; i < 5; i++)
            {
                byte[] sendBuff = Encoding.UTF8.GetBytes($"Hello World! {i} ! ");
                Send(sendBuff);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfByte)
        {
            Console.WriteLine($"Transferred bytes: {numOfByte}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //식당
            string host = Dns.GetHostName();//현재 컴퓨터의 호스트 이름을 가져온다
            IPHostEntry ipHost = Dns.GetHostEntry(host);//해당 이름 호스트의 IP정보를 가져온다
            IPAddress ipAddr = ipHost.AddressList[0];// 첫번째 ip주소를 서택
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);//현재 IP 주소와 포트번호로 엔드포인트 생성 (숫자만 가능)

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new GameSession(); });

            while (true)
            {
                
                try
                {
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