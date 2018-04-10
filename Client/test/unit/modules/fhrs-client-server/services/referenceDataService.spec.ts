import ReferenceDataService from '../../../../../src/modules/fhrs-client-server/services/referenceDataService';
import Authority from '../../../../../src/modules/fhrs-client-server/models/authority';

describe('the ReferenceDataService', () => {
    let mockAuthorities, mockRatings, httpMock, referenceDataService;

    beforeEach(() => {
        mockAuthorities = [
            {name : 'Authority1', localAuthorityId: '1233', schemeType: '1'},
            {name : 'Authority2', localAuthorityId: '1234', schemeType: '1'},
            {name : 'Authority3', localAuthorityId: '1235', schemeType: '1'}
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
});