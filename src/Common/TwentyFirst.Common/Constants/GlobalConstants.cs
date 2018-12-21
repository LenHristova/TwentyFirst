namespace TwentyFirst.Common.Constants
{
    public static class GlobalConstants
    {
        //Role Constants
        public const string MasterAdministratorRoleName = "Master Administrator";

        public const string AdministratorRoleName = "Administrator";

        public const string EditorRoleName = "Editor";

        //URL
        public const string AdministrationHomePage = "/Administration/Articles";

        public const string AdministrationLoginPage = "/Identity/Account/Login";

        //HTML
        public const string HtmlTab = "&emsp;";

        public const string HtmlNewLine = "<br />";

        public const string AlreadySubscribedEmail = "Този имейл вече е абониран за новини.";

        public const string ConfirmationEmailSend = "Изпратихме ви имейл за потвърждение";
        
        //Pagination Constants
        public const int AdministrationArticlesOnPageCount = 10;

        public const int AdministrationInterviewsOnPageCount = 10;

        public const int AdministrationCategoriesOnPageCount = 10;

        //Image Constants
        public const int ImageShortDescriptionMaxLength = 50;

        public const int ImagesOnSearchPageCount = 2;

        //Article Constants
        public const int ConnectedArticleShortTitleMaxLength = 50;

        public const int ArticlesCountForFromCategoriesSection = 10;

        //Static html files
        public const string HtmlTemplatesFolderPath = "html-templates";

        public const string HtmlConfirmationEmailFilePath = "confirmation-email.html";

        public const string HtmlArticlesToEmailFilePath = "articles-to-email.html";

        public const string HtmlSingleArticleFilePath = "single-article.html";

        public const string HtmlConfirmationLinkPlaceholder = "{{confirmation-link}}";

        public const string HtmlUnsubscribeLinkPlaceholder = "{{unsubscribe-link}}";

        public const string HtmlImportantArticlesPlaceholder = "{{important-articles}}";

        public const string HtmlArticleTilePlaceholder = "{{article-title}}";

        public const string HtmlArticleLinkPlaceholder = "{{article-link}}";

        public const string SubscribeConfirmationEmailSubject = "Абонамент за новините на 21st";

        public const string ImportantArticlesEmailSubject = "Най-важните новини от 21st";
        
    }
}
