using System;
using ClassLibraryForMyApp;

namespace WinFormTestApp_FileByteCount
{
    /// <summary>
    /// Interface implementation for Console
    /// </summary>
    class FormMainLog : ILog
    {
        FormMain form;

        public FormMainLog(FormMain formMain)
        {
            form = formMain;
        }

        public void WriteLine(string value)
        {
            //running on the mainForm thread
            form.Invoke(form.myDelegate, new Object[] { value });
        }
    }
}
