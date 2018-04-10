import {Index} from '../../../../../src/modules/fhrs-client-server/index';
import Authority from '../../../../../src/modules/fhrs-client-server/models/authority';

describe('the index view model', () => {
    let mockReferenceService, mockReportService, mockRatings;
    let mockAuthorities = new Array<Authority>();
    mockAuthorities.push(new Authority({Name: 'Test1', LocalAuthorityId: '1234', SchemeType: '1' }));
    mockAuthorities.push(new Authority({Name: 'Test2', LocalAuthorityId: '1234', SchemeType: '1' }));
    mockAuthorities.push(new Authority({Name: 'Test3', LocalAuthorityId: '1234', SchemeType: '1' }));

    let mockReport = {
            "authorityId": 198, "schemeType": 0, "totalEstablishments": 1777, "ratings": [
                { "name": "Pass", "percentage": 73.776027011817675, "ratingKey": "fhis_pass_en-gb", "schemeType": 2 },
                { "name": "Pass and eat safe", "percentage": 1.6882386043894204, "ratingKey": "fhis_pass_and_eat_safe_en-gb", "schemeType": 2 },
                { "name": "Improvement Required", "percentage": 9.0602138435565553, "ratingKey": "fhis_improvement_required_en-gb", "schemeType": 2 },
                { "name": "Awaiting Inspection", "percentage": 3.4327518289251548, "ratingKey": "fhis_awaiting_inspection_en-gb", "schemeType": 2 },
                { "name": "Awaiting Publication", "percentage": 0.0, "ratingKey": "fhis_awaiting_publication_en-gb", "schemeType": 2 },
                { "name": "Exempt", "percentage": 12.042768711311199, "ratingKey": "fhis_exempt_en-gb", "schemeType": 2 }]
        }

    beforeEach(() => {
       mockReferenceService = {
           getAuthorities: () => new Promise((resolve =>{
               return resolve(mockAuthorities) 
            })),
       };

       mockReportService = {
           getEstablishmentReport: () => new Promise((resolve =>{
               return resolve(mockReport) 
            }))
       };
    })

    it('binds initial values', (done) => {
        let indexViewModel = new Index(mockReferenceService, mockReportService);
        indexViewModel.activate().then(() =>{
            expect(indexViewModel.fhrsAuthorities).toBeDefined();
            expect(indexViewModel.fhrsAuthorities).toBe(mockAuthorities);

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
           expect(indexViewModel.authorityReport.totalEstablishments).toBe(1777);
           expect(indexViewModel.authorityReport.authorityId).toBe(198);
           expect(indexViewModel.authorityReport.ratings[0]).toBeDefined();
           expect(indexViewModel.authorityReport.ratings[0].percentage).toBe(73.776027011817675);
           done();
       });
    });
});