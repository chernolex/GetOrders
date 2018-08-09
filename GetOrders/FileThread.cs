using System;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;
using System.Drawing;

namespace GetOrders
{
    class FileThread
    {
        private readonly string _shopName;
        private readonly string _stringIP;
        private readonly string _mask;
        private readonly string _soucreDir;
        private readonly string _finalDir;
        private readonly bool _ifGettingFile;
        private readonly TextBox _textBox;
        public ThreadStatus Status { get; set; }

        private string GetMessageByStatus(ThreadStatus status, string additionalInfo)
        {
            return ThreadStatuses.GetMessageByStatus(status, additionalInfo);
        }

        public FileThread(TextBox textbox, string shopName, string stringIP, bool ifGettingFile, string mask, 
            string sourceDir, string finalDir)
        {
            Status = ThreadStatus.None;
            _textBox = textbox;
            _stringIP = stringIP;
            _shopName = shopName;
            _ifGettingFile = ifGettingFile;
            _mask = mask;
            if (ifGettingFile)
            {
                _soucreDir = sourceDir;
                _finalDir = finalDir;
            }
            else
            {
                _soucreDir = finalDir;
                _finalDir = sourceDir;
            }

            StartThread();
        }

        public void StartThread()
        {
            var thread = new Thread(CopyFile) { Name = _shopName };
            thread.Start();
        }

        public void SetThreadToStop()
        {
            UpdateStatus(ThreadStatus.Stopping);
        }

        private void Stop()
        {
            //SetText("Получена команда \"Прервать\"", true);
            UpdateStatus(ThreadStatus.Stopped);
        }

        //void SetText(string text, bool isTextRed)
        //{
        //    if (textBox.InvokeRequired) textBox.Invoke(new Action<string>((s) => textBox.Text = s), text);
        //    else textBox.Text = text;

        //    System.Drawing.Color color;

        //    if (isTextRed)
        //        color = System.Drawing.Color.FromArgb(253, 100, 120);
        //    else
        //        color = System.Drawing.Color.YellowGreen;

        //    if (textBox.InvokeRequired) textBox.Invoke(new Action<System.Drawing.Color>((col) => textBox.BackColor = col), color);
        //    else textBox.BackColor = color;
            
        //}

        private void UpdateStatus(ThreadStatus newStatus)
        {
            UpdateStatusWithAdditionalInfo(newStatus, "");
        }
        private void UpdateStatusWithAdditionalInfo(ThreadStatus newStatus, string additionalInfo)
        {
            Status = newStatus;
            string newText = GetMessageByStatus(Status, additionalInfo);
            Color newColor = ThreadStatuses.GetColorByStatus(Status);
            if (_textBox.InvokeRequired)
                _textBox.Invoke(new Action<string, Color>((text, color) =>
                {
                    _textBox.Text = text;
                    _textBox.BackColor = color;
                }), newText, newColor);
            else
            {
                _textBox.Text = newText;
                _textBox.BackColor = newColor;
            }
        }

        private void CopyFile()
        {
            if (_textBox.InvokeRequired) _textBox.Invoke(new Action<Color>((col) => _textBox.BackColor = col), Color.FromArgb(240, 240, 240));
            else _textBox.BackColor = Color.FromArgb(240, 240, 240);
            bool isPingSuccessful = false;

            try
            {
                int count = 0;
                UpdateStatus(ThreadStatus.Connecting);
                while (!isPingSuccessful)
                {
                    if (Status == ThreadStatus.Stopping)
                    {
                        Stop();
                        return;
                    }

                    if (count < 6)
                    {
                        count++;
                        UpdateStatusWithAdditionalInfo(ThreadStatus.Connecting, count.ToString());
                        Ping ping = new Ping();

                        var pr = ping.Send(_stringIP);
                        if (pr != null && pr.Status == IPStatus.Success)
                            isPingSuccessful = true;
                    }
                    else
                    {
                        UpdateStatus(ThreadStatus.NoConnection);
                        break;
                    }
                }

                if (isPingSuccessful)
                {
                    UpdateStatus(ThreadStatus.TryingToCopy);
                    Thread.Sleep(1000);
                    var sourceDirectory = _ifGettingFile ? new DirectoryInfo($@"\\{_stringIP}\{_soucreDir}") : new DirectoryInfo(_soucreDir);
                    Thread.Sleep(2000);
                    UpdateStatus(ThreadStatus.GettingFilesList);
                    sourceDirectory.Refresh();
                    FileInfo[] fi = sourceDirectory.GetFiles(_mask);

                    var pathSave = _ifGettingFile ? $@"{_finalDir}\{_shopName}" : $@"\\{_stringIP}\{_finalDir}";
                    DirectoryInfo dits = new DirectoryInfo(pathSave);
                    if (!dits.Exists)
                        dits.Create();

                    if (fi.Length == 0)
                    {
                        UpdateStatus(ThreadStatus.NoFile);
                        return;
                    }


                    for (int i = 0; i < fi.Length; i++)
                    {
                        if (Status == ThreadStatus.Stopping)
                        {
                            Stop();
                            return;
                        }

                        UpdateStatusWithAdditionalInfo(ThreadStatus.CopyingFile, $"{(i + 1)} из {fi.Length}");
                        if (_ifGettingFile)
                        {
                            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
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
                    UpdateStatus(ThreadStatus.FileCopied);
                }
                
            }
            catch (Exception)
            {
                UpdateStatus(ThreadStatus.Error);
            }
        }
    }
}
