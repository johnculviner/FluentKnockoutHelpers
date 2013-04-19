using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http;
using FluentKnockoutHelpers.Core.TypeMetadata;
using System.Drawing;
using System.Linq;
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

        [ExcludeMetadata]
        public IEnumerable<ColorData> Get()
        {
            return _colorService.GetAllColors();
        }
    }
}
