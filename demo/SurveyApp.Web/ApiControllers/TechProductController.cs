using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FluentKnockoutHelpers.Core.TypeMetadata;
using SurveyApp.Model.Models;
using SurveyApp.Model.Persistance;
using System.Data.Entity;
using SurveyApp.Web.ApiModels.Survey;
using SurveyApp.Web.Models;

namespace SurveyApp.Web.ApiControllers
{
    public class TechProductController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TechProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public KnockoutValidationResult ValidateSerialNumberUnique(ValidateSerialNumberUniqueDto dto)
        {
            //yes business logic in the controller... this is a demo afterall...
            if (_unitOfWork.TechProducts.Any(x => x.SerialNumber == dto.SerialNumber && 
                x.TechProductId != dto.TechProductId))
                
                return new KnockoutValidationResult
                    {
                        isValid = false,
                        message = "Someone already has this serial number. (Is it stolen?)"
                    };

            return new KnockoutValidationResult(); //no problems...
        }

        public class ValidateSerialNumberUniqueDto
        {
            public string SerialNumber { get; set; }
            public int TechProductId { get; set; }
        }
    }
}
