namespace Common.Data
{
    public interface INamedEntityBase<TKey> : IEntityBase<TKey>
    {
        string Name { get; set; }
    }
}
