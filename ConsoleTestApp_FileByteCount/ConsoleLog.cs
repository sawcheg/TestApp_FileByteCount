using System;
using ClassLibraryForMyApp;

namespace ClassLibraryForMyApp
{
    /// <summary>
    /// Interface implementation for Console
    /// </summary>
    class ConsoleLog : ILog
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
