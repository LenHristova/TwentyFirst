namespace TwentyFirst.Common.Models.Articles
{
    using System;
    using Data.Models;
    using Extensions;
    using Mapping.Contracts;

    public class ArticleEditsViewModel : IMapFrom<ArticleEdit>
    {
        public string EditorUserName { get; set; }

        public DateTime EditDateTime { get; set; }

        public string EditDateTimeOnString
            => this.EditDateTime.UtcToEstFormatted().ToFormattedString();
    }
}
