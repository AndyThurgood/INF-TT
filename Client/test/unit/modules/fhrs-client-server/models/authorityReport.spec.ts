import AuthorityReport from '../../../../../src/modules/fhrs-client-server/models/authorityReport';

describe('the AuthorityReport model', () => {
       
    it('maps the authority report properties', () =>{
        let mockReport = {"authorityId":198,"schemeType":0,"totalEstablishments":1777,"ratings":[
            {"name":"Pass","percentage":73.776027011817675,"ratingKey":"fhis_pass_en-gb","schemeType":2},
            {"name":"Pass and eat safe","percentage":1.6882386043894204,"ratingKey":"fhis_pass_and_eat_safe_en-gb","schemeType":2},
            {"name":"Improvement Required","percentage":9.0602138435565553,"ratingKey":"fhis_improvement_required_en-gb","schemeType":2},
            {"name":"Awaiting Inspection","percentage":3.4327518289251548,"ratingKey":"fhis_awaiting_inspection_en-gb","schemeType":2},
            {"name":"Awaiting Publication","percentage":0.0,"ratingKey":"fhis_awaiting_publication_en-gb","schemeType":2},
            {"name":"Exempt","percentage":12.042768711311199,"ratingKey":"fhis_exempt_en-gb","schemeType":2}]}
        let authorityReport = new AuthorityReport(mockReport);
        expect(authorityReport.authorityId).toBeDefined();
        expect(authorityReport.authorityId).toEqual(198);
        expect(authorityReport.ratings).toBeDefined();
    });
});