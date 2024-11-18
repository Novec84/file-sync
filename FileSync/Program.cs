namespace FileSync
{
    static public class Program
    {
        static public void Main(string[] args)
        {
            Arguments arguments = new Arguments();

            Console.WriteLine("FILE SYNC");
            Console.WriteLine("**************************************");

            if (args.Length == 0)
            {
                arguments.WriteInfo();
                return;
            }
            arguments.Initialize(args);
            arguments.Write();

            Console.WriteLine();

            using (Logger logger = new Logger(arguments.LogFilename))
            {
                logger.LogDirectories(arguments.InputDirectory, arguments.StoreDirectory);

                int inputCount = Utilities.NumberOfFiles(arguments.InputDirectory);
                Console.WriteLine($"Input count: {inputCount}");
                if (inputCount == 0)
                    return;
                int storeCount = Utilities.NumberOfFiles(arguments.StoreDirectory);
                Console.WriteLine($"Store count: {storeCount}");

                Dictionary<long, List<string>> storeCache = Utilities.FileLength2Filenames(arguments.StoreDirectory);

                Utilities.Synchronize(
                    arguments.InputDirectory,
                    arguments.StoreDirectory,
                    arguments.NewDirectory,
                    arguments.DuplicityDirectory,
                    storeCache,
                    logger);
            }
        }
    }
}