using System.Collections.Generic;
using System.Web.Http;
using SurveyApp.Model.Persistance;
using SurveyApp.Web.Models.Api.Survey;

namespace SurveyApp.Web.Controllers.Api
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
            return _unitOfWork.Surveys.ToSurveySummaries(_unitOfWork.ZipCodeLocations);
        }

        // GET api/survey/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/survey
        public void Post([FromBody]string value)
        {
        }

        // PUT api/survey/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/survey/5
        public void Delete(int id)
        {
        }
    }
}
