import {App} from '../../../src/shell/app';
class RouterStub {
  routes;
  
  configure(handler) {
    handler(this);
  }
  
  map(routes) {
    this.routes = routes;
  }
}

describe('the App module', () => {
  var sut, mockedRouter;

  beforeEach(() => {
    mockedRouter = new RouterStub();
    sut = new App();
    sut.configureRouter(mockedRouter, mockedRouter);
  });

  it('contains a router property', () => {
    expect(sut.router).toBeDefined();
  });

  it('configures the router title', () => {
    expect(sut.router.title).toEqual('Infinity Works Technical Assessment');
  });

  it('should have a default route', () => {
    expect(sut.router.routes).toContainEqual({ route: ['', 'client'], name: 'fhrs-client',  moduleId: 'modules/fhrs-client/index', nav: true, title:'Client' });
  });

   it('should have a client-server route', () => {
    expect(sut.router.routes).toContainEqual({ route: 'client-server', name: 'fhrs-client-server',  moduleId: 'modules/fhrs-client-server/index', nav: true, title:'Client-Server' });
  });
});
