import { inject } from 'aurelia-framework';
import ReferenceDataService from './services/referenceDataService';
import ReportDataService from './services/reportDataService';
import AuthorityReport from './models/authorityReport';


/** fhrs index ViewModel, responsible data orchestration and view logic */
@inject(ReferenceDataService, ReportDataService)
export class Index {

    private referenceDataService: ReferenceDataService;
    private reportDataService: ReportDataService;

    constructor(referenceDataService, reportDataservice) {
        this.referenceDataService = referenceDataService;
        this.reportDataService = reportDataservice;
    }

    public authorityReport: AuthorityReport;
    public isDataLoading: boolean = false
    public fhrsAuthorities;
    public selectedAuthority;

    /** ViewModel entry point */
    activate() {
        return this.getReferenceData();
    }

    /** Event call back  */
    changeCallback(event) {
        return this.getEstablishmentReport();
    }

    public getImgUrl(rating) {
      return require('../../../static/images/'+ rating +'.jpg')
    }

    /** Get reference data  */
    private getReferenceData() {
        return this.referenceDataService.getAuthorities().then(response => this.fhrsAuthorities = response)
    }

    /** Get and set report data */
    private getEstablishmentReport() {
        try {
            this.isDataLoading = true;
            return this.reportDataService.getEstablishmentReport(this.selectedAuthority.localAuthorityId, this.selectedAuthority.schemeType).then(response => {
                this.authorityReport = new AuthorityReport(response);
                this.isDataLoading = false
            });
        } catch (error) {
            console.log(error);
        }
    }
}
