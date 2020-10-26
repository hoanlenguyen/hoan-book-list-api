namespace HoanBookListData.Models.Paging
{
    public interface ISortable
    {
        string SortFieldName { get; set; }

        bool IsAscending { get; set; }
    }
}