using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentKnockoutHelpers.Core.Settings
{
    /// <summary>
    /// settings governing how and if required tokens are enabled
    /// on the display name for non-nullable/required properties
    /// </summary>
    public class RequiredTokenSettings
    {
        public RequiredTokenSettings()
        {
            RequiredToken = "*";
            IsEnabled = true;
            RequiredTokenPosition = RequiredTokenPosition.LeftOfLabel;
        }


        /// <summary>
        /// Supply a custom required token to use something other than the default "*"
        /// </summary>
        public string RequiredToken { get; set; }

        /// <summary>
        /// Set whether or not to use a required token on the display name for non-nullable/required properties
        /// </summary>
        public bool IsEnabled { get; set; }

        public RequiredTokenPosition RequiredTokenPosition { get; set; }
    }

    public enum RequiredTokenPosition
    {
        LeftOfLabel,
        RightOfLabel
    }
}
