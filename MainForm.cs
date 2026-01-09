using CSSimpleFunctions;
using System;
using System.Windows.Forms;

namespace AcciTrack_Build
{
    public partial class MainForm : Form
    {
        Startup Startup;
        PyCS pyCS;

        public MainForm(Startup Startup, PyCS pyCS)
        {
            InitializeComponent();
            this.Startup = Startup;
            this.pyCS = pyCS;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Startup.Hide();
            webView21.Source = new Uri("http://127.0.0.1:5000/");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            webView21.Source = new Uri("http://127.0.0.1:5000/exit");
            pyCS.Stop();
            Startup.Close();
        }
    }
}
