using System;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    public class Food
    {
        public Food()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [Range(0, 10)]
        public int HealthyScore { get; set; }
    }
}
