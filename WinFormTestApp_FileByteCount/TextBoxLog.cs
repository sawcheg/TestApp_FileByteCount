using System;
using ClassLibraryForMyApp;
using System.Windows.Forms;

namespace WinFormTestApp_FileByteCount
{
    class FormMainLog : ILog
    {
        FormMain form;

        public FormMainLog(FormMain formMain)
        {
            form = formMain;
        }

        public void WriteLine(string value)
        {
            form.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                form.AddLogItemMethod(value);
            });

        }
    }
}
