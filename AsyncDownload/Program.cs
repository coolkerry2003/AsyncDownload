using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => TestParallelFor());
            Task.Run(() => TestFor());
            Console.ReadLine();
        }
        static void TestFor()
        {

            Stopwatch mTimer = new Stopwatch();
            mTimer.Start();

            Dictionary<string, string> mDic = new Dictionary<string, string>();
            int mProgress = 0;
            int iEnd = 10;
            int jEnd = 100;
            int mTotal = iEnd * jEnd;
            for (int i = 0; i < iEnd; i++)
            {
                for (int j = 0; j < jEnd; j++)
                {
                    string mRecord = string.Format("{0},{1}", i, j);

                    WebClient mAgetn = new WebClient();
                    mAgetn.DownloadStringAsync(new Uri("https://tw.yahoo.com"));
                    mAgetn.DownloadStringCompleted += (sender, e) =>
                    {
                        try
                        {
                            mProgress++;
                            Console.WriteLine("TestFor 進度{0}/{1} {2} 網頁內容總共為 {3} 個字元。", mProgress, mTotal, mRecord, e.Result.Length);
                            mDic.Add(mRecord, e.Result);
                            if (mProgress >= mTotal)
                            {
                                mTimer.Stop();
                                Console.WriteLine("TestFor Total Time (over all): {0} ms", mTimer.ElapsedMilliseconds);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        finally
                        {
                        }
                    };
                }
            }
            Console.WriteLine("等待中...");
        }
        static void TestParallelFor()
        {
            Stopwatch mTimer = new Stopwatch();
            mTimer.Start();

            Dictionary<string, string> mDic = new Dictionary<string, string>();
            int mProgress = 0;
            int iEnd = 10;
            int jEnd = 100;
            int mTotal = iEnd * jEnd;

            Parallel.For(0, iEnd, i =>
            {
                Parallel.For(0, jEnd, j =>
                {
                    string mRecord = string.Format("{0},{1}", i, j);

                    WebClient mAgetn = new WebClient();
                    mAgetn.DownloadStringAsync(new Uri("https://tw.yahoo.com"));
                    mAgetn.DownloadStringCompleted += (sender, e) =>
                    {
                        try
                        {
                            mProgress++;
                            Console.WriteLine("TestParallelFor 進度{0}/{1} {2} 網頁內容總共為 {3} 個字元。", mProgress, mTotal, mRecord, e.Result.Length);
                            mDic.Add(mRecord, e.Result);
                            if (mProgress >= mTotal)
                            {
                                mTimer.Stop();
                                Console.WriteLine("TestParallelFor Total Time (over all): {0} ms", mTimer.ElapsedMilliseconds);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        finally
                        {
                        }
                    };
                });
            });
            Console.WriteLine("等待中...");
        }
    }
}
