/*
Copyright (c) 2023 Convergence Systems Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

namespace CSLibrary.IO
{
    public class AsyncWriteFile : IDisposable
    {
        private Thread mWriteFileThread = null;
        private Queue messageQueue = null;
        private Queue synchQ = null;
        private object myLock = new object();
        private int stopFlag = 0;
        private int stopped = 0;
        private AutoResetEvent startedThread = new AutoResetEvent(false);
        //private string filepath = null;
        private FileStream file = null;
        private StreamWriter sw = null;
        private bool orginalStarted = false;
        private bool addDateToFile = true;
        private string date = "";
        private string path = "";
        private string filename = "";
        private string fileextension = "txt";
        private bool dateChange = false;

        public string Date
        {
            get { lock (myLock) return date; }
            set
            {
                lock (myLock)
                {
                    if (date != value)
                    {
                        date = value;
                        dateChange = true;
                    }
                }
            }
        }

        public string Path
        {
            get { lock (myLock) return path; }
            set
            {
                lock (myLock)
                {
                    if (path != value)
                    {
                        path = value;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
            }
        }

        public string FileName
        {
            get { lock (myLock) return filename; }
            set
            {
                lock (myLock)
                {
                    if (filename != value)
                    {
                        filename = value;
                        /*orginalStarted = mWriteFileThread != null && mWriteFileThread.IsAlive;

                        if (orginalStarted)
                        {
                            StopMonitor();
                        }

                        CreateNewFile();

                        if (orginalStarted)
                        {
                            StartMonitor();
                        }*/
                    }
                }
            }
        }

        public string FileExtension
        {
            get { lock (myLock) return fileextension; }
            set { lock (myLock) fileextension = value; }
        }

        public bool AddDateToFileName
        {
            get { lock (myLock) return addDateToFile; }
            set { lock (myLock) addDateToFile = value; }
        }


        public String FullFilePath
        {
            get
            {
                lock (myLock)
                {
                    if (addDateToFile)
                        return string.Format("{0}{1}{2}.{3}", path, filename, date, fileextension);
                    else
                        return string.Format("{0}{1}.{2}", path, filename, fileextension);
                }
            }
        }


        public AsyncWriteFile()
        {
            Init(this.path, this.filename, this.date, false);
        }

        public AsyncWriteFile(string path, string filename, string date, bool addDateToFile)
        {
            Init(path, filename, date, addDateToFile);
        }

        public void Dispose()
        {
            CleanBuffer();
            StopMonitor();
            if (sw != null)
            {
                sw.Close();
                sw = null;
            }
            if (file != null)
            {
                file.Close();
                file = null;
            }
        }

        public void CreateNewFile()
        {
            if (dateChange)
            {
                orginalStarted = mWriteFileThread != null && mWriteFileThread.IsAlive;

                if (orginalStarted)
                {
                    StopMonitor();
                }

                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                if (file != null)
                {
                    file.Close();
                    file = null;
                }
                file = new FileStream(FullFilePath, FileMode.Append, FileAccess.Write, FileShare.Read | FileShare.Delete);
                sw = new StreamWriter(file);
                sw.AutoFlush = true;
                if (orginalStarted)
                {
                    StartMonitor();
                }
                dateChange = false;
            }
            
        }
        private void Init(string path, string filename, string date, bool addDateToFile)
        {
            try
            {
                this.Path = path;
                this.filename = filename;
                this.date = date;
                this.addDateToFile = addDateToFile;

                //CreateNewFile();

                messageQueue = new Queue();
                synchQ = Queue.Synchronized(messageQueue);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private void ThreadStart()
        {
            startedThread.Set();
            if (file == null || sw == null)
            {
                file = new FileStream(FullFilePath, FileMode.Append, FileAccess.Write, FileShare.Read | FileShare.Delete);
                sw = new StreamWriter(file);
                sw.AutoFlush = true;
            }

            while (!Interlocked.Equals(stopFlag, 1))
            {
                if (synchQ.Count > 0)
                {
                    sw.WriteLine((string)synchQ.Dequeue());
                    //sw.Flush();
                }

                Thread.Sleep(1);
            }
            Interlocked.Exchange(ref stopped, 1);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartMonitor()
        {
            if (mWriteFileThread == null || !mWriteFileThread.IsAlive)
            {
                Interlocked.Exchange(ref stopFlag, 0);

                mWriteFileThread = new Thread(new ThreadStart(ThreadStart));
                mWriteFileThread.IsBackground = true;
                mWriteFileThread.Start();
                startedThread.WaitOne();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StopMonitor()
        {
            Interlocked.Exchange(ref stopFlag, 1);

            if (mWriteFileThread != null)
                mWriteFileThread.Join();
            //ClearBuffer?
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CleanBuffer()
        {
            synchQ.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Write(string Message)
        {
            synchQ.Enqueue(Message);
        }
    }
}
