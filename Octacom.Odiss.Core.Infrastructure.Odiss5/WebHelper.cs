using System;
using System.Text.RegularExpressions;

namespace Octacom.Odiss.Core.Infrastructure.Odiss5
{
    internal static class WebHelper
    {
        internal static Guid? GetApplicationIdFromUri(this Uri uri)
        {
            var regex = new Regex(@"app/(.*?)(/|\z)", RegexOptions.IgnoreCase);
            var match = regex.Match(uri.ToString());
            var appId = match.Groups[1].ToString();

            Guid.TryParse(appId, out var result);

            if (result == default(Guid))
            {
                return null;
            }
            else
            {
                return result;
            }
        }
    }
}
