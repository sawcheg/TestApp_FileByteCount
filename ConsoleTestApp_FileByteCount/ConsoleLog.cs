using System;
using ClassLibraryForMyApp;

namespace TestApp_FileByteCount
{
    /// <summary>
    /// Interface implementation for Console
    /// </summary>
    public class ConsoleLog : ILog
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
