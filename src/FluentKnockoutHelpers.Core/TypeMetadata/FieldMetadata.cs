using System.Collections.Generic;

namespace FluentKnockoutHelpers.Core.TypeMetadata
{
    public class FieldMetadata
    {
        public FieldMetadata()
        {
            Rules = new List<ValidationRule>();
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ValidationRule> Rules { get; set; }
    }
}