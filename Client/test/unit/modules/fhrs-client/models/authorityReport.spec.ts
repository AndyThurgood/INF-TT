import AuthorityReport from '../../../../../src/modules/fhrs-client/models/authorityReport';
import Establishment from '../../../../../src/modules/fhrs-client/models/establishment';
import Rating from '../../../../../src/modules/fhrs-client/models/rating';

describe('the AuthorityReport model', () => {
    let mockEstablishments, mockRatings;
    beforeEach(() => {
        mockEstablishments = new Array<Establishment>();
        mockEstablishments.push(new Establishment({BusinessName: 'Bobs Burgers', RatingKey: 'fhrs_5_en-gb'}))
        mockEstablishments.push(new Establishment({BusinessName: 'Freds Fish', RatingKey: 'fhrs_5_en-gb'}))
        mockEstablishments.push(new Establishment({BusinessName: 'Mandys Meats', RatingKey: 'fhrs_1_en-gb'}))
        mockEstablishments.push(new Establishment({BusinessName: 'Trevors Toffees', RatingKey: 'fhrs_exempt_en-gb'}))
        
        mockRatings = [new Rating ('5-star','fhrs_5_en-gb', 1),
            new Rating('4-star', 'fhrs_4_en-gb', 1),
            new Rating('3-star', 'fhrs_3_en-gb', 1 ),
            new Rating('2-star', 'fhrs_2_en-gb', 1 ),
            new Rating('1-star', 'fhrs_1_en-gb', 1 ),
            new Rating('0-star', 'fhrs_0_en-gb', 1 ),
            new Rating('Awaiting Inspection', 'fhrs_awaitinginspection_en-gb', 1 ),
            new Rating('Awaiting Publication', 'fhrs_awaitingpublication_en-gb', 1 ),
            new Rating('Exempt', 'fhrs_exempt_en-gb', 1 )
        ];
    });

    it('maps the Authority Id', () =>{
        let authorityReport = new AuthorityReport(1, mockEstablishments, mockRatings);
        expect(authorityReport.authorityId).toBeDefined();
        expect(authorityReport.authorityId).toEqual(1);
    });

    it('generates the total number of establishments', () =>{
        let authorityReport = new AuthorityReport(1, mockEstablishments, mockRatings);
        expect(authorityReport.totalEstablishments).toBeDefined();
        expect(authorityReport.totalEstablishments).toEqual(4);
    });

    it('calculates the correct ratings', () =>{
        let authorityReport = new AuthorityReport(1, mockEstablishments, mockRatings);
        expect(authorityReport.ratings).toBeDefined();
        expect(authorityReport.ratings[0].percentage).toEqual(50.0);
        expect(authorityReport.ratings[4].percentage).toEqual(25.0);
        expect(authorityReport.ratings[8].percentage).toEqual(25.0);
    });

});