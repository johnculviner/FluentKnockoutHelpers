using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Persistance
{
    public class EfUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataStore _ctx;

        public EfUnitOfWork()
        {
            _ctx = new DataStore();
            Surveys = _ctx.Surveys;
            Foods = _ctx.Foods;
            FoodGroups = _ctx.FoodGroups;
            Relations = _ctx.Relations;
            TechProducts = _ctx.TechProducts;
            ZipCodeLocations = _ctx.ZipCodeLocations;
        }

        public DbSet<Survey> Surveys { get; private set; }
        public DbSet<Food> Foods { get; private set; }
        public DbSet<FoodGroup> FoodGroups { get; private set; }
        public DbSet<Relation> Relations { get; private set; }
        public DbSet<TechProduct> TechProducts { get; private set; }
        public DbSet<ZipCodeLocation> ZipCodeLocations { get; private set; }

        public int Commit()
        {
            return _ctx.SaveChanges();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
