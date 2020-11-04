namespace HoanBookListData.Models.Paging
{
    public interface ISortable
    {
        string SortBy { get; set; }

        bool IsAsc { get; set; }
    }
}