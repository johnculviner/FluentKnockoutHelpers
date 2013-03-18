using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Model.Models
{
    public class Location
    {
        [Display(Name = "Location")]
        public string FormattedLocation { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
