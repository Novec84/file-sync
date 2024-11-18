namespace FileSync
{
    public class Arguments
    {
        public string StoreDirectory { get; set; }
        public string InputDirectory { get; set; }
        public string NewDirectory { get; set; }
        public string DuplicityDirectory { get; set; }
        public string LogFilename { get; set; }

        public Arguments()
        {
        }

        public void Initialize(string[] commandArgs)
        {
            int i;
            for(i = 1; i < commandArgs.Length; i++)
                switch(commandArgs[i - 1].ToLower())
                {
                    case "-store": StoreDirectory = commandArgs[i]; break;
                    case "-input": InputDirectory = commandArgs[i]; break;
                    case "-new": NewDirectory = commandArgs[i]; break;
                    case "-duplicity": DuplicityDirectory = commandArgs[i]; break;
                    case "-log": LogFilename = commandArgs[i]; break;
                }
        }

        public void Write()
        {
            Console.WriteLine("Directories:");
            Console.WriteLine($" > store:     {StoreDirectory}");
            Console.WriteLine($" > input:     {InputDirectory}");
            Console.WriteLine($" > new:       {NewDirectory}");
            Console.WriteLine($" > duplicity: {DuplicityDirectory}");
            Console.WriteLine($"Log: {LogFilename}");
        }

        public void WriteInfo()
        {
            Console.WriteLine("Arguments:");
            Console.WriteLine($" -store directory");
            Console.WriteLine($" -input directory");
            Console.WriteLine($" -new directory");
            Console.WriteLine($" -duplicity directory");
            Console.WriteLine($" -log filename (optional)");
        }
    }
}
