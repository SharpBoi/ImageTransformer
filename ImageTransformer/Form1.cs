using ImageTransformer.RequestHandlers;
using ImageTransformer.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTransformer
{
    public partial class Form1 : Form
    {
        HttpProcessor httpServer = new HttpProcessor();

        public Form1()
        {
            InitializeComponent();
            FormClosing += Form1_FormClosing;

            Logger.Init("./", 1024);
            Logger.Log(LogType.Info, "---==Server woke up==---");


            httpServer.AddRequestHandler(new ImageRotFlipHandler());
            httpServer.StartListenAsync("http://localhost:8080/");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StackTrace st = new StackTrace();

            var sf = st.GetFrame(0);

            httpServer.StopListenAsync();
        }
    }
}
