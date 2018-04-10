/** A report model that pivots establishment data into percentage based result format. */
export default class AuthorityReport {
    constructor(authorityReport) {
       this.authorityId = Number(authorityReport.authorityId);
       this.totalEstablishments = Number(authorityReport.totalEstablishments);
       this.ratings = authorityReport.ratings;
    }
    public authorityId: number;
    public totalEstablishments: number;
    public ratings = Array();
}