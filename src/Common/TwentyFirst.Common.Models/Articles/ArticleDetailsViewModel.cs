namespace TwentyFirst.Common.Models.Articles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Categories;
    using Constants;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class ArticleDetailsViewModel : IMapFrom<Article>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Lead { get; set; }

        public string HtmlContent 
            => this.Content.Insert(0, GlobalConstants.HtmlTab)
                .Replace("\n", GlobalConstants.HtmlNewLine +
                               GlobalConstants.HtmlNewLine +
                               GlobalConstants.HtmlTab);

        public string Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public string ImageUrl { get; set; }

        public virtual IEnumerable<CategoryBaseViewModel> Categories { get; set; }

        public virtual IEnumerable<ArticleViewModel> ConnectedArticles { get; set; }

        public virtual IEnumerable<string> CategoriesIds
            => this.Categories.Select(c => c.Id).ToList();

        public string PublishedOnString
            => this.PublishedOn.UtcToEstFormatted().ToFormattedString();

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Article, ArticleDetailsViewModel>()
                .ForMember(dest => dest.Categories,
                    x => x.MapFrom(src => src.Categories.Where(ca => !ca.Category.IsDeleted).Select(ac => ac.Category)))
                .ForMember(dest => dest.ConnectedArticles,
                    x => x.MapFrom(src => src.ConnectedTo.Where(ca => !ca.ConnectedTo.IsDeleted).Select(ac => ac.ConnectedTo)));
        }
    }
}
