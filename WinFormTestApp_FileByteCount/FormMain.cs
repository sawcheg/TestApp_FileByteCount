using System;
using System.Windows.Forms;
using ClassLibraryForMyApp;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace WinFormTestApp_FileByteCount
{
    public partial class FormMain : Form
    {
        FormMainLog tbLog;
        FileCollection fc;
        Thread myThread;
        public delegate void AddListItem(string myString);
        public AddListItem myDelegate;

        public FormMain()
        {
            InitializeComponent();
            tbLog = new FormMainLog(this);
            fc = new FileCollection(tbLog);
            tbPath.Text = fc.GetDefaultDirectory();
            myDelegate = new AddListItem(AddListItemMethod);            
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (Directory.Exists(tbPath.Text))
            {
                myThread = new Thread(ProcStart);
                myThread.IsBackground = true;
                myThread.Start(tbPath.Text);
                SetEnabled(false);
                timer1.Start();
            }
            else
                MessageBox.Show("Выбранный каталог не существует!");
        }

        private void ProcStart(object path)
        {
            fc.RunProcess((string)path);
        }

        public void AddListItemMethod(string s)
        {
            listBox1.Items.Add(s);
        }

        private void btnSetDirectory_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = tbPath.Text;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbPath.Text = fbd.SelectedPath;                  
                }
            }
        }

        private void SetEnabled(bool val)
        {
            tbPath.Enabled = val;
            btnRun.Enabled = val;
            btnSetDirectory.Enabled = val;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!myThread.IsAlive)
            {
                SetEnabled(true);
                timer1.Stop();
            }
        }
    }
}
