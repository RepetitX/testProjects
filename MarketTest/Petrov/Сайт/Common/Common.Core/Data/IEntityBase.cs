namespace Common.Data
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }
    }
}
