using System;
using System.Windows.Forms;
using ClassLibraryForMyApp;
using System.IO;

namespace WinFormTestApp_FileByteCount
{
    public partial class FormMain : Form
    {
        FormMainLog tbLog;
        FileCollection fc;

        public FormMain()
        {
            InitializeComponent();
            tbLog = new FormMainLog(this);
            fc = new FileCollection(tbLog);
            tbPath.Text = fc.GetDefaultDirectory();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            tbResult.Text = "";
            if (Directory.Exists(tbPath.Text))
                fc.RunProcess(tbPath.Text);
            else
                MessageBox.Show("Выбранный каталог не существует!");
        }

        public void AddLogItemMethod(string s)
        {
            tbResult.Text += s + Environment.NewLine;
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
    }
}
