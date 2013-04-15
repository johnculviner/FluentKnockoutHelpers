using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http;
using FluentKnockoutHelpers.Core.TypeMetadata;
using System.Drawing;
using System.Linq;

namespace SurveyApp.Web.ApiControllers
{
    public class ColorController : ApiController
    {
        [ExcludeMetadata]
        public IEnumerable<ColorData> Get()
        {
            return Enum.GetValues(typeof (KnownColor))
                .Cast<KnownColor>()
                .Select(Color.FromKnownColor)
                .Select(x => new ColorData
                    {
                        ColorId = x.ToKnownColor(),
                        ColorCode = ColorTranslator.ToHtml(x),
                        ColorName = Regex.Replace(x.Name, "([A-Z])", " $1", RegexOptions.Compiled)
                    });
        }
    }

    public class ColorData
    {
        public KnownColor ColorId { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
    }
}
