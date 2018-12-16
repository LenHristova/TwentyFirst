namespace TwentyFirst.Common.Models.Articles
{
    using AutoMapper;
    using Data.Models;
    using Mapping.Contracts;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ArticleDeleteViewModel : IMapTo<Article>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Лийд")]
        public string Lead { get; set; }

        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Снимка")]
        public string ImageThumbUrl { get; set; }

        [Display(Name = "Топ новина?")]
        public bool IsTop { get; set; }

        [Display(Name = "Важна новина?")]
        public bool IsImportant { get; set; }

        [Display(Name = "Категории")]
        public IEnumerable<string> Categories { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Article, ArticleDeleteViewModel>()
                .ForMember(dest => dest.Categories,
                    x => x.MapFrom(
                        src => src.Categories.Where(ca => !ca.Category.IsDeleted).Select(ac => ac.Category.Name)));
        }
    }
}
