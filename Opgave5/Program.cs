using System;

namespace Opgave5
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerWorker serverWorker = new ServerWorker();
            serverWorker.Start();
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }
    }
}
