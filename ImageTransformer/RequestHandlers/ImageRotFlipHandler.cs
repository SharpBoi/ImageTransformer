using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using ImageTransformer.Utilities;

namespace ImageTransformer.RequestHandlers
{
    public class ImageRotFlipHandler : IRequestHandler
    {
        #region Fields
        private Dictionary<string, RotateFlipType> cmd_action = new Dictionary<string, RotateFlipType>();

        private string cmd;
        private int[] rect;
        #endregion

        #region Funcs
        public ImageRotFlipHandler()
        {
            cmd_action.Add("rotate-cw", RotateFlipType.Rotate90FlipNone);
            cmd_action.Add("rotate-ccw", RotateFlipType.Rotate270FlipNone);
            cmd_action.Add("flip-h", RotateFlipType.RotateNoneFlipX);
            cmd_action.Add("flip-v", RotateFlipType.RotateNoneFlipY);
        }

        public IRequestHandler Clone()
        {
            return MemberwiseClone() as IRequestHandler;
        }

        public void InterruptHandle()
        {
        }

        public bool IsRequestCanBeHandled(HttpListenerRequest req)
        {
            if (req.HttpMethod.ToLower() == "post")
            {
                string uri = req.RawUrl;
                int cmdStart = 0, cmdEnd = 0;
                try
                {
                    cmdStart = uri.IndexOf("/process/") + 9;
                    cmdEnd = uri.LastIndexOf("/");
                    this.cmd = uri.Substring(cmdStart, cmdEnd - cmdStart);
                }
                catch
                {
                    Debug.WriteLine("Exception: while parsing command");
                    return false;
                }

                if (cmd_action.Keys.Contains(cmd))
                {
                    try
                    {
                        string csvRect = uri.Substring(cmdEnd + 1);
                        this.rect = Utility.ParseRect(csvRect);

                        return true;
                    }
                    catch
                    {
                        Debug.WriteLine("Exception: while parsing request arguments");
                        return false;
                    }
                }
                else
                {
                    Debug.WriteLine("");
                }
            }

            return false;
        }

        public void StartHandle(HttpListenerContext context)
        {
            byte[] imgBytes = new byte[context.Request.ContentLength64];
            context.Request.InputStream.Read(imgBytes, 0, imgBytes.Length);

            try
            {
                Bitmap img = Utility.PNGBytesToBitmap(imgBytes);

                img.RotateFlip(cmd_action[cmd]);

                imgBytes = Utility.BitmapToPNGBytes(img);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Write(imgBytes, 0, imgBytes.Length);
                context.Response.Close();
            }
            catch
            {
                Debug.WriteLine("Exception: cant parse png image");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.Close();
            }

        }
        #endregion
    }
}
