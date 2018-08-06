using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GetOrders
{
    public partial class Form1 : Form
    {
        bool isScheduleMode = false;
        System.Timers.Timer time;
        bool isAutoMode = false;

        public Form1()
        {
            InitializeComponent();
        }

        void RemoveAllChecks()
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (isScheduleMode)
            {
                bool allDownloaded = true;
                if (textBox1.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox1, "Союз", "10.7.21.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox1.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (textBox2.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox2, "Игарка", "10.7.22.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox2.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (textBox3.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox3, "Риал", "10.7.23.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox3.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (textBox4.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox4, "Стиль", "10.7.24.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox4.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (textBox5.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox5, "Зимушка", "10.7.25.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox5.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (textBox6.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox6, "Ассорти", "10.7.26.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox6.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (textBox7.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    new FileThread(textBox7, "Кедр", "10.7.27.50", path_textBox.Text, true, ".xls", "рабочий стол", path_textBox.Text);
                    allDownloaded = false;
                }
                if (textBox7.BackColor == System.Drawing.Color.FromArgb(240, 240, 240))
                    allDownloaded = false;

                if (allDownloaded)
                {
                    time.Enabled = false;
                    MessageBox.Show("Заявки скопированы");
                    MessageBox.Show("Все заявки скопированы. Дата/Время:" + DateTime.Now.ToString());
                }
            }

            if (isAutoMode)
            {
                RemoveAllChecks();

                if (textBox1.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox1.Checked = true;
                }

                if (textBox2.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox2.Checked = true;
                }

                if (textBox3.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox3.Checked = true;
                }

                if (textBox4.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox4.Checked = true;
                }

                if (textBox5.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox5.Checked = true;
                }

                if (textBox6.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox6.Checked = true;
                }

                if (textBox7.BackColor == System.Drawing.Color.FromArgb(253, 100, 120))
                {
                    checkBox7.Checked = true;
                }

                StartCopyingFiles();
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
            catch (System.Exception ex)
            {
                path_textBox.Text = "C:\\Заявки";
            }
            if (path_textBox.Text == "")
                path_textBox.Text = "C:\\Заявки";

            radioButton1.Checked = true;
            textBox8.Text = "*.xls";
            textBox9.Text = "рабочий стол";

            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++ )
            {
                string arg = args[i];

                if (arg == "-schedule")
                    isScheduleMode = true;

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
                    isAutoMode = true;
                }
            }

            if (isScheduleMode || isAutoMode)
            {
                StartCopyingFiles();
                
                time = new System.Timers.Timer();
                time.Interval = 10000;
                time.Enabled = true;
                time.Elapsed += OnTimedEvent;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            fbd.ShowDialog();
            path_textBox.Text = fbd.SelectedPath;
            try
            {
                StreamWriter sw = new StreamWriter("path.conf", false);
                sw.Write(path_textBox.Text);
                sw.Close();
            }
            catch (System.Exception ex)
            {
            	
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
            catch (System.Exception ex)
            {

            }
            StartCopyingFiles();
        }

        void StartCopyingFiles()
        {
            if (checkBox1.Checked)
                new FileThread(textBox1, "Союз", "10.7.21.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox2.Checked)
                new FileThread(textBox2, "Игарка", "10.7.22.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox3.Checked)
                new FileThread(textBox3, "Риал", "10.7.23.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox4.Checked)
                new FileThread(textBox4, "Стиль", "10.7.24.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox5.Checked)
                new FileThread(textBox5, "Зимушка", "10.7.25.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox6.Checked)
                new FileThread(textBox6, "Ассорти", "10.7.26.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox7.Checked)
                new FileThread(textBox7, "Кедр", "10.7.27.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox8.Checked)
                new FileThread(textBox10, "Белый орел", "10.7.28.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
            if (checkBox9.Checked)
                new FileThread(textBox11, "Перекресток", "10.7.30.50", path_textBox.Text, radioButton1.Checked, textBox8.Text, textBox9.Text, path_textBox.Text);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                button1.Text = "Скачать файлы";
            }
            //else
            //{
            //    radioButton2.Checked = true;
            //    button1.Text = "Отправить файлы";
            //}
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                radioButton1.Checked = false;
                radioButton3.Checked = false;
                button1.Text = "Отправить файлы";
            }
            //else
            //{
            //    radioButton1.Checked = true;
            //    button1.Text = "Скачать файлы";
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            fbd.ShowDialog();
            textBox9.Text = fbd.SelectedPath;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                button1.Text = "Удалить файлы";
            }
        }
    }
}
