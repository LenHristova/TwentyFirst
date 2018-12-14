namespace TwentyFirst.Services.DataServices.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Models.Articles;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IArticleService
    {
        Task<string> CreateAsync(ArticleCreateInputModel articleCreateInputModel, string creatorId);

        Task<IEnumerable<SelectListItem>> AllToSelectListItemsAsync();
    }
}
