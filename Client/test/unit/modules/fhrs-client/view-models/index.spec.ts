import {Index} from '../../../../../src/modules/fhrs-client/index';
import Authority from '../../../../../src/modules/fhrs-client/models/authority';
import Establishment from '../../../../../src/modules/fhrs-client/models/establishment';
import Rating from '../../../../../src/modules/fhrs-client/models/rating';

describe('the index view model', () => {
    let mockReferenceService, mockReportService, mockRatings;
    let mockAuthorities = new Array<Authority>();
    let mockEstablishments = new Array<Establishment>();

    mockAuthorities.push(new Authority({Name: 'Test1', LocalAuthorityId: '1234', SchemeType: '1' }));
    mockAuthorities.push(new Authority({Name: 'Test2', LocalAuthorityId: '1234', SchemeType: '1' }));
    mockAuthorities.push(new Authority({Name: 'Test3', LocalAuthorityId: '1234', SchemeType: '1' }));

    mockEstablishments.push(new Establishment({BusinessName: 'Test1', RatingKey: 'fhrs_5_en-gb'}));
    mockEstablishments.push(new Establishment({BusinessName: 'Test2', RatingKey: 'fhrs_5_en-gb'}));
    mockEstablishments.push(new Establishment({BusinessName: 'Test3', RatingKey: 'fhrs_1_en-gb'}));
    mockEstablishments.push(new Establishment({BusinessName: 'Test4', RatingKey: 'fhrs_1_en-gb'}));

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

    beforeEach(() => {
       mockReferenceService = {
           getAuthorities: () => new Promise((resolve =>{
               return resolve(mockAuthorities) 
            })),
            getRatings: () => new Promise((resolve =>{
               return resolve(mockRatings) 
            }))
       };

       mockReportService = {
           getEstablishments: () => new Promise((resolve =>{
               return resolve(mockEstablishments) 
            }))
       };
    })

    it('binds initial values', (done) => {
        let indexViewModel = new Index(mockReferenceService, mockReportService);
        indexViewModel.activate().then(() =>{
            expect(indexViewModel.fhrsAuthorities).toBeDefined();
            expect(indexViewModel.fhrsAuthorities).toBe(mockAuthorities);
            
            /**Test private props */
            expect((indexViewModel as any).ratings).toBeDefined();
            expect((indexViewModel as any).ratings).toBe(mockRatings);

            done();
        })
    });

    it('gets establishment data', (done) => {
       let indexViewModel = new Index(mockReferenceService, mockReportService);
       indexViewModel.selectedAuthority = new Authority({Name: 'Test1', LocalAuthorityId: '1234', SchemeType: '1' });
       (indexViewModel as any).ratings = mockRatings;
       indexViewModel.changeCallback(null).then(() =>{
           expect(indexViewModel.authorityReport).toBeDefined();
           expect(indexViewModel.authorityReport).toBeDefined();
           expect(indexViewModel.authorityReport.totalEstablishments).toBe(4);
           expect(indexViewModel.authorityReport.authorityId).toBe(1234);
           expect(indexViewModel.authorityReport.ratings[0]).toBeDefined();
           expect(indexViewModel.authorityReport.ratings[0].percentageDisplay()).toBe('50.0');
           done();
       });
    });
});