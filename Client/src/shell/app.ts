import { Router, RouterConfiguration } from 'aurelia-router';
import {PLATFORM} from 'aurelia-pal';

/** Application entry point */
export class App {
  public router: Router;

  public configureRouter(config: RouterConfiguration, router: Router) {
    try {
      config.title = 'Infinity Works Technical Assessment';
      config.map([
        { route: ['', 'client'], name: 'fhrs-client', moduleId: PLATFORM.moduleName('modules/fhrs-client/index'), nav: true, title: 'Client' },
        { route: 'client-server', name: 'fhrs-client-server', moduleId: PLATFORM.moduleName('modules/fhrs-client-server/index'), nav: true, title: 'Client-Server' },
      ]);
      this.router = router;
    }
    catch (error) {
      console.log(error)
    }
  }
}
