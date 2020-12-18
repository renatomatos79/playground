using System;

namespace MFC
{
    class Program
    {
        static void Main(string[] args)
        {
            var userName = Environment.GetEnvironmentVariable("USER_NAME");
            Console.WriteLine($"Hello {userName}!");
            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }
    }
}
