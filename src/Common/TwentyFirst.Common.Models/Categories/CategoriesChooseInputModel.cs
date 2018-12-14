namespace TwentyFirst.Common.Models.Categories
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;

    public class CategoriesChooseInputModel
    {
        public IEnumerable<string> CategoriesIds { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
