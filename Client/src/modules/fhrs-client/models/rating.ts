/** Rating class, designed to hold data that is used as an index when
 * aggregating FHRS data.  */
export default class Rating{

    /** Create a new instance of the Rating class */
    constructor(name: string, ratingKey: string, schemeType: number){
        this.name = name;
        this.ratingKey = ratingKey;
        this.schemeType = Number(schemeType);
    }

    public name: string;
    public ratingKey: string;
    public schemeType: number;
    public percentage: number;
    public percentageDisplay(){
        return this.percentage.toFixed(1);
    }
}