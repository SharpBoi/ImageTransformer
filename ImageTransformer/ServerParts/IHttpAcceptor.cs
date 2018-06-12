using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer.ServerParts
{
    public interface IHttpAcceptor
    {
        void StartListenAsync(string url);
        void StopListenAsync();

        bool IsListening { get; }
    }
}
