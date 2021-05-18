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
        List<FileRecordForThread> file_list;
        static ILog Log;
        string xml_file;
        DateTime dtStart;
        static int m_remain;
        static EventWaitHandle m_event;
        static bool is_en_culture;

        /// <summary>
        /// Constructor of a class for processing files
        /// </summary>
        /// <param name="log">Interface implementation in main programm</param>
        public FileCollection(ILog log)
        {
            file_list = new List<FileRecordForThread>();
            INI = new IniFile(Environment.CurrentDirectory + "TestApp.ini");
            Log = log;
            is_en_culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en";
        }

        /// <summary>
        /// Getting the default directory
        /// </summary>
        /// <param name="is_console">Console application attribute (additional handler in the loop)</param>
        /// <returns></returns>
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
                if (is_en_culture)
                {
                    Console.WriteLine("Enter the path to the directory to calculate the sum of the file byte values... " + Environment.NewLine 
                      + (old_path ? "Or the path will be used from previous run " : "Or the default path will be used") + ":" + default_path);
                }
                else
                {
                    Console.WriteLine("Введите путь к каталогу для подсчета суммы значений байт файлов... "
                        + Environment.NewLine + "Либо будет использован путь "
                        + (old_path ? "из предыдущего запуска " : "по умолчанию") + ":" + default_path);
                }

                while (directory_path.Length == 0)
                {
                    directory_path = Console.ReadLine();
                    if (directory_path.Length == 0)
                        directory_path = default_path;
                    else if (!Directory.Exists(directory_path))
                    {
                        Console.WriteLine(is_en_culture ? "The directory path you entered does not exist.Re - enter(enter Y)"  
                                                        : "Введенный путь к каталогу не существует. Повторить ввод (введите Y)? ");
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

        /// <summary>
        /// Starting the calculation process
        /// </summary>
        /// <param name="directory_path">Selected Directory</param>
        public void RunProcess(string directory_path)
        {
            dtStart = DateTime.Now;
            file_list.Clear();
            xml_file = directory_path + @"\Result.xml";
            ClearOldResultXML();
            if (directory_path.Length > 0)
            {
                Log.WriteLine(is_en_culture ? "The file list is being prepared...":"Идет подготовка списка файлов...");
                ProcessDirectory(directory_path);
                Log.WriteLine((is_en_culture ? "Number of files in the selected directory = " : "Количество файлов в выбранном каталоге = ") + file_list.Count.ToString());

                m_remain = file_list.Count;
                m_event = new ManualResetEvent(false);
                file_list.ForEach(frec => ThreadPool.QueueUserWorkItem(ProcessFile, frec));
                //ожидание завершения обработки последней записи
                m_event.WaitOne();
            }
            SaveResultToXML();
            Log.WriteLine((is_en_culture ? "Execution time, sec: " : "Время выполнения, с: ") + (DateTime.Now - dtStart).TotalSeconds);
            INI.Write("Last_run", "path", directory_path);
        }

        /// <summary>
        /// Process all files in the directory passed in, recurse on any directories
        /// that are found, and process the files they contain
        /// </summary>
        /// <param name="targetDirectory">Selected Directory</param>
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

        /// <summary>
        /// Logic for processing found files here
        /// </summary>
        /// <param name="file_record">full path to the file</param>
        static void ProcessFile(object file_record)
        {
            FileRecordForThread rec = (FileRecordForThread)file_record;
            try
            {
                // imitation of long work (for debug)  
#if DEBUG
                Thread.Sleep(rec.Sec_sleep);
#endif                               
                rec.CalcBytesInFile();
                Log.WriteLine(string.Format((is_en_culture ? "Thread {0}: {1} bytes in file '{2}'." :
                    "Поток {0}: {1} байтов в файле '{2}'."), Thread.CurrentThread.ManagedThreadId, rec.CountByte, rec.Path));
            }
            catch (Exception e)
            {
                Log.WriteLine(string.Format((is_en_culture ? "Error in Thread {0}: {1}." 
                    : "Ошибка в потоке {0}: {1}."), rec.Number.ToString(), e.Message));
            }
            if (Interlocked.Decrement(ref m_remain) == 0)
                m_event.Set();
        }

        /// <summary>
        /// Deleting the old result if it exists
        /// </summary>
        private void ClearOldResultXML()
        {
            if (File.Exists(xml_file))
                File.Delete(xml_file);
        }

        /// <summary>
        /// Saving the result to XML
        /// </summary>
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
            Log.WriteLine((is_en_culture ? "Results file saved to file: " : "Файл результатов сохранен в файл: ") + xml_file);
        }
    }
}
