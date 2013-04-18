using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    //Labels are automatically split on camel casing when using FluentKnockoutHelpers: FirstName => First Name
    public class Survey
    {
        public int SurveyId { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Location HomeLocation { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public bool LikesBooleans { get; set; }

        [Url]
        [Required]
        public string FavoriteWebsite { get; set; }

        [Required]
        [Display(Name = "Favorite Color")]
        public System.Drawing.KnownColor FavoriteColorId { get; set; }
        
        public List<Relation> Children { get; set; }

        public List<TechProduct> TechProducts { get; set; }

        public List<Food> FavoriteFoods { get; set; }
    }
}
