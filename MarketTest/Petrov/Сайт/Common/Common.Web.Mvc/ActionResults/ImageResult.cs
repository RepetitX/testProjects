using System;
using System.IO;
using System.Web.Mvc;

namespace Common.Web.Mvc
{
    public class ImageResult : ActionResult
    {
        public Stream ImageStream { get; private set; }
        public string ContentType { get; private set; }

        public ImageResult(Stream imageStream, string contentType)
        {
            if (imageStream == null)
                throw new ArgumentNullException("imageStream");

            if (contentType == null)
                throw new ArgumentNullException("contentType");

            ImageStream = imageStream;
            ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = ContentType;

            var buffer = new byte[4096];

            while (true)
            {
                var read = ImageStream.Read(buffer, 0, buffer.Length);

                if (read == 0)
                    break;

                response.OutputStream.Write(buffer, 0, read);
            }

            response.End();
        }
    }
}