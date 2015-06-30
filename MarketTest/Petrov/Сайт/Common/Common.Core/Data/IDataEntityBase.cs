namespace Common.Data
{
    public interface IDataEntityBase<TKey> : IEntityBase<TKey>
    {
        byte[] Data { get; set; }
    }
}
