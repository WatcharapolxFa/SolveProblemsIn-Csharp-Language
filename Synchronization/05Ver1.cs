using System;
using System.Threading;

namespace cv_lab
{
    class Program
    {
        private static string x = "";
        private static int allowDid = 1;
        private static int exitFlag = 0;
        private static int updateFlag = 0;
        private static object _lock = new Object();

        static void ThReadX(Object i)
        {
            while (exitFlag == 0)
            {
                lock (_lock)
                {
                    if (x != "exit")
                    {
                        if (allowDid == 2)
                        {
                            Console.WriteLine("***Thread {0} : x = {1}***", i, x);
                            allowDid = 1;
                        }
                    }
                }
            }
            Console.WriteLine("---Thread {0} exit---", i);
        }

        static void ThWriteX()
        {
            string xx;
            while (exitFlag == 0)
            {
                lock (_lock)
                {
                    if (allowDid == 1)
                    {
                        Console.Write("Input : ");
                        xx = Console.ReadLine();
                        if (xx == "exit")
                            exitFlag = 1;
                        x = xx;
                        allowDid = 2;
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Thread A = new Thread(ThWriteX);
            Thread B = new Thread(ThReadX);
            Thread C = new Thread(ThReadX);
            Thread D = new Thread(ThReadX);

            A.Start();
            B.Start(1);
            C.Start(2);
            D.Start(3);

        }
    }
}