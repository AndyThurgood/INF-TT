import { Endpoint } from 'aurelia-api';
import { inject } from 'aurelia-framework';

/** Reference data service, responsible for handling application reference data retrieval. */
@inject(Endpoint.of('local-api'))
export default class ReferenceDataService {
    private apiEndpoint;

    constructor(endpoint) {
        this.apiEndpoint = endpoint;
    }

    /** Get all authorities available from the FHRS API */
    public getAuthorities(){
        return this.apiEndpoint.find('authority', 'authorities')
        .then(response => { return response; })
            .catch(error =>{
                console.log(error)
        });
    }
}