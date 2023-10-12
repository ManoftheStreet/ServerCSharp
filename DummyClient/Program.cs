using ServerCore;
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

            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return new ServerSession(); });

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