 using System;
 using System.Diagnostics;

 namespace PacketTimer{
    internal static class PacketTimerClass{
        internal static void StartTimer(ref Stopwatch stopwatch)
        {
            //update the timer refrence to be a new timer object miliseconds
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
 
        internal static void EndTimer(ref Stopwatch stopwatch)
        {
            if (stopwatch != null)
            {
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                stopwatch = null;
            }
        }
    }
 }
 