using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentKnockoutHelpers.Core.TypeMetadata
{
    /// <summary>
    /// Exclude this class, or method from FluentKnockoutHelpers metadata generation
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExcludeMetadata : Attribute
    {
    }
}
