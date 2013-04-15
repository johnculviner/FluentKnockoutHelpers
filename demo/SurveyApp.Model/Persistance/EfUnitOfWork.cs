using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
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
        }

        public IQueryable<Survey> Surveys { get; private set; }
        public IQueryable<Food> Foods { get; private set; }
        public IQueryable<FoodGroup> FoodGroups { get; private set; }
        public IQueryable<Relation> Relations { get; private set; }
        public IQueryable<TechProduct> TechProducts { get; private set; }

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
