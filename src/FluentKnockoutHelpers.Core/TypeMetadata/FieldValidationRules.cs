using System.Collections.Generic;

namespace FluentKnockoutHelpers.Core.TypeMetadata
{
    public class FieldValidationRules
    {
        public FieldValidationRules()
        {
            Rules = new List<ValidationRule>();
        }
        public List<ValidationRule> Rules { get; set; }
        public string FieldName { get; set; }
    }
}