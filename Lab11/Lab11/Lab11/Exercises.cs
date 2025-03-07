using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab11
{
    internal class Exercises
    {
        static public long Tasks(int N, int K)
        {
            Task<long> numeratorTask = Task.Run(() => calculateNumerator(N, K));
            Task<long> denominatorTask = Task.Run(() => calculateDenominator(K));

            Task.WaitAll(numeratorTask, denominatorTask);

            return (long)(numeratorTask.Result / denominatorTask.Result);
        }


        static public long Delegates(int N, int K)
        {
            Func<int, int, long> op = calculateNumerator;
            Func<int, long> op2 = calculateDenominator;

            IAsyncResult numerator = op.BeginInvoke(N, K, null, null);
            IAsyncResult denominator = op2.BeginInvoke(K, null, null);

            long numeratorResult = op.EndInvoke(numerator);
            long denominatorResult = op2.EndInvoke(denominator);

            return (long)(numeratorResult / denominatorResult);
        }


        static public async Task<long> awaitMethod(int N, int K)
        {
            long[] results = await Task.WhenAll(Task<long>.Run(() => calculateNumerator(N, K)), Task<long>.Run(() => calculateDenominator(K)));

            long numeratorResult = results[0];
            long denominatorResult = results[1];

            return (long) (numeratorResult / denominatorResult);
        }

        static public long calculateDenominator(int K)
        {
            long result = 1;
            for(int i =1; i<=K; i++)
            {
                result = result * i;
            }
            return result;
        }

        static public long calculateNumerator(int N, int K)
        {
            long result = 1;
            for (int i = (N-K+1); i <= N; i++)
            {
                result = result * i;
            }
            return result;
        }


        static public void Fibonacci(int i, ProgressBar progressBar, Action<long> callback)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += ((object sender, DoWorkEventArgs args) =>
            {
                int n = (int)args.Argument;
                long f0 = 0, f1 = 1, fn = 0;
                for (int k=0; k<=n; k++)
                {
                    if (k == 0)
                    {
                        fn = 0;
                    }
                    else if (k == 1)
                    {
                        fn = 1;
                    }
                    else
                    {
                        fn = f0 + f1;
                        f0 = f1;
                        f1 = fn;
                    }
                    double progress = (double)((k + 1.0) / (n + 1.0) * 100);
                    backgroundWorker.ReportProgress((int)progress, fn);
                    Thread.Sleep(1000);
                }
                args.Result = fn;
            });
            backgroundWorker.ProgressChanged += ((object sender, ProgressChangedEventArgs args) =>
            {
                progressBar.Value = args.ProgressPercentage;

             });
            backgroundWorker.RunWorkerCompleted += ((object sender, RunWorkerCompletedEventArgs args) =>
            {
                callback((long)args.Result);
            });
            backgroundWorker.RunWorkerAsync(i);
        }

        static public void compressFiles(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles();

            Parallel.ForEach(files, file =>
            {
                string compressedFilePath = Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(file.Name) + ".gz");
                using (FileStream originalFileStream = File.Open(file.FullName, FileMode.Open))
                {
                    using (FileStream compressedFileStream = File.Create(compressedFilePath))
                    {
                        using (var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressor);
                        }
                    }
                };
            });
        }
    }
}
