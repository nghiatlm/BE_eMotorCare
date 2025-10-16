namespace eMotoCare.BO.Pages
{
    public class PageResult<T>
    {
        public int PageCurrent { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T>? RowDatas { get; set; }

        public PageResult(List<T> items, int pageSize, int pageCurrent, int totalPages)
        {
            RowDatas = items;
            PageSize = pageSize;
            PageCurrent = pageCurrent;
            Total = totalPages;
        }
    }
}
