using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Common.Web.Mvc
{
    public class HttpPostedFileSizeAttribute : ValidationAttribute
    {
        public int MaxSize { get; private set; }

        /// <summary>
        /// Validate maximum size of uploaded file.
        /// </summary>
        /// <param name="maxSize">Maximum file size in bytes.</param>
        public HttpPostedFileSizeAttribute(int maxSize)
        {
            MaxSize = maxSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, Math.Round((MaxSize / 1024D), 2));
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file != null)
                return file.ContentLength <= (MaxSize * 1024);

            var files = value as IEnumerable<HttpPostedFileBase>;

            if (files != null && files.Any(f => f != null))
            {
                var totalContentLength = files.Where(f => f != null).Select(f => f.ContentLength).Aggregate((l1, l2) => l1 + l2);

                return totalContentLength <= (MaxSize * 1024);
            }

            return true;
        }
    }
}