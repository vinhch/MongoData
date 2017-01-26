using System;
using Microsoft.Extensions.DependencyInjection;

namespace MongoData.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppStartup.Run();
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            //run test here
            var testDb = AppStartup.ServiceProvider.GetService<TestDb>();
            testDb.Run();

            Console.ReadLine();
        }
    }
}
