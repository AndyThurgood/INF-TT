/** Authority model that in this context is just reference data for a drop down. */
export default class Authority{

    constructor(authority){
        this.name = authority.Name;
        this.localAuthorityId = Number(authority.LocalAuthorityId);
        this.schemeType = Number(authority.SchemeType);
    }

    public name: string;
    public localAuthorityId: number;
    public schemeType: number;
}