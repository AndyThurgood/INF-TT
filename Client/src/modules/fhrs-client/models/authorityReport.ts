import Establishment from './establishment'
import Rating from './rating'

/** A report model that pivots establishment data into percentage based result format. */
export default class AuthorityReport {
    constructor(authorityId: number, establishments: Array<Establishment>, ratings: Array<Rating>) {
        if (establishments) {
            this.totalEstablishments = establishments.length;
            this.authorityId = authorityId;
            ratings.forEach((rating => {
                rating.percentage =  (100.0 * (establishments.filter(x => x.ratingKey == rating.ratingKey).length / this.totalEstablishments));
                this.ratings.push(rating)
            }));
        }
    }
    public authorityId: number;
    public totalEstablishments: number;
    public ratings = Array();
}