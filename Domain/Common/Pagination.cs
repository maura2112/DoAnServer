namespace Domain.Common
{
    public class Pagination<T>
    {
        public int TotalItemsCount { get; set; }
        public int PageSize { get; set; }
        //public int TotalPagesCount
        //{
        //    get 
        //    {
        //        var temp = TotalItemsCount / PageSize;
        //        return TotalItemsCount % PageSize == 0 ? temp : temp + 1;
        //    }
        //}
        public int PageIndex { get; set; }

        ///// <summary>
        ///// page number start from 0
        ///// </summary>
        //public bool Next => PageIndex < TotalPagesCount;
        //public bool Previous => PageIndex > 1;
        public List<T>? Items { get; set; }
    }
}
