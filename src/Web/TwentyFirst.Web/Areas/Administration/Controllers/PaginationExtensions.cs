namespace TwentyFirst.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using X.PagedList;

    public static class PaginationExtensions
    {
        public static async Task<IPagedList<T>> PaginateAsync<T>(
            this IList<T> collection,
            int? pageNumber,
            int itemsOnPage)
        {
            var validPageNumber = ValidatePage(pageNumber, collection.Count, itemsOnPage);
            return await collection.ToPagedListAsync(validPageNumber, itemsOnPage);
        }

        private static int ValidatePage(int? page, int allItemsCount, double itemsOnPage)
        {
            var pagesCount = Math.Ceiling(allItemsCount / itemsOnPage);
            return IsValidPage(page, pagesCount)
                ? page.Value
                : 1;
        }

        private static bool IsValidPage(int? page, double pagesCount)
            => page.HasValue && page.Value >= 1 && page.Value <= pagesCount;
    }
}
