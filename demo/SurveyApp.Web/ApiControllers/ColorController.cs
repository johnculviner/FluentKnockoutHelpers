using System.Collections.Generic;
using System.Web.Http;
using SurveyApp.Model.Models;
using SurveyApp.Model.Services;

namespace SurveyApp.Web.ApiControllers
{
    public class ColorController : ApiController
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        public IEnumerable<Color> Get()
        {
            return _colorService.Get();
        }

        public void Post(IEnumerable<Color> colors)
        {
            _colorService.Save(colors);
        }
    }
}
