using System;
using System.Collections.Generic;
using System.Linq;
using Model.Fhrs;
using Model.Fhrs.Extensions;
using Model.Fhrs.Reports;
using Xunit;

namespace UnitTests.Model
{
    
    public class ReportExtensionTests
    {
        private readonly IEnumerable<Establishment> _establishments;

        public ReportExtensionTests()
        {
            _establishments = new List<Establishment>
            {
                new Establishment { RatingKey = "fhrs_5_en-gb", RatingValue = "5"},
                new Establishment { RatingKey = "fhrs_5_en-gb", RatingValue = "5"},
                new Establishment { RatingKey = "fhrs_5_en-gb", RatingValue = "5"},
                new Establishment { RatingKey = "fhrs_5_en-gb", RatingValue = "5"},
                new Establishment { RatingKey = "fhrs_5_en-gb", RatingValue = "4"},
                new Establishment { RatingKey = "fhrs_4_en-gb", RatingValue = "4"},
                new Establishment { RatingKey = "fhrs_4_en-gb", RatingValue = "4"},
                new Establishment { RatingKey = "fhrs_0_en-gb", RatingValue = "0"},
                new Establishment { RatingKey = "fhrs_0_en-gb", RatingValue = "0"},
                new Establishment { RatingKey = "fhrs_0_en-gb", RatingValue = "0"},
            };    
        }

        [Fact]
        public void Report_Extension_GetAuthorityReport_Throws_IfAuthorityIsZero()
        {
            Assert.Throws<ArgumentNullException>(() => _establishments.GetAuthorityReport(0, null));
        }

        [Fact]
        public void Report_Extension_GetAuthorityReport_Throws_IfRatingsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => _establishments.GetAuthorityReport(197, null));
        }

        [Fact]
        public void Report_Extension_GetAuthorityReport_ReturnsValidResult()
        {
            IEnumerable<Rating> ratings = new List<Rating>
            {
                new Rating {Name = "5-Star", RatingKey="fhrs_5_en-gb", SchemeType=1 },
                new Rating {Name = "4-Star", RatingKey="fhrs_4_en-gb", SchemeType=1 },
                new Rating {Name = "3-Star", RatingKey="fhrs_3_en-gb", SchemeType=1 },
                new Rating {Name = "2-Star", RatingKey="fhrs_2_en-gb", SchemeType=1 },
                new Rating {Name = "1-Star", RatingKey="fhrs_1_en-gb", SchemeType=1 },
                new Rating {Name = "0-Star", RatingKey="fhrs_0_en-gb", SchemeType=1 },
                new Rating {Name = "Awaiting Inspection", RatingKey="fhrs_awaitinginspection_en-gb", SchemeType=1 },
                new Rating {Name = "Awaiting Publication", RatingKey="fhrs_awaitingpublication_en-gb", SchemeType=1 },
                new Rating {Name = "Exempt", RatingKey="fhrs_exempt_en-gb", SchemeType=1 },
                new Rating {Name = "Pass", RatingKey="fhis_pass_en-gb", SchemeType=2 },
                new Rating {Name = "Pass and eat safe", RatingKey="fhis_pass_and_eat_safe_en-gb", SchemeType=2 },
                new Rating {Name = "Improvement Required", RatingKey="fhis_improvement_required_en-gb", SchemeType=2 },
                new Rating {Name = "Awaiting Inspection", RatingKey="fhis_awaiting_inspection_en-gb", SchemeType=2 },
                new Rating {Name = "Awaiting Publication", RatingKey="fhis_awaiting_publication_en-gb", SchemeType=2 },
                new Rating {Name = "Exempt", RatingKey="fhis_exempt_en-gb", SchemeType=2 },
            };

            AuthorityReport authorityReport = _establishments.GetAuthorityReport(198, ratings);
            Assert.NotNull(authorityReport);
            Assert.Equal(198, authorityReport.AuthorityId);
            Assert.Equal(10, authorityReport.TotalEstablishments);
            Assert.Equal(50, authorityReport.Ratings.FirstOrDefault(x => x.RatingKey == "fhrs_5_en-gb").Percentage);
            Assert.Equal(20, authorityReport.Ratings.FirstOrDefault(x => x.RatingKey == "fhrs_4_en-gb").Percentage);
            Assert.Equal(30, authorityReport.Ratings.FirstOrDefault(x => x.RatingKey == "fhrs_0_en-gb").Percentage);
        }
    }
}
