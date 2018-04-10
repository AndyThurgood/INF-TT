// polyfill fetch client conditionally
const fetchPolyfill = !self.fetch
  ? import('isomorphic-fetch' /* webpackChunkName: 'fetch' */)
  : Promise.resolve(self.fetch);

import { Endpoint } from 'aurelia-api';
import { inject } from 'aurelia-framework';
import Establishment from '../models/establishment'

/** Report data service, responsible for grabbing report data. */
@inject(Endpoint.of('fhrs-api'))
export default class ReportDataService {
    private apiEndpoint;

    constructor(endpoint) {
        this.apiEndpoint = endpoint;
    }

    /** Get a list of establishments based on an authorityId and map them to the model*/
    public getEstablishments(authorityId: Number): Promise<Array<Establishment>> {
        return this.apiEndpoint.find('establishments', {localAuthorityId:authorityId, pagesize: 0})
            .then(response => response.establishments.map(e => new Establishment(e)))
            .catch(error => {
                console.log(error)
        })
    }
}
