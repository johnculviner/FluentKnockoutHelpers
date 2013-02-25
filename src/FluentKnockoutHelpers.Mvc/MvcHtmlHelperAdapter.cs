using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentKnockoutHelpers.Core;

namespace FluentKnockoutHelpers.Mvc
{
    public class MvcHtmlHelperAdapter : IHtmlHelperAdapter
    {
        private readonly HtmlHelper _htmlHelper;

        public MvcHtmlHelperAdapter(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public void WriteToOutput(string s)
        {
            _htmlHelper.Raw(s);
        }
    }
}
