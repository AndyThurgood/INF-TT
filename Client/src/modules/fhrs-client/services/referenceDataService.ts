// polyfill fetch client conditionally
const fetchPolyfill = !self.fetch
  ? import('isomorphic-fetch' /* webpackChunkName: 'fetch' */)
  : Promise.resolve(self.fetch);
  
import { Endpoint } from 'aurelia-api';
import { inject } from 'aurelia-framework';
import Rating from '../models/rating';
import Authority from '../models/authority'

/** Reference data service, responsible for handling application reference data retrieval. */
@inject(Endpoint.of('fhrs-api'))
export default class ReferenceDataService {
    private apiEndpoint;

    constructor(endpoint) {
        this.apiEndpoint = endpoint;
    }

    /** Get all authorities available from the FHRS API and map them to our strongly typed class.*/
    public getAuthorities(): Promise<Array<Authority>>{
        return this.apiEndpoint.find('authorities', 'basic')
        .then(response => response.authorities.map(a => new Authority(a)))
            .catch(error =>{
                console.log(error)
        });
    }

    /** So the FHRS API provides a list of Ratings that can be used as an iterator 
     * see: http://api.ratings.food.gov.uk/ratings (even though it misses some ratingkeynames).
     * Using this as a mock endpoint in lieu, as the requirement states 'n-star' format. 
     ** Assumption - Return FHIS values also, so that Scottish authorities display a result. **
     ** Assumption - Return all rating types so that percentages make sense. ** */
    public getRatings() : Promise<Array<Rating>>  {
        return Promise.resolve(       
            [new Rating ('5-star','fhrs_5_en-gb', 1),
            new Rating('4-star', 'fhrs_4_en-gb', 1),
            new Rating('3-star', 'fhrs_3_en-gb', 1 ),
            new Rating('2-star', 'fhrs_2_en-gb', 1 ),
            new Rating('1-star', 'fhrs_1_en-gb', 1 ),
            new Rating('0-star', 'fhrs_0_en-gb', 1 ),
            new Rating('Awaiting Inspection', 'fhrs_awaitinginspection_en-gb', 1 ),
            new Rating('Awaiting Publication', 'fhrs_awaitingpublication_en-gb', 1 ),
            new Rating('Exempt', 'fhrs_exempt_en-gb', 1 ),
            new Rating('Pass', 'fhis_pass_en-gb', 2 ),
            new Rating('Pass and eat safe', 'fhis_pass_and_eat_safe_en-gb', 2),
            new Rating('Improvement Required', 'fhis_improvement_required_en-gb', 2 ),
            new Rating('Awaiting Inspection', 'fhis_awaiting_inspection_en-gb', 2 ),
            new Rating('Awaiting Publication', 'fhis_awaiting_publication_en-gb', 2 ),
            new Rating('Exempt', 'fhis_exempt_en-gb', 2 )
        ]
        );
    }
}
