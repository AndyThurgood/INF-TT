/** Establishment model, used to generate report data. */
export default class Establishment{
constructor(establishment){
    this.name = establishment.BusinessName;
    this.ratingKey = establishment.RatingKey
}

    public name: string;
    public ratingKey: string;
}