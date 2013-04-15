using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Persistance
{
    public interface IUnitOfWork
    {
        IQueryable<Survey> Surveys { get; }
        IQueryable<Food> Foods { get; }
        IQueryable<FoodGroup> FoodGroups { get; }
        IQueryable<Relation> Relations { get; }
        IQueryable<TechProduct> TechProducts { get; }
        
        int Commit();
    }
}
