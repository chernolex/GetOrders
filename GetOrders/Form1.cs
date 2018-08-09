using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace GetOrders
{
    public partial class Form1 : Form
    {
        private bool _isScheduleMode;
        private System.Timers.Timer _time;
        private bool _isAutoMode;
        private readonly Dictionary<string, FileThread> _threadsMap = new Dictionary<string, FileThread>();

        public Form1()
        {
            InitializeComponent();
        }

        // ReSharper disable once UnusedMember.Local
        // Probably will be used in future.
        private void RemoveAllChecks()
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
        }

        private void StartThread(TextBox textBox, string name, string ip, bool isGettingFile, string mask,
            string remoteLocation, string localLocation)
        {
            if (_threadsMap.ContainsKey(name))
                _threadsMap[name].StartThread();
            else
                AddThread(textBox, name, ip, isGettingFile, mask, remoteLocation, localLocation);
        }

        private void AddThread(TextBox textBox, string name, string ip, bool isGettingFile, string mask, string remoteLocation, string localLocation)
        {
            _threadsMap.Add(name, new FileThread(textBox, name, ip, isGettingFile, mask, remoteLocation, localLocation));
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (_isScheduleMode)
            {
                bool allDownloaded = true;

                foreach (var thread in _threadsMap)
                {
                    if (ThreadStatuses.GetThreadStatusTypeByStatus(thread.Value.Status) == ThreadStatusType.Failed)
                    {
                        thread.Value.StartThread();
                        allDownloaded = false;
                    }
                }

                if (allDownloaded)
                {
                    _time.Enabled = false;
                    MessageBox.Show(@"Заявки скопированы");
                    MessageBox.Show(@"Все заявки скопированы. Дата/Время:" + DateTime.Now);
                }
            }

            if (_isAutoMode)
            {
                foreach (var thread in _threadsMap)
                {
                    if (ThreadStatuses.GetThreadStatusTypeByStatus(thread.Value.Status) == ThreadStatusType.Failed)
                        thread.Value.StartThread();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("path.conf");
                path_textBox.Text = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception)
            {
                path_textBox.Text = @"C:\Заявки";
            }
            if (path_textBox.Text == "")
                path_textBox.Text = @"C:\Заявки";

            radioButton1.Checked = true;
            textBox8.Text = @"*.xls";
            textBox9.Text = @"рабочий стол";

            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++ )
            {
                string arg = args[i];

                if (arg == "-schedule")
                    _isScheduleMode = true;

                if (arg == "-rmv")
                {
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    checkBox6.Checked = false;
                    checkBox7.Checked = false;
                    checkBox8.Checked = false;
                    checkBox9.Checked = false;
                    checkBox10.Checked = false;
                }

                if (arg == "-so")
                    checkBox1.Checked = true;

                if (arg == "-ig")
                    checkBox2.Checked = true;

                if (arg == "-ri")
                    checkBox3.Checked = true;

                if (arg == "-st")
                    checkBox4.Checked = true;

                if (arg == "-zi")
                    checkBox5.Checked = true;

                if (arg == "-as")
                    checkBox6.Checked = true;

                if (arg == "-ke")
                    checkBox7.Checked = true;

                if (arg == "-bo")
                    checkBox8.Checked = true;

                if (arg == "-pe")
                    checkBox9.Checked = true;

                if (arg == "-pr")
                    checkBox10.Checked = true;

                if (arg == "-send")
                {
                    radioButton2.Checked = true;
                }

                if (arg == "-mask")
                {
                    i++;
                    textBox8.Text = args[i];
                }

                if (arg == "-source")
                {
                    i++;
                    path_textBox.Text = args[i];
                }

                if (arg == "-dest")
                {
                    i++;
                    textBox9.Text = args[i];
                }

                if (arg == "-auto")
                {
                    _isAutoMode = true;
                }
            }

            if (_isScheduleMode || _isAutoMode)
            {
                StartCopyingFiles();

                _time = new System.Timers.Timer
                {
                    Interval = 10000,
                    Enabled = true
                };
                _time.Elapsed += OnTimedEvent;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog {RootFolder = Environment.SpecialFolder.DesktopDirectory};
            fbd.ShowDialog();
            path_textBox.Text = fbd.SelectedPath;
            try
            {
                StreamWriter sw = new StreamWriter("path.conf", false);
                sw.Write(path_textBox.Text);
                sw.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(path_textBox.Text);
            try
            {
                if (!di.Exists)
                    di.Create();
            }
            catch (Exception)
            {
                // ignored
            }

            StartCopyingFiles();
        }

        void StartCopyingFiles()
        {
            if (checkBox1.Checked)
                StartThread(textBox1, "Союз", "10.7.21.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox2.Checked)
                StartThread(textBox2, "Игарка", "10.7.22.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox3.Checked)
                StartThread(textBox3, "Риал", "10.7.23.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox4.Checked)
                StartThread(textBox4, "Стиль", "10.7.24.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox5.Checked)
                StartThread(textBox5, "Зимушка", "10.7.25.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox6.Checked)
                StartThread(textBox6, "Ассорти", "10.7.26.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox7.Checked)
                StartThread(textBox7, "Кедр", "10.7.27.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox8.Checked)
                StartThread(textBox10, "Белый орел", "10.7.28.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox9.Checked)
                StartThread(textBox11, "Перекресток", "10.7.30.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
            if (checkBox10.Checked)
                StartThread(textBox12, "Промышленный", "10.7.29.50", radioButton1.Checked, textBox8.Text, textBox9.Text,
                    path_textBox.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                button1.Text = @"Скачать файлы";
            }
            //else
            //{
            //    radioButton2.Checked = true;
            //    button1.Text = "Отправить файлы";
            //}
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                radioButton3.Checked = false;
                button1.Text = @"Отправить файлы";
            }
            //else
            //{
            //    radioButton1.Checked = true;
            //    button1.Text = "Скачать файлы";
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog {RootFolder = Environment.SpecialFolder.DesktopDirectory};
            fbd.ShowDialog();
            textBox9.Text = fbd.SelectedPath;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                button1.Text = @"Удалить файлы";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var thread in _threadsMap)
            {
                if (ThreadStatuses.GetThreadStatusTypeByStatus(thread.Value.Status) != ThreadStatusType.Failed)
                    thread.Value.SetThreadToStop();
            }
        }
    }
}
