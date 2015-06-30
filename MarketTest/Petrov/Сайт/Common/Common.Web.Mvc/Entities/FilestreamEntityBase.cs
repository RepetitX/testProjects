using System;
using System.ComponentModel.DataAnnotations;
using Common.Data;

namespace Common.Web.Mvc.Entities
{
    public class FilestreamEntityBase<TKey> : DataEntityBase<TKey>, IFilestreamEntityBase<TKey>
    {
        [Required]
        public Guid Guid { get; set; }
    }
}