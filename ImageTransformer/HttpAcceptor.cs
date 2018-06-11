using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageTransformer
{
    public abstract class HttpAcceptor : IHttpAcceptor
    {
        #region Fields
        private HttpListener listener;
        private Thread listenThread;
        private List<Task> connTasks = new List<Task>();
        #endregion

        #region Funcs
        public void StartListenAsync(string url)
        {
            if (listener == null)
                listener = new System.Net.HttpListener();

            if (listener.IsListening == false)
            {
                listener.Prefixes.Clear();
                listener.Prefixes.Add(url);
                listener.Start();
                
                listenThread = new Thread(listen);
                listenThread.Start();
            }
        }
        public void StopListenAsync()
        {
            if (listener.IsListening)
            {
                lock (listener)
                {
                    listener.Stop();
                    listenThread.Abort();
                }
            }
        }
        private void listen()
        {
            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();

                OnRequestAccept(context);
            }
        }

        protected virtual void OnRequestAccept(HttpListenerContext context) { }
        #endregion

        #region Props
        public bool IsListening { get { return listener.IsListening; } }
        #endregion
    }
}
