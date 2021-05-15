using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using IniFiles;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace TestApp_FileByteCount
{
    class FileCollection
    {
        List<FileRecordForThread> file_list;
        string xml_file = Environment.CurrentDirectory + "\\Result.xml";
        static int count_completed;

        public FileCollection()
        {
            file_list = new List<FileRecordForThread>();
        }

        public void RunProcess()
        {
            string default_path, directory_path = "";
            bool old_path;

            ClearOldResultXML();

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
                else
                    INI.Write("Last_run", "path", directory_path);
            }

            count_completed = 0;

            if (directory_path.Length > 0)
                ProcessDirectory(directory_path);

            // waiting for the process to complete
            while (file_list.Count > count_completed)
                Thread.Sleep(100);
            SaveResultToXML();
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        private void ProcessDirectory(string targetDirectory)
        {
            Random waitTime = new Random();
            int seconds;
            FileRecordForThread frec;
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                seconds = waitTime.Next(1 * 1000, 11 * 1000);
                Thread t = new Thread(ProcessFile);
                frec = new FileRecordForThread(fileName, file_list.Count + 1, seconds);
                file_list.Add(frec);
                t.Start(frec);
            }
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Logic for processing found files here.
        static void ProcessFile(object file_record)
        {
            FileRecordForThread rec = (FileRecordForThread)file_record;
            try
            {
                // imitation of long work (for test)
                // Thread.Sleep(rec.Sec_sleep);                
                rec.CalcBytesInFile();
                Console.WriteLine("Thread {0}: {1} bytes in file '{2}'.", rec.Number.ToString(), rec.CountByte.ToString(), rec.Path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Thread {0}: {1}.", rec.Number.ToString(), e.Message);
            }
            count_completed++;
        }       

        private void ClearOldResultXML()
        {
            if (File.Exists(xml_file))
                File.Delete(xml_file);
        }

        private void SaveResultToXML()
        {
            XElement fileElement;
            XAttribute fileAttr;
            XElement byteElement;
            XDocument xdoc = new XDocument();
            XElement resultElement = new XElement("Result");

            foreach (FileRecordForThread fr in file_list)
            {
                fileElement = new XElement("file");
                fileAttr = new XAttribute("path", fr.Path);
                byteElement = new XElement("ByteCount", fr.CountByte.ToString());
                fileElement.Add(fileAttr);
                fileElement.Add(byteElement);
                resultElement.Add(fileElement);
            }
            xdoc.Add(resultElement);
            xdoc.Save(xml_file);
            Console.WriteLine();
            Console.WriteLine("Файл результатов сохранен в файл: " + xml_file);
        }
    }
}
