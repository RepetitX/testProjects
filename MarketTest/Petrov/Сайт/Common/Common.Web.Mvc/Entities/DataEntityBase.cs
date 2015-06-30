using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Data;

namespace Common.Web.Mvc.Entities
{
    public class DataEntityBase<TKey> : IDataEntityBase<TKey>
    {
        [HiddenInput(DisplayValue = false)]
        public TKey Id { get; set; }

        [Required]
        [MinLength(1)]
        public byte[] Data { get; set; }
    }
}
