using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace TimerTest
{
	class Program
	{
		static void PollingTest()
		{
			const int granularity = 10;
			while (true)
			{
				long iterations = 0;
				Stopwatch totalWatch = new Stopwatch();
				totalWatch.Start();
				Stopwatch singleIterationWatch = new Stopwatch();
				singleIterationWatch.Start();
				long maximumDelay = 0;
				while (totalWatch.ElapsedMilliseconds < 1000)
				{
					maximumDelay = Math.Max(maximumDelay, singleIterationWatch.ElapsedMilliseconds);
					singleIterationWatch.Reset();
					singleIterationWatch.Start();
					iterations++;
					Thread.Sleep(granularity);
					// Thread.Yield();
				}
				Console.WriteLine("Iterations/second: {0}, maximum delay: {1} ms", iterations, maximumDelay);
			}
		}

		static void TimerAccuracyTest()
		{
			while (true)
			{
				ManualResetEvent resetEvent = new ManualResetEvent(false);
				Stopwatch watch = new Stopwatch();
				var timer = new System.Timers.Timer(1000);
				timer.Elapsed += (object source, ElapsedEventArgs arguments) => OnAccuracyTimer(timer, watch, resetEvent);
				watch.Start();
				timer.Enabled = true;
				resetEvent.WaitOne();
			}
		}

		static void OnAccuracyTimer(System.Timers.Timer timer, Stopwatch watch, ManualResetEvent resetEvent)
		{
			Console.WriteLine("Elapsed time: {0}", watch.ElapsedMilliseconds);
			watch.Stop();
			timer.Enabled = false;
			resetEvent.Set();
		}

		static void TimerLoadTest()
		{
			for(int i = 0; i < 100; i++)
			{
				Stopwatch watch = new Stopwatch();
				var timer = new System.Timers.Timer(1000);
				timer.Elapsed += (object source, ElapsedEventArgs arguments) => OnLoadTimer(watch);
				watch.Start();
				timer.Enabled = true;
			}
			ManualResetEvent termination = new ManualResetEvent(false);
			termination.WaitOne();
		}

		static void OnLoadTimer(Stopwatch watch)
		{
			watch.Stop();
		}

		static void Main(string[] arguments)
		{
			PollingTest();
			// TimerAccuracyTest();
			// TimerLoadTest();
		}
	}
}
