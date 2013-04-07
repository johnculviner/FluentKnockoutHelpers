using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentKnockoutHelpers.Core.TypeMetadata
{
    internal class TypeMetadata
    {
        public string TypeName { get; set; }
        public object Instance { get; set; }
        public IEnumerable<FieldValidationRules> FieldValidationRules { get; set; }
    }
}
