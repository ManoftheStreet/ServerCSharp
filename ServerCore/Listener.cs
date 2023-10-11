using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket; // 서버에서 클라이언트의 연결 요청을 대기하는 소켓
        Func<Session> _sessionFactory;


        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            //문지기 교육
            _listenSocket.Bind(endPoint);

            //영업시작
            // 클라이언트의 연결 요청을 대기합니다. 여기서 10은 동시에 대기할 수 있는 최대 연결 요청 수를 의미합니다.
            _listenSocket.Listen(10);

            // 비동기 연결 요청을 위한 이벤트 인수를 생성하고 설정합니다.
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            // 연결 요청이 수락되었을 때 호출될 콜백 함수를 등록합니다.
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            // 비동기 연결 요청을 시작합니다.
            RegisterAccept(args);
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            // 클라이언트의 연결 요청을 비동기로 대기합니다.
            bool pending = _listenSocket.AcceptAsync(args);

            // 연결 요청이 즉시 완료되었다면 콜백 함수를 직접 호출합니다.
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            }
            else
                Console.WriteLine(args.SocketError.ToString());
            
            RegisterAccept(args);
        }
    }
}
