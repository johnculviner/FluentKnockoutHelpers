using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    public class ZipCodeLocation
    {
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
