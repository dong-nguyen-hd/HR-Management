namespace Business.Resources
{
    public class QueryResource
    {
        #region Property
        public int Page { get; set; }
        public int PageSize { get; set; }
        #endregion

        #region Constructor
        public QueryResource(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;

            if (Page <= 0)
                Page = 1;

            if (PageSize <= 0 || PageSize > 20)
                PageSize = 10;
        }
        #endregion
    }
}
