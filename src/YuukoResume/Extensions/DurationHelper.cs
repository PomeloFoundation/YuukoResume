using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class DurationHelper
    {
        public static bool IsOneMonthDuration(this IHtmlHelper self, DateTime From, DateTime? To)
        {
            if (!To.HasValue)
                return false;
            if (From.Year == To.Value.Year && From.Month == To.Value.Month)
                return true;
            return false;
        } 
    }
}
