using System.Collections.Generic;
using System.IO;

namespace ClassLibraryForMyApp
{
    class FileRecordForThread
    {
        public string Path { get; }
        public int Number { get; }
        public int Sec_sleep { get; }
        private long countByte;
        public long CountByte { get => countByte; }

        /// <summary>
        /// Recording by file for processing
        /// </summary>
        /// <param name="path"></param>
        /// <param name="number"></param>
        /// <param name="sec_sleep"></param>
        public FileRecordForThread(string path, int number, int sec_sleep)
        {
            Path = path;
            Number = number;
            Sec_sleep = sec_sleep;
            countByte = 0;
        }

        /// <summary>
        /// Get number of Bytes in File
        /// </summary>
        public void CalcBytesInFile()
        {
            using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
              countByte = fs.Length;
        }
    }
}
