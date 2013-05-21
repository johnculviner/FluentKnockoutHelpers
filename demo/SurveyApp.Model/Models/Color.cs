using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Model.Models
{
    public class Color
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [RegularExpression("^#?([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$", ErrorMessage = "The value must be a hex color code")]
        public string HexCode { get; set; }
    }
}
