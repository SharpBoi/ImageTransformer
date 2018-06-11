using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer.RequestHandlers
{
    public interface IRequestHandler
    {
        IRequestHandler Clone();
        bool IsRequestCanBeHandled(HttpListenerRequest req);
        void StartHandle(HttpListenerContext context);
        void InterruptHandle();
    }
}
