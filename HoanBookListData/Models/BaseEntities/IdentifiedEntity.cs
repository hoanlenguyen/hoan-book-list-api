namespace HoanBookListData.Models.BaseEntities
{
    public class IdentifiedEntity<T> : BaseEntity
    {
        public T Id { get; set; }
    }
}