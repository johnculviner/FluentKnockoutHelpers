using System;
using System.Collections.Generic;

namespace SurveyApp.Model.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ZipCode { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<TechProduct> TechProducts { get; set; }
        public System.Drawing.KnownColor FavoriteColor { get; set; }
        public List<Relation> Children { get; set; }
        public List<Food> FavoriteFoods { get; set; }
    }
}
