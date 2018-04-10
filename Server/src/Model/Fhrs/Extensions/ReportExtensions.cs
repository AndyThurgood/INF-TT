using System;
using System.Collections.Generic;
using System.Linq;
using Model.Fhrs.Reports;

namespace Model.Fhrs.Extensions
{
    /// <summary>
    /// Move to own class..
    /// </summary>
    public static class ReportExtensions
    {
        public static AuthorityReport GetAuthorityReport(this IEnumerable<Establishment> establishments, int authorityId, IEnumerable<Rating> ratings)
        {
            if (authorityId == 0)
            {
                throw new ArgumentNullException(nameof(authorityId));
            }
            if (ratings == null)
            {
                throw new ArgumentNullException(nameof(ratings));
            }

            var establishmentList = establishments as IList<Establishment> ?? establishments.ToList();
            var authorityReport = new AuthorityReport
            {
                AuthorityId = authorityId,
                TotalEstablishments = establishmentList.Count
            };

            foreach (Rating rating in ratings)
            {
                rating.Percentage = (100.0 * establishmentList.Count(x => x.RatingKey == rating.RatingKey) / establishmentList.Count);
                authorityReport.Ratings.Add(rating);
            }

            return authorityReport;
        }
    }
}
