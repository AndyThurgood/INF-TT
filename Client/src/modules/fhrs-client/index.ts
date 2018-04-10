import { inject } from 'aurelia-framework';
import ReferenceDataService from './services/referenceDataService';
import ReportDataService from './services/reportDataService';
import AuthorityReport from './models/authorityReport';
import Rating from './models/rating';
import Authority from './models/Authority';

/** fhrs index ViewModel, responsible data orchestration and view logic */
@inject(ReferenceDataService, ReportDataService)
export class Index {

    private referenceDataService: ReferenceDataService;
    private reportDataService: ReportDataService;
    private ratings: Array<Rating>;

    constructor(referenceDataService, reportDataservice) {
        this.referenceDataService = referenceDataService;
        this.reportDataService = reportDataservice;
    }

    public authorityReport: AuthorityReport;
    public isDataLoading: boolean = false
    public fhrsAuthorities: Array<Authority>;
    public selectedAuthority: Authority;

    /** ViewModel entry point */
    public activate() {
        return this.getReferenceData();
    }

    /** Event call back  */
    public changeCallback(event) {
        return this.getEstablishmentReport();
    }

    public getImgUrl(rating) {
        return require('../../../static/images/'+ rating +'.jpg')
    }

    /** Get reference data  */
    private getReferenceData() {
        try {
            return Promise.all([
            this.referenceDataService.getAuthorities().then(response => this.fhrsAuthorities = response),
            this.referenceDataService.getRatings().then(response => this.ratings = response)
            ]);
        }
        catch (error) {
            console.error(error);
        }
    }

    /** Get and set report data */
    private getEstablishmentReport() {
        try {
            this.isDataLoading = true;
            return this.reportDataService.getEstablishments(this.selectedAuthority.localAuthorityId).then(establishments => {
                this.authorityReport = new AuthorityReport(this.selectedAuthority.localAuthorityId, establishments, this.getRatingTypes(this.selectedAuthority.schemeType));
                this.isDataLoading = false;
            });
        }
        catch (error) {
            console.log(error)
        }
    }

    /** Get the FHRS or FHIS rating types */
    private getRatingTypes(schemeType: number): Array<Rating> {
        return this.ratings.filter(rating => rating.schemeType == schemeType);
    }
}