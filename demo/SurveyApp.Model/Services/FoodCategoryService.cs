using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Raven.Client;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Services
{
    public interface IFoodGroupService
    {
        IEnumerable<FoodGroup> GetAllFoodGroups();
        void Save(IEnumerable<FoodGroup> foodGroups);
    }

    public class FoodGroupService : IFoodGroupService
    {
        private readonly IDocumentSession _documentSession;

        public FoodGroupService(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public IEnumerable<FoodGroup> GetAllFoodGroups()
        {
            return _documentSession.Query<FoodGroup>().ToList();
        }

        public void Save(IEnumerable<FoodGroup> foodGroups)
        {
            _documentSession.Store(foodGroups);
        }
    }
}
