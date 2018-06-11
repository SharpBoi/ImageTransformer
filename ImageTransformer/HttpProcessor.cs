using ImageTransformer.RequestProcessors;
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
        private List<IRequestProcessor> detectors = new List<IRequestProcessor>();
        private List<IRequestProcessor> processors = new List<IRequestProcessor>();
        private List<Task> connectionTasks = new List<Task>();
        #endregion

        #region Funcs
        public void AddRequestProcessor(IRequestProcessor reqProc)
        {
            for (int i = 0; i < detectors.Count; i++)
                if (detectors[i].GetType() == reqProc.GetType())
                {
                    Debug.WriteLine("Exception: Processor of passed type already exists");
                    throw new Exception("Processor of passed type already exists");
                }
            detectors.Add(reqProc);
        }

        protected override void OnRequestAccept(HttpListenerContext context)
        {
            Task task = new Task(() => processRequest(context));
            task.Start();
        }

        private void processRequest(HttpListenerContext context)
        {
            for (int i = 0; i < detectors.Count; i++)
            {
                IRequestProcessor detector = detectors[i];
                if (detector.IsRequestProcessable(context.Request))
                {
                    IRequestProcessor processor = detector.Clone();
                    processor.StartProcessing(context);

                    return;
                }
            }
        }
        #endregion

        #region Props
        #endregion
    }
}
