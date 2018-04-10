import ReportDataService from '../../../../../src/modules/fhrs-client/services/reportDataService';
import Establishment from '../../../../../src/modules/fhrs-client/models/establishment';

describe('the ReportDataService', () => {
    let mockEstablishments, httpMock, reportDataService;

    beforeEach(() => {
        mockEstablishments = {establishments: [
            {BusinessName : 'Establishment1', RatingKey: 'key2'},
            {BusinessName : 'Establishment2', RatingKey: 'key2'},
            {BusinessName : 'Establishment3', RatingKey: 'key2'}
        ]};
        
        let httpMock = {
            find: (endpoint, params) => new Promise((resolve =>{
               return resolve(mockEstablishments) 
            }))
        }

        reportDataService = new ReportDataService(httpMock);
    })

    /** TODO, add type equality check */
    it('returns authority data', (done) => {
         reportDataService.getEstablishments().then((response) => {
            expect(response).toBeDefined();
            expect(response.length).toEqual(3);
            expect(response[0]).toBeDefined();
            expect(response[0].name).toEqual('Establishment1');
            done();
         });
    });
});