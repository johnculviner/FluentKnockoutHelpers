using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Services
{
    public interface IColorService
    {
        IEnumerable<ColorData> GetAllColors();
    }

    public class ColorService : IColorService
    {
        public IEnumerable<ColorData> GetAllColors()
        {
            return Enum.GetValues(typeof(KnownColor))
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
}
