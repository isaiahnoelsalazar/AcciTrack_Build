using NETSimpleFunctions;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AcciTrack_Build
{
    public partial class MainForm : Form
    {
        bool Started = false;
        PyCS pyCS = new PyCS();
        Thread thread, thread1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(() =>
            {
                if (!File.Exists("pipdone"))
                {
                    pyCS.InstallPip();
                    pyCS.Pip(new string[]
                    {
                        "flask",
                        "PythonSimpleFunctions",
                        "flask_cloudflared"
                    });
                    SimpleFileHandler.Write("pipdone", string.Empty);
                }
                Started = true;
                pyCS.RunFile("main.py");
            }));
            thread1 = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    if (Started)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            webView21.Source = new Uri("http://127.0.0.1:5000/login");
                        }));
                        break;
                    }
                }
            }));
            thread.Start();
            thread1.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            webView21.Source = new Uri("http://127.0.0.1:5000/exit");
            pyCS.Stop();
        }
    }
}
