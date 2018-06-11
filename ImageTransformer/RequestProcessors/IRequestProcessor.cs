using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer.RequestProcessors
{
    public interface IRequestProcessor
    {
        IRequestProcessor Clone();
        bool IsRequestProcessable(HttpListenerRequest req);
        void StartProcessing(HttpListenerContext context);
        void InterruptProcessing();
    }
}
