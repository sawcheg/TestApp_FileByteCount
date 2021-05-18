using System;
using ClassLibraryForMyApp;
using System.Threading;

namespace TestApp_FileByteCount
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// </summary>
        static void Main(string[] args)
        {
            bool is_en_culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en";
            FileCollection fc = new FileCollection(new ConsoleLog());
            Console.WriteLine(is_en_culture? "Application launch" : "Запуск приложения");
            try
            {
                fc.RunProcess(fc.GetDefaultDirectory(true));
            }
            catch (Exception e)
            {
                Console.WriteLine(is_en_culture ? "Error in Process: {0}" : "Ошибка при выполнении: {0}", e.Message);
            }
            Console.WriteLine(is_en_culture ? "Press the key to exit..." : "Нажмите клавишу для завершения...");
            Console.Read();
        }
    }
}
