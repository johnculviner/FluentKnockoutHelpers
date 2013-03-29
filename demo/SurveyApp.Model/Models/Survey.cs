using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Model.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public Location FavoritePlace { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Date Of Death")]
        public DateTime? DateOfDeath { get; set; }

        [Display(Name = "Likes Music")]
        public bool LikesMusic { get; set; }

        [Display(Name = "Tech Products")]
        public List<TechProduct> TechProducts { get; set; }

        [Display(Name = "Favorite Color")]
        public System.Drawing.KnownColor FavoriteColor { get; set; }

        [Display(Name = "Children")]
        public List<Relation> Children { get; set; }

        [Display(Name = "Favorite Foods")]
        public List<Food> FavoriteFoods { get; set; }
    }
}
