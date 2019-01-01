namespace TwentyFirst.Common.Constants
{
    public static class GlobalConstants
    {
        //Role Constants
        public const string MasterAdministratorOrAdministrator = "Master Administrator, Administrator";

        public const string MasterAdministratorRoleName = "Master Administrator";

        public const string AdministratorRoleName = "Administrator";

        public const string EditorRoleName = "Editor";

        public const string MasterAdministratorUsernameConfiguration = "MasterAdministratorAccount:Username";

        //URL
        public const string AdministrationHomePage = "/Administration/Articles";

        public const string AdministrationLoginPage = "/Identity/Account/Login";

        public const string AdministrationAccountsPage = "/Identity/Account/Index";

        //HTML
        public const string HtmlTab = "&emsp;&emsp;";

        public const string HtmlNewLine = "<br />";

        public const string AlreadySubscribedEmail = "Този имейл вече е абониран за новини.";

        public const string ConfirmationEmailSend = "Изпратихме ви имейл за потвърждение";

        //Validation constants
        public const int MinPasswordLength = 6;

        public const int MaxPasswordLength = 100;

        public const int MinUsernameLength = 6;

        public const int MaxUsernameLength = 50;
        
        //Pagination Constants
        public const int AdministrationArticlesOnPageCount = 10;
        public const int ArticlesOnPageCount = 10;

        public const int AdministrationInterviewsOnPageCount = 10;
        public const int InterviewsOnPageCount = 10;

        public const int AdministrationCategoriesOnPageCount = 10;

        public const int PollsOnPageCount = 10;

        public const int AdministrationArticleEditsOnPageCount = 10;

        public const int AdministrationUsersOnPageCount = 3;

        //Category Constants
        public const int MainCategoriesCount = 3;

        //Image Constants
        public const string CorruptedImage = "Файлът \"{0}\" не е снимка или е повреден.";

        public const string SuccessfulImageUpload = "Всички снимки бяха качени успешно.";

        public const string SuccessfulImageDelete = "Снимката беше изтрита успешно.";

        public const string SuccessfulImageRecover = "Снимката беше възстановена успешно.";

        public const string SuccessfulPollDelete = "Анкетата беше изтрита успешно.";

        public const int ImageShortDescriptionMaxLength = 50;

        public const int ImagesOnSearchPageCount = 2;

        //Article Constants
        public const int MaxArticlesCountToGet = 3000;

        public const int AdminMaxArticlesCountToGet = int.MaxValue;

        public const int ArticleShortTitleMaxLength = 50;

        public const int TopArticlesCount = 4;
        public const int ImportantArticlesCount = 10;

        public const int ArticlesCountForFromCategoriesSection = 10;

        public const int ArticlesCountForLatestSection = 10;

        //Interview Constants
        public const int MaxInterviewsCountToGet = 3000;

        public const int InterviewsCountForLatestSection = 4;

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
