namespace Keyavi.Logs.Sdk.Core;

public static class PaginationExtensions
{
    public static bool IsLastPage(this Pagination pagination)
    {
        if (pagination == null)
        {
            return true;
        }

        if (pagination.ItemsInPage == 0)
        {
            return true;
        }

        if (pagination.TotalRecords < pagination.PageSize)
        {
            return true;
        }


        return (pagination.PageSize * pagination.Page - 1) + pagination.ItemsInPage >= pagination.TotalRecords;
    }
}

