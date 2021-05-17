using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using IniFiles;

namespace ClassLibraryForMyApp
{
    public class FileCollection
    {
        IniFile INI;
        static List<FileRecordForThread> file_list;
        static ILog Log;
        static string xml_file;

        public FileCollection(ILog log)
        {
            file_list = new List<FileRecordForThread>();
            INI = new IniFile(Environment.CurrentDirectory + "TestApp.ini");
            Log = log;
        }

        public string GetDefaultDirectory(bool is_console = false)
        {
            string default_path, directory_path = "";
            bool old_path;

            old_path = INI.KeyExists("path", "Last_run");

            if (old_path)
                default_path = INI.ReadINI("Last_run", "path");
            else
                default_path = Environment.CurrentDirectory;

            if (is_console)
            {
                Console.WriteLine("Введите путь к каталогу для подсчета суммы значений байт файлов... "
                    + Environment.NewLine + "Либо будет использован путь "
                    + (old_path ? "из предыдущего запуска " : "по умолчанию") + ":" + default_path); ;

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
            }
            else 
                directory_path = default_path;

            return directory_path;
        }

        public void RunProcess(string directory_path)
        {
            file_list.Clear();
            xml_file = directory_path + "\\Result.xml";
            ClearOldResultXML();
            if (directory_path.Length > 0)
            {
                Log.WriteLine("Идет подготовка списка файлов...");
                ProcessDirectory(directory_path);
                Log.WriteLine("Количество файлов в выбранном каталоге = " + file_list.Count.ToString());

                foreach (FileRecordForThread frec in file_list)
                {
                    Thread t = new Thread(ProcessFile);
                    t.IsBackground = true;
                    t.Start(frec);
                    t.Join();
                }              
            }
            SaveResultToXML();
            INI.Write("Last_run", "path", directory_path);
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        private void ProcessDirectory(string targetDirectory)
        {
            Random waitTime = new Random();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                file_list.Add(new FileRecordForThread(fileName, file_list.Count + 1, waitTime.Next(1 * 1000, 11 * 1000)));

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
                Log.WriteLine(string.Format("Thread {0}: {1} bytes in file '{2}'.", rec.Number.ToString(), rec.CountByte.ToString(), rec.Path));
            }
            catch (Exception e)
            {
                Log.WriteLine(string.Format("Error in Thread {0}: {1}.", rec.Number.ToString(), e.Message));
            }
            
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
            Log.WriteLine();
            Log.WriteLine("Файл результатов сохранен в файл: " + xml_file);
        }
    }
}
