using System;
using System.IO;
using IniFiles;
using System.Threading;
using System.Collections.Generic;

namespace TestApp_FileByteCount
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Запуск приложения");
            FileCollection fc = new FileCollection();
            try
            {
                fc.RunProcess();
            }
            catch (Exception e)
            { 
                Console.WriteLine("Error in Process: {0}", e.Message);
            }
            Console.WriteLine("Нажмите клавишу для завершения");
            Console.Read();

        }
    }
}
