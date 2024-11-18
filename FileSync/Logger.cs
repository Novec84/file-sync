namespace FileSync
{
    public class Logger : IDisposable
    {
        private StreamWriter? writer;
        private bool disposed;

        public Logger(string filename)
        {
            writer = string.IsNullOrEmpty(filename) ? null : new StreamWriter(filename);
            disposed = false;
        }

        ~Logger()
        {
            Dispose(false);
        }

        public void LogDirectories(string inputDirectory, string storeDirectory)
        {
            if (writer != null)
            {
                writer.WriteLine($"{inputDirectory};;{storeDirectory}");
                writer.Flush();
            }
        }

        public void LogNewFile(string newFile)
        {
            if (writer != null)
            {
                writer.WriteLine($"{newFile};N;");
                writer.Flush();
            }
        }

        public void LogDuplicityFile(string inputFile, string storeFile)
        {
            if (writer != null)
            {
                writer.WriteLine($"{inputFile};D;{storeFile}");
                writer.Flush();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (writer != null)
                    writer.Dispose();
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
