import { Endpoint } from 'aurelia-api';
import { inject } from 'aurelia-framework';

/** Report data service, responsible for grabbing report data. */
@inject(Endpoint.of('local-api'))
export default class ReportDataService {
    private apiEndpoint;

    constructor(endpoint) {
        this.apiEndpoint = endpoint;
    }

    /** Get a list of establishments based on an authorityId */
    public getEstablishmentReport(authorityId: number, schemeType: number){
        return this.apiEndpoint.find('establishment/establishmentReport', {id:authorityId, schemeType: schemeType})
        .then(response => { return response})
            .catch(error => {
                console.log(error)
        })
    }
}