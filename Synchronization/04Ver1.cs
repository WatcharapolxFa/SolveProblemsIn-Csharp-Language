using System.Threading;
namespace OS_Sync_01
{
    class program
    {
        private static string x = "";
        private static int allowDid = 1;
        private static int exitFlag = 0;

        private static object _lock = new Object();

        static void ThReadX()
        {
            string temp = x;
            while (exitFlag == 0)
                lock (_lock)
                {
                    if (allowDid == 2) {
                        Console.WriteLine("X = {0}", x);
                        allowDid = 1;
                    }
                }
        }

        static void ThWritten()
        {
            string xx;
            while (exitFlag == 0)
            {
                lock (_lock)
                {
                    if (allowDid == 1) {
                        Console.Write("Input : ");
                        xx = Console.ReadLine();
                        if (xx == "exit") {
                            exitFlag = 1;
                            Console.WriteLine("Thread {0} exit", exitFlag);
                            allowDid = 3;
                        } else {
                            x = xx;
                            allowDid = 2;
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Thread A = new Thread(ThReadX);
            Thread B = new Thread(ThWritten);

            A.Start();
            B.Start();

            A.Join();
            B.Join();

        }
    }
}