using System;
using ClassLibraryForMyApp;

namespace ClassLibraryForMyApp
{
    class ConsoleLog : ILog
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
