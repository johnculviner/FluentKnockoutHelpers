using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using SurveyApp.Model.DomainModels;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Services
{
    public interface ITechProductService
    {
        ValidationResult ValidateSerialNumberUnique(ValidateSerialNumberUniqueDto dto);
    }

    public class TechProductService : ITechProductService
    {
        private readonly IDocumentSession _documentSession;

        public TechProductService(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ValidationResult ValidateSerialNumberUnique(ValidateSerialNumberUniqueDto dto)
        {
            //if (_documentSession.Query<TechProduct>().Any(x => x.SerialNumber == dto.SerialNumber &&
            //    x.TechProductId != dto.TechProductId))

            //    return new ValidationResult
            //    {
            //        HasError = true,
            //        ErrorMessage = "Someone already has this serial number. (Is it stolen?)"
            //    };

            return new ValidationResult(); //no problems...
        }
    }
}
