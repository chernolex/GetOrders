using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;

namespace GetOrders
{
    class FileThread
    {
        Thread thread;
        TextBox textBox;
        string shopName;
        string stringIP;
        string path;
        bool ifGettingFile;
        string mask;
        string soucreDir;
        string finalDir;

        public FileThread(TextBox _textbox, string _shopName, string _stringIP, string _path, bool _ifGettingFile, string _mask, string _sourceDir, string _finalDir)
        {
            textBox = _textbox;
            stringIP = _stringIP;
            shopName = _shopName;
            path = _path;
            ifGettingFile = _ifGettingFile;
            mask = _mask;
            if (ifGettingFile)
            {
                soucreDir = _sourceDir;
                finalDir = _finalDir;
            }
            else
            {
                soucreDir = _finalDir;
                finalDir = _sourceDir;
            }
            

            thread = new Thread(CopyFile);
            thread.Name = shopName;
            thread.Start();
        }

        void SetText(string text, bool isTextRed)
        {
            if (textBox.InvokeRequired) textBox.Invoke(new Action<string>((s) => textBox.Text = s), text);
            else textBox.Text = text;

            System.Drawing.Color color;

            if (isTextRed)
                color = System.Drawing.Color.FromArgb(253, 100, 120);
            else
                color = System.Drawing.Color.YellowGreen;

            if (textBox.InvokeRequired) textBox.Invoke(new Action<System.Drawing.Color>((col) => textBox.BackColor = col), color);
            else textBox.BackColor = color;
            
        }

        void SetText(string text)
        {
            if (textBox.InvokeRequired) textBox.Invoke(new Action<string>((s) => textBox.Text = s), text);
            else textBox.Text = text;
        }

        void CopyFile()
        {
            if (textBox.InvokeRequired) textBox.Invoke(new Action<System.Drawing.Color>((col) => textBox.BackColor = col), System.Drawing.Color.FromArgb(240, 240, 240));
            else textBox.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            bool isPingSuccessful = false;

            try
            {
                int count = 0;
                SetText("Попытка подключиться к магазину");
                while (!isPingSuccessful)
                {
                    if (count < 6)
                    {
                        count++;
                        SetText("Попытка подключения №" + count);
                        Ping ping = new Ping();
                        PingReply pr;

                        pr = ping.Send(stringIP);
                        if (pr.Status == IPStatus.Success)
                            isPingSuccessful = true;
                    }
                    else
                    {
                        SetText("Не удается подключиться к магазину", true);
                        break;
                    }
                }

                if (isPingSuccessful)
                {
                    if (ifGettingFile)
                    {
                        SetText("Магазин в сети, попытка скопировать заявку");
                    }
                    else
                    {
                        SetText("Магазин в сети, попытка отправить файл");
                    }
                    Thread.Sleep(1000);
                    DirectoryInfo sourceDirectory;
                    if (ifGettingFile)
                    {
                        sourceDirectory = new DirectoryInfo("\\\\" + stringIP + "\\" + soucreDir);
                    }
                    else
                    {
                        sourceDirectory = new DirectoryInfo(soucreDir);
                    }
                    Thread.Sleep(2000);
                    SetText("Получение списка файлов");
                    sourceDirectory.Refresh();
                    FileInfo[] fi = sourceDirectory.GetFiles(mask);
                    
                    string pathSave = "";
                    if (ifGettingFile)
                    {
                        pathSave = finalDir + "\\" + shopName;
                    }
                    else
                    {
                        pathSave = "\\\\" + stringIP + "\\" + finalDir;
                    }
                    DirectoryInfo dits = new DirectoryInfo(pathSave);
                    if (!dits.Exists)
                        dits.Create();

                    if (fi.Length == 0)
                    {
                        if (ifGettingFile)
                        {
                            SetText("Файл для получения не найден", true);
                            return;
                        }
                        else
                        {
                            SetText("Файл для копирования не найден", true);
                            return;
                        }
                    }


                    for (int i = 0; i < fi.Length; i++)
                    {
                        SetText("Копирую файл " + (i + 1) + " из " + fi.Length);
                        if (ifGettingFile)
                        {
                            string date = DateTime.Now.ToString();
                            date = date.Replace(":", "-");

                            string name = fi[i].Name.Remove(fi[i].Name.IndexOf('.'));
                            string resolution = fi[i].Name.Substring(fi[i].Name.IndexOf('.'));
                            fi[i].CopyTo(pathSave + "\\" + name + " " + date + resolution, true);
                        }
                        else
                        {
                            fi[i].CopyTo(pathSave + "\\" + fi[i].Name, true);
                        }
                    }

                    if (ifGettingFile)
                    {
                        SetText("Файл(ы) получен(ы)", false);
                    }
                    else
                    {
                        SetText("Файл(ы) отправлен(ы)", false);
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                SetText("Не получилось скопировать файл(ы)", true);
            }
            return;
        }
    }
}
