namespace FileSync
{
    static public class Utilities
    {
        static string filePattern = "*";
        static int bufferSize = 4096;
        static byte[] buffer1 = new byte[bufferSize];
        static byte[] buffer2 = new byte[bufferSize];

        static public int NumberOfFiles(string directory)
        {
            return Directory.GetFiles(directory, filePattern).Length;
        }

        static public Dictionary<long, List<string>> FileLength2Filenames(string directory)
        {
            Progress progress = new Progress();
            Dictionary<long, List<string>> result = new Dictionary<long, List<string>>();
            string[] files = Directory.GetFiles(directory, filePattern);
            progress.Start("Creating store cache", files.Length);
            int i;
            for(i = 0; i < files.Length; i++)
            {
                FileInfo fi = new FileInfo(files[i]);
                if (!result.ContainsKey(fi.Length))
                    result[fi.Length] = new List<string>();
                result[fi.Length].Add(Path.GetFileName(files[i]));
                progress.Update(i);
            }
            progress.Stop();
            return result;
        }

        static public void Synchronize(
            string inputDirectory,
            string storeDirectory,
            string newDirectory,
            string duplicityDirectory,
            Dictionary<long, List<string>> storeCache,
            Logger logger)
        {
            Progress progress = new Progress();
            string[] inputFiles = Directory.GetFiles(inputDirectory, filePattern);
            progress.Start("Synchronizing", inputFiles.Length);
            int i;
            for(i = 0; i < inputFiles.Length; i++)
            {
                FileInfo fi = new FileInfo(inputFiles[i]);
                if (!storeCache.ContainsKey(fi.Length))
                    CreateFileInNew(inputFiles[i], newDirectory, logger);
                else
                {
                    bool found = false;
                    List<string> storedFiles = storeCache[fi.Length];
                    int j;
                    for (j = 0; j < storedFiles.Count; j++)
                    {
                        string storedFile = Path.Combine(storeDirectory, storedFiles[j]);
                        if (FilesAreEqual(inputFiles[i], storedFile))
                        {
                            CreateFilesInDuplicity(inputFiles[i], storedFile, duplicityDirectory, logger);
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                        CreateFileInNew(inputFiles[i],newDirectory,logger);
                }
                progress.Update(i);
            }
            progress.Stop();
        }

        static private bool FilesAreEqual(string filename1, string filename2)
        {
            using (FileStream stream1 = new FileStream(filename1, FileMode.Open))
                using (FileStream stream2 = new FileStream(filename2, FileMode.Open))
                    for(; ; )
                    {
                        int count1 = stream1.Read(buffer1, 0, bufferSize);
                        int count2 = stream2.Read(buffer2, 0, bufferSize);

                        if (count1 != count2)
                            return false;
                        if (count1 == 0)
                            return true;

                        //memcmp possible
                        if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                            return false;
                    }
        }

        static private void CreateFileInNew(string inputFilename, string newDirectory, Logger logger)
        {
            string newFilename = Path.Combine(newDirectory, Path.GetFileName(inputFilename));
            if (File.Exists(newFilename))
                File.Delete(newFilename);
            File.Copy(inputFilename, newFilename);
            
            logger.LogNewFile(Path.GetFileName(inputFilename));
        }

        static private void CreateFilesInDuplicity(string inputFilename, string storeFilename, string duplicityDirectory, Logger logger)
        {
            string fullFilename;
            string inputDuplicity = Path.GetFileName(inputFilename);
            string storeDuplicity = Path.GetFileName(storeFilename);

            fullFilename = Path.Combine(duplicityDirectory, inputDuplicity);
            if (File.Exists(fullFilename))
                File.Delete(fullFilename);
            File.Copy(inputFilename, fullFilename);

            fullFilename = Path.Combine(duplicityDirectory, Path.GetFileNameWithoutExtension(inputDuplicity) + "(" + storeDuplicity + ")" + Path.GetExtension(inputDuplicity));
            if (File.Exists(fullFilename))
                File.Delete(fullFilename);
            File.Copy(storeFilename, fullFilename);

            logger.LogDuplicityFile(inputDuplicity, storeDuplicity);
        }
    }
}
