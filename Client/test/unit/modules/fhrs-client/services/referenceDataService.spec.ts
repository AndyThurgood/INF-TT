import ReferenceDataService from '../../../../../src/modules/fhrs-client/services/referenceDataService';
import Rating from '../../../../../src/modules/fhrs-client/models/rating';
import Authority from '../../../../../src/modules/fhrs-client/models/authority';

describe('the ReferenceDataService', () => {
    let mockAuthorities, mockRatings, httpMock, referenceDataService;

    beforeEach(() => {
        mockAuthorities = {authorities: [
            {Name : 'Authority1', LocalAuthorityId: '1233', SchemeType: '1'},
            {Name : 'Authority2', LocalAuthorityId: '1234', SchemeType: '1'},
            {Name : 'Authority3', LocalAuthorityId: '1235', SchemeType: '1'}
        ]};
        
        mockRatings = [new Rating('5-star', 'fhrs_5_en-gb', 1),
        new Rating('4-star', 'fhrs_4_en-gb', 1),
        new Rating('3-star', 'fhrs_3_en-gb', 1),
        new Rating('2-star', 'fhrs_2_en-gb', 1),
        new Rating('1-star', 'fhrs_1_en-gb', 1),
        new Rating('0-star', 'fhrs_0_en-gb', 1),
        new Rating('Awaiting Inspection', 'fhrs_awaitinginspection_en-gb', 1),
        new Rating('Awaiting Publication', 'fhrs_awaitingpublication_en-gb', 1),
        new Rating('Exempt', 'fhrs_exempt_en-gb', 1)
        ];

        let httpMock = {
            find: (endpoint, params) => new Promise((resolve =>{
               return resolve(mockAuthorities) 
            }))
        }

        referenceDataService = new ReferenceDataService(httpMock);
    })

    /** TODO, add type equality check */
    it('returns authority data', (done) => {
         referenceDataService.getAuthorities().then((response) => {
            expect(response).toBeDefined();
            expect(response.length).toEqual(3);
            expect(response[0]).toBeDefined();
            expect(response[0].name).toEqual('Authority1');
            done();
         });
    });

    /**Pass through... */
    it('returns rating data', (done) => {
        referenceDataService.getRatings().then((response) => {
            expect(response).toBeDefined();
            expect(response.length).toEqual(15);
            done();
         });
    });
});