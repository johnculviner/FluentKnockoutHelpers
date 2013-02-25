using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentKnockoutHelpers.Core
{
    public interface IHtmlHelperAdapter
    {
        void WriteToOutput(string s);
    }
}
