using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var myCancellationTokenSource = new CancellationTokenSource();
            LongRunningCancellableOperation(1000, myCancellationTokenSource.Token);
            Task.Run(() =>
            {
                // cancel "LongRunningCancellableOperation" task after running for 3s
                Thread.Sleep(3000);
                myCancellationTokenSource.Cancel();
            });
            Console.ReadKey();
        }

        private static Task<decimal> LongRunningCancellableOperation(int loop, CancellationToken cancellationToken)
        {
            Task<decimal> task = null;

            // Start a task and return it
            task = Task.Run(() =>
            {
                decimal result = 0;

                try
                {
                    // Loop for a defined number of iterations
                    for (int i = 0; i < loop; i++)
                    {
                        Console.Write(i + "-");

                        // Check if a cancellation is requested, if yes,
                        // throw a TaskCanceledException.
                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw new TaskCanceledException(task);
                        }

                        // Do something that takes times like a Thread.Sleep in .NET Core 2.
                        Thread.Sleep(50);
                        result += i;
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Cancelled");
                }

                return result;
            });

            return task;
        }
    }
}
