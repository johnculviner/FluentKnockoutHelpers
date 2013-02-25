using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Persistance
{
    //TODO fix leaky abstraction of DbSet once access patterns are known
    public interface IUnitOfWork
    {
        DbSet<Survey> Surveys { get; }
        DbSet<Food> Foods { get; }
        DbSet<FoodGroup> FoodGroups { get; }
        DbSet<Relation> Relations { get; }
        DbSet<TechProduct> TechProducts { get; }
        DbSet<ZipCodeLocation> ZipCodeLocations { get; }
        
        int Commit();
    }
}
