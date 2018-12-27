namespace TwentyFirst.Common.Models.Categories
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoriesChooseInputModel
    {
        public IEnumerable<string> CategoriesIds { get; set; }

        public IEnumerable<CategoryBaseViewModel> CategoryBaseViewModels { get; set; }

        public IEnumerable<SelectListItem> Categories
            => this.CategoryBaseViewModels
                .Select(a => new SelectListItem
                {
                    Value = a.Id,
                    Text = a.Name
                })
                .ToList();
    }
}
