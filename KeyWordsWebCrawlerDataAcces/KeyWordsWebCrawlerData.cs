namespace KeyWordsWebCrawlerDataAcces
{
    using KeyWordsWebCrawlerDomain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class KeyWordsWebCrawlerData : DbContext
    {
        // Your context has been configured to use a 'KeyWordsWebCrawlerData' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'KeyWordsWebCrawlerDataAcces.KeyWordsWebCrawlerData' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'KeyWordsWebCrawlerData' 
        // connection string in the application configuration file.
        public KeyWordsWebCrawlerData()
            : base("name=KeyWordsWebCrawlerData")
        {
        }

        public DbSet<SearchResultsHistory> SearchResultsHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}