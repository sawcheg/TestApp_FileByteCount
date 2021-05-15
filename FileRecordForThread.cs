namespace TestApp_FileByteCount
{
    class FileRecordForThread
    {
        public string Path { get; }
        public int Number { get; }
        public int Sec_sleep { get; }
        public long CountByte { get; set; }

        public FileRecordForThread(string path, int number, int sec_sleep)
        {
            Path = path;
            Number = number;
            Sec_sleep = sec_sleep;
        }      
    }
}
