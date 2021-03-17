using System;
using System.Text.RegularExpressions;

namespace HR_Management.Extensions
{
    public static class RelateValidate
    {
        /// <summary>
        /// Validate Start/End Date
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        public static bool ValidateTimeInput(DateTime startDate, DateTime? endDate)
        {
            if (default(DateTime) == startDate) return false;

            if (endDate is null)
            {
                if (DateTime.Compare(startDate, DateTime.Now) > 0) return false;
            }
            else
            {
                if (startDate.Year > DateTime.Now.Year) return false;
                if (DateTime.Compare(startDate, Convert.ToDateTime(endDate)) >= 0) return false;
            }

            return true;
        }
    }
}
