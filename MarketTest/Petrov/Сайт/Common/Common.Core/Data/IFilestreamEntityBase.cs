using System;

namespace Common.Data
{
    public interface IFilestreamEntityBase<TKey> : IDataEntityBase<TKey>
    {
        Guid Guid { get; set; }
    }
}
