using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyApp.Model.Models;
using SurveyApp.Model.Helpers;

namespace SurveyApp.Model.Persistance
{
    public class DataStore : DbContext
    {
        private const string DATABASE = "SurveyAppDb";

        static DataStore()
        {
            Database.SetInitializer(new DataStoreInitializer());
        }

        public DataStore()
            : base(DATABASE)
        {
        }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodGroup> FoodGroups { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<TechProduct> TechProducts { get; set; }
        public DbSet<ZipCodeLocation> ZipCodeLocations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //creates many-to-many between Survey and Food
            modelBuilder.Entity<Survey>()
                .HasMany(s => s.FavoriteFoods)
                .WithMany();

            //creates a many-to-many between Survey and TechProduct
            modelBuilder.Entity<Survey>()
            .HasMany(s => s.TechProducts)
            .WithMany();

            modelBuilder.Entity<ZipCodeLocation>()
                .HasKey(t => t.ZipCode)
                .Property(t => t.ZipCode)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
