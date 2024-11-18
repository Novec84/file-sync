using System.Diagnostics;

namespace FileSync
{
    public class Progress
    {
        private Stopwatch Watch;
        private int Count;

        public Progress()
        {
            Watch = new Stopwatch();
            Count = 0;
        }

        public void Start(string message, int count)
        {
            Watch.Restart();
            Count = count;
            Console.Write("{0} ({1}): {2,3}%", message, Count, (Count > 0) ? 0 : 100);
        }

        public void Update(int current)
        {
            Console.Write("\x8\x8\x8\x8{0,3}%", (Count > 0) ? (int)(((current + 1) * 100.0) / Count) : 100);
        }

        public void Stop()
        {
            Watch.Stop();
            Console.WriteLine(" [DONE - tooks {0} second(s)]", Watch.ElapsedMilliseconds / 1000.0);
        }
    }
}
