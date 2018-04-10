import 'isomorphic-fetch';
import apiConfig from './config';
import 'font-awesome/css/font-awesome.css';
import 'bootstrap/dist/css/bootstrap.css';
import '../static/styles.css';
import "core-js/es6/promise";
import { Aurelia } from 'aurelia-framework';
import { PLATFORM } from 'aurelia-pal';

/** Aurelia main entry point. */
export async function configure(aurelia: Aurelia) {
  aurelia.use
    .standardConfiguration()
    .developmentLogging()
    .plugin(PLATFORM.moduleName('aurelia-api'), {
      endpoints: [
      apiConfig.fhrsEndpoint,
      apiConfig.localEndpoint
      ]
    });

  await aurelia.start();
  await aurelia.setRoot(PLATFORM.moduleName('shell/app'));
}
