using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SurveyApp.Model.Models;
using SurveyApp.Model.Persistance;

namespace SurveyApp.Web.ApiControllers
{
    public class ZipCodeLocationController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public ZipCodeLocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ZipCodeLocation GetLocationForZip(int id /*zip code*/)
        {
            return _unitOfWork.ZipCodeLocations.First(x => x.ZipCode == id);
        }
    }
}
