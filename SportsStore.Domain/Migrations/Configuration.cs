namespace SportsStore.Domain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SportsStore.Domain.Concrete;
    using SportsStore.Shared.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<DbProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DbProductContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Products.AddOrUpdate(i => i.Name
            , new Product { Name = "Kayak", Description = "A boat for on Preson", Category = "Watersports", Price = 275.00M }
            , new Product { Name = "Lifejacket", Description = "A boat for on Preson", Category = "Watersports", Price = 48.95M }
            , new Product { Name = "Soccer Ball", Description = "A boat for on Preson", Category = "Soccer", Price = 19.50M }
            , new Product { Name = "Corner Flags", Description = "A boat for on Preson", Category = "Soccer", Price = 34.95M }
            , new Product { Name = "Stadium", Description = "A boat for on Preson", Category = "Soccer", Price = 79500.00M }
            , new Product { Name = "Thinking Cap", Description = "A boat for on Preson", Category = "Chess", Price = 17.00M }
            , new Product { Name = "Unsteady Chair", Description = "A boat for on Preson", Category = "Chess", Price = 29.950M }
            , new Product { Name = "Human Chess Board", Description = "A boat for on Preson", Category = "Chess", Price = 75.00M }
            , new Product { Name = "Bling-Bling King", Description = "A boat for on Preson", Category = "Chess", Price = 1200.00M }
            );
        }
    }
}
