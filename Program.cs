using System;
using System.Reflection;
using System.IO;
using IniFiles;

namespace TestApp_FileByteCount
{
    class Program
    {

        static void Main(string[] args)
        {
            string default_path, directory_path = "";
            bool old_path;
            Console.WriteLine("Запуск приложения");
            IniFile INI = new IniFile("TestApp_FileByteCount.ini");

            old_path = INI.KeyExists("Last_run", "path");

            if (old_path)
                default_path = INI.ReadINI("Last_run", "path");
            else
                default_path = Environment.CurrentDirectory;

            Console.WriteLine("Введите путь к каталогу для подсчета суммы значений байт файлов... "
                + Environment.NewLine + "Либо будет использован путь "
                + (old_path ? " из предыдущего запуска " : "по умолчанию") + ":" + default_path); ;

            while (directory_path.Length == 0)
            {
                directory_path = Console.ReadLine();
                if (directory_path.Length == 0)
                    directory_path = default_path;
                else if (!Directory.Exists(directory_path))
                {
                    Console.WriteLine("Введенный путь к каталогу не существует. Повторить ввод (введите Y)? ");
                    directory_path = "";
                    if (Console.ReadKey().Key != ConsoleKey.Y)
                        break;
                }
            }
            Console.ReadLine();


        }
    }
}
