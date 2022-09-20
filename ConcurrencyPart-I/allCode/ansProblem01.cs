using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        static long Sum_Global = 0;
        static int NUM_THREAD = Environment.ProcessorCount;
        // static long[] sum_Local = new long[10000];

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try 
            {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }
            return returnData;
        }
        static long sum(int threadN, int N)
        {
            int dataAmount = 1000000000;

            int indexNow = (int)((dataAmount*(threadN*1.0/N*1.0)));
            int indexCounter = (int)(dataAmount*((threadN+1)*1.0/N*1.0));
            int index;
            long result = 0;
            for(index = indexNow ; index < indexCounter ; index++)
            {
                if ((((int)Data_Global[index]) & 1) == 0) {
                    result -= Data_Global[index];
                }
                else if (Data_Global[index] % 3 == 0)
                {
                    result += (Data_Global[index]*2); // *2
                }
                else if (Data_Global[index] % 5 == 0)
                {
                    result += (Data_Global[index]/2); // /2
                }
                else if (Data_Global[index] % 7 == 0)
                {
                    result += (Data_Global[index] / 3);
                }
                Data_Global[index] = 0;
            }
            return result;  // return for Thread Join
        }

        static void workerThread(object num)
        {
            // Console.WriteLine(num);
            long Sum_Local = 0;
            Sum_Local = sum((int)num, NUM_THREAD); //#threadNumber, all threads
            Sum_Global += Sum_Local;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int y;

            Console.WriteLine("Thread Num : {0}", NUM_THREAD);
            Thread[] threadsArr = new Thread[NUM_THREAD];

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            // MultiThread Computing
            /* Start */
            Console.Write("\n\nWorking...\n");
            sw.Start();
            for (int i=0; i<NUM_THREAD; i++) {
                threadsArr[i] = new Thread(workerThread);
                threadsArr[i].Start(i);
            }
            for (int i=0; i<NUM_THREAD; i++) threadsArr[i].Join();     
            sw.Stop();
            Console.WriteLine("Done.");           
            /* STOP */

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}