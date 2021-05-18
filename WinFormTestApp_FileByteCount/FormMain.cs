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
        bool is_en_culture;
        // to continuously display messages on the main thread
        public delegate void AddListItem(string myString);
        public AddListItem myDelegate;

        public FormMain()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language))
            {
                Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);                
            }
            is_en_culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "en";
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
                MessageBox.Show(is_en_culture? "The selected directory does not exist!" : "Выбранный каталог не существует!");
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!myThread.IsAlive)
            {
                SetEnabled(true);
                timer1.Stop();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {                      
            if (MessageBox.Show(MyStrings.ChangeLocalizacion, MyStrings.ChangeLocalizacionCaption, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Language = comboBox1.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new System.Globalization.CultureInfo[]{
                 System.Globalization.CultureInfo.GetCultureInfo("ru-RU"),
                 System.Globalization.CultureInfo.GetCultureInfo("en-US")
            };
            comboBox1.DisplayMember = "NativeName";
            comboBox1.ValueMember = "Name";
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Language))
            {
                comboBox1.SelectedValue = Properties.Settings.Default.Language;
            }
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }
    }
}
