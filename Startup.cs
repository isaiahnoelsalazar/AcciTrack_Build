using CSSimpleFunctions;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AcciTrack_Build
{
    public partial class Startup : Form
    {
        bool Started = false;
        PyCS pyCS = new PyCS();
        Thread thread, thread1;

        public Startup()
        {
            InitializeComponent();
        }

        private void Startup_Load(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(() =>
            {
                if (!File.Exists("pipdone"))
                {
                    pyCS.InstallPip();
                    pyCS.Pip(new string[]
                    {
                        "flask",
                        "PythonSimpleFunctions"
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
                            new MainForm(this, pyCS).Show();
                        }));
                        break;
                    }
                }
            }));
            thread.Start();
            thread1.Start();
        }

        private void Startup_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread.Abort();
            thread1.Abort();
        }
    }
}
