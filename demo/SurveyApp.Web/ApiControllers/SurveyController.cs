using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SurveyApp.Model.Models;
using SurveyApp.Model.Persistance;
using System.Data.Entity;
using SurveyApp.Web.ApiModels.Survey;

namespace SurveyApp.Web.ApiControllers
{
    public class SurveyController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public SurveyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/survey
        public IEnumerable<SurveySummary> GetAll()
        {
            //TODO make queryable
            return _unitOfWork.Surveys.ToSurveySummaries();
        }

        // GET api/survey/5
        public Survey Get(int id)
        {
            return _unitOfWork.Surveys
                    .Include(x => x.TechProducts)
                    .Include(x => x.Children)
                    .Include("Children.Children")
                    .Include(x => x.FavoriteFoods)
                    .Include("FavoriteFoods.FoodGroup")
                    .First(x => x.SurveyId == id);
        }

        // POST api/survey
        public void Post(Survey survey)
        {

        }

        // DELETE api/survey/5
        public void Delete(int id)
        {

        }
    }
}
