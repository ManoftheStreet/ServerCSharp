using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        public void Acquird()
        {
            while(true)
            {
                int orginal = Interlocked.Exchange(ref _locked, 1);
                if (orginal == 0)
                {
                    break;
                }
            }
            
        }

        public void Release()
        {
            _locked = 0;
        }

    }

    class Program
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread1()
        {
            for(int i = 0; i < 100000; i++)
            {
                _lock.Acquird();
                _num++;
                _lock.Release();
            }
        }

        static void Thread2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquird();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread1);
            Task t2 = new Task(Thread2);
            t1.Start();
            t2.Start();

            Task.WaitAll(t1,t2);
            Console.WriteLine(_num);
        }

    }
}