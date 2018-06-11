using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ImageTransformer
{
    public class Delegates
    {
        public delegate void RequestHandler(HttpListenerRequest request);
    }
}
