using ImageTransformer.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer
{
    public class HttpProcessor : HttpAcceptor
    {
        #region Fields
        private List<IRequestHandler> detectors = new List<IRequestHandler>();
        private List<IRequestHandler> handlers = new List<IRequestHandler>();
        private List<Task> connsTasks = new List<Task>();
        #endregion

        #region Funcs
        public void AddRequestHandler(IRequestHandler reqHandler)
        {
            for (int i = 0; i < detectors.Count; i++)
                if (detectors[i].GetType() == reqHandler.GetType())
                {
                    Debug.WriteLine("Exception: Processor of passed type already exists");
                    throw new Exception("Processor of passed type already exists");
                }
            detectors.Add(reqHandler);
        }

        protected override void OnRequestAccept(HttpListenerContext context)
        {
            Task task = null;
            task = new Task(() => processRequest(context, task));
            task.Start();
        }

        private void processRequest(HttpListenerContext context, Task ownerTask)
        {
            for (int i = 0; i < detectors.Count; i++)
            {
                IRequestHandler detector = detectors[i];
                if (detector.IsRequestCanBeHandled(context.Request))
                {
                    IRequestHandler handler = detector.Clone();
                    handler.StartHandle(context);

                    return;
                }
            }
        }
        #endregion

        #region Props
        #endregion
    }
}
