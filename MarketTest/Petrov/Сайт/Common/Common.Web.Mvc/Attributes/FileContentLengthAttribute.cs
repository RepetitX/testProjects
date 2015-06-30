using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Common.Web.Mvc
{
    public class FileContentLengthAttribute : ValidationAttribute
    {
        private int SumContentLengthMax { get; set; }
        private int ContentLengthMax { get; set; }

        /// <summary>
        /// Проверяет размер загружаемых файлов. Может использоваться для HttpPostedFileBase или List<HttpPostedFileBase>
        /// Если contentLengthMax = 0, то файлы по отдельности не будут проверены
        /// Если sumContentLengthMax = 0, то суммарный размер файлов не будет проверен
        /// </summary>
        /// <param name="contentLengthMax">допустимый размер одного файла. Используется для HttpPostedFileBase и List<HttpPostedFileBase></param>
        /// <param name="sumContentLengthMax">допустимый размер всех файловю Используется только для List<HttpPostedFileBase></param>
        public FileContentLengthAttribute(int contentLengthMax, int sumContentLengthMax)
        {
            SumContentLengthMax = sumContentLengthMax*1024*1024;
            ContentLengthMax = contentLengthMax*1024*1024;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if(value is HttpPostedFileBase)
                {
                    var file = value as HttpPostedFileBase;
                    return file.ContentLength <= ContentLengthMax;
                }
                if(value is List<HttpPostedFileBase>)
                {
                    var files = value as List<HttpPostedFileBase>;
                    if (SumContentLengthMax != 0 && ContentLengthMax != 0)
                        return Sum(files) && Max(files);
                    if (SumContentLengthMax != 0)
                        return Sum(files);
                    if (ContentLengthMax != 0)
                        return Max(files);
                }
            }
            return true;
        }

        private bool Sum(IEnumerable<HttpPostedFileBase> files)
        {
            return files.Where(f => f != null).Sum(f => f.ContentLength) <= SumContentLengthMax;
        }

        private bool Max(IEnumerable<HttpPostedFileBase> files)
        {
            return files.Where(f => f != null).Max(f => f.ContentLength) <= ContentLengthMax;
        }
    }
}
