using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentKnockoutHelpers.Core.TypeMetadata;
using System.Data.Entity;
using SurveyApp.Model.Models;
using SurveyApp.Model.Services;
using SurveyApp.Web.Models;

namespace SurveyApp.Web.ApiControllers
{
    public class TechProductController : ApiController
    {
        private readonly ITechProductService _techProductService;

        public TechProductController(ITechProductService techProductService)
        {
            _techProductService = techProductService;
        }

        [HttpPost]
        public KnockoutValidationResult ValidateSerialNumberUnique(ValidateSerialNumberUniqueDto dto)
        {
            return new KnockoutValidationResult(_techProductService.ValidateSerialNumberUnique(dto));
        }
    }
}
