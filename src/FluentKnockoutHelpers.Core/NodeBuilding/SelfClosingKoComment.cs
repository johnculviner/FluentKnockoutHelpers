using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentKnockoutHelpers.Core.NodeBuilding
{
    /// <summary>
    /// A node representing a Knockout comment block that will immediately write its end tag
    /// </summary>
    public class SelfClosingKoComment : HtmlNode, IKoComment
    {
        /// <summary>
        /// Gets the beginning comment tag
        /// </summary>
        public override string BeginTagBegin
        {
            get { return "<!-- ko "; }
        }

        /// <summary>
        /// returns the self closing tag
        /// </summary>
        public override string BeginTagEnd
        {
            get { return " --><!-- /ko -->"; }
        }
    }
}