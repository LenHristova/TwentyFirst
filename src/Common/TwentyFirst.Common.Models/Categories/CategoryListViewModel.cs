﻿namespace TwentyFirst.Common.Models.Categories
{
    using Data.Models;
    using Mapping.Contracts;

    public class CategoryListViewModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
