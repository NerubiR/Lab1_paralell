using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Lab1_Paralell_Prog
{
    class Program
    {
        static void Main(string[] args)
        {
            var SS1 = new List<Double>();
            var SS2 = new List<Double>();
            var SS3 = new List<Double>();
            var file1 = @"C:\Users\Влад\source\repos\Lab1_Paralell_Prog\Lab1_Paralell_Prog\number_1.txt";
            var file2 = @"C:\Users\Влад\source\repos\Lab1_Paralell_Prog\Lab1_Paralell_Prog\number_2.txt";
            var file3 = @"C:\Users\Влад\source\repos\Lab1_Paralell_Prog\Lab1_Paralell_Prog\number_3.txt";


            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < 2; i++)
            {
                
                stopwatch.Start();
                var sum1 = makeSum(SS1, file1);
                sum1 += makeSum(SS2, file2);
                sum1 += makeSum(SS3, file3);
                stopwatch.Stop();
                var time1 = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"1 Thread: Result: {sum1}, Time: {time1}");
                stopwatch.Reset();
            }

            for (int i = 0; i < 2; i++)
            {
                stopwatch.Start();
                int tm = 1; //Время паузы
                var result2 = makeSumWithThreads(SS1, SS2, SS3, tm);
                stopwatch.Stop();
                var time2 = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                Console.WriteLine($"Multi Tread Result: {result2}, Time: {time2}");
            }
           

            double makeSum(List<double> array, string path)
            {
                return makeSumWithRange(array, path);
            }

            double makeSumWithThreads(List<double> array1, List<double> array2, List<double> array3, int tm=0)
            {
                double sum1 = 0.0;
                double sum2 = 0.0;
                double sum3 = 0.0;

                var thread1 = new Thread(() => { sum1 = makeSumWithRange(array1, file1, tm); });
                var thread2 = new Thread(() => { sum2 = makeSumWithRange(array2, file2, tm); });
                var thread3 = new Thread(() => { sum3 = makeSumWithRange(array3, file3, tm); });

                thread1.Start();
                thread2.Start();
                thread3.Start();

                thread1.Join();
                thread2.Join();
                thread3.Join();
                //Console.ReadLine();

                return sum1 + sum2 + sum3;
            }

            double makeSumWithRange(List<double> array, string path, int tm = 1)
            {
                Thread.Sleep(tm);
                try
                {
                    using (StreamReader input = new StreamReader(path))
                        while (!input.EndOfStream)
                            array.Add(double.Parse(input.ReadLine()));
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("Файл отсутсвует");
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine("Директория отсутствует");
                }

                decimal sum = 0.0m;
                for (var i = 0; i < array.Count; i++)
                    sum = (decimal)sum + (decimal)array[i];
                array.Clear();
                return (double)sum;
            }
        }
    }
}
