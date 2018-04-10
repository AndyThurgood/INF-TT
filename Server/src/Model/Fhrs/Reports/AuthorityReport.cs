using System.Collections.Generic;

namespace Model.Fhrs.Reports
{
    /// <summary>
    /// AuthorityReport that holds aggregated Establishment ratings.
    /// </summary>
    public class AuthorityReport
    {
        public AuthorityReport()
        {
            Ratings = new List<Rating>();
        }

        public int AuthorityId { get; set; }
        public int SchemeType { get; set; }
        public int TotalEstablishments { get; set; }
        public IList<Rating> Ratings { get; set; }
    }
}
