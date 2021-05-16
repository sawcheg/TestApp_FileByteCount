using System;
using System.IO;
using IniFiles;
using ClassLibraryForMyApp;

namespace TestApp_FileByteCount
{
    class Program
    {
        static void Main(string[] args)
        {

            FileCollection fc = new FileCollection(new ConsoleLog());
            Console.WriteLine("Запуск приложения");

            try
            {
                fc.RunProcess(fc.GetDefaultDirectory(true));
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
