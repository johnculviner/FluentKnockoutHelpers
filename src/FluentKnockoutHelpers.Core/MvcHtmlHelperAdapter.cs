using System.Web.WebPages.Html;

namespace FluentKnockoutHelpers.Core
{
    public class WebPagesHtmlHelperAdapter : IHtmlHelperAdapter
    {
        private readonly HtmlHelper _htmlHelper;

        public WebPagesHtmlHelperAdapter(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public void WriteToOutput(string s)
        {
            _htmlHelper.Raw(s);
        }
    }
}
