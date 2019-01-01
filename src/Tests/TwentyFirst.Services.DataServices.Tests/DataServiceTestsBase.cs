namespace TwentyFirst.Services.DataServices.Tests
{
    using Common.Mapping;
    using Common.Models.Articles;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    //xUnit creates a new instance of the test class for every test that is run,
    //so any code which is placed into the constructor of the test class will be run for every single test.
    //This makes the constructor a convenient place to put reusable context setup code
    //where to share the code without sharing object instances
    //=> get a clean copy of the context object(s) for every test that is run.
    public class DataServiceTests : IDisposable //shared setup/cleanup code without sharing object instances
    {
        protected readonly TwentyFirstDbContext dbContext;

        //set test context
        public DataServiceTests()
        {
            this.dbContext = new TwentyFirstDbContext(this.GetDbOptions());
            this.SetMapper();
        }

        //clean test context
        public void Dispose()
        {
            this.dbContext.Dispose();
        }

        private DbContextOptions<TwentyFirstDbContext> GetDbOptions()
            => new DbContextOptionsBuilder<TwentyFirstDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        private void SetMapper()
        {
            try
            {
                AutoMapperConfig.RegisterMappings(typeof(ArticleCreateInputModel).Assembly);
            }
            catch (InvalidOperationException)
            {
                //Do nothing -> AutoMapper is already initialized
            }
        }
    }
}
