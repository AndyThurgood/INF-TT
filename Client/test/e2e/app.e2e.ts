import {PageObject_App} from './app.po';
import {PageObject_Index} from './modules/index.po';
import {browser, element, by, By, $, $$, ExpectedConditions} from 'aurelia-protractor-plugin/protractor';
import {config} from '../protractor.conf';

describe('infinity works technical assessment', function() {
  let poApp: PageObject_App;
  let poIndex: PageObject_Index;

  beforeAll(async () => {
    poApp = new PageObject_App();
    poIndex = new PageObject_Index();

    await browser.loadAndWaitForAureliaPage(`http://localhost:${config.port}`);
  });

  // app wide tests
  it('should load the page and display the initial page title', async () => {
    await browser.waitForRouterComplete();
    await expect(poApp.getCurrentPageTitle()).toBe('Client | Infinity Works Technical Assessment');
  });

  it('should display greeting', async () => {
    await expect(poApp.getGreeting()).toBe('Infinity Works Technical Assessment');
  });

  it('should display sub greeting', async () => {
    await expect(poApp.getSubGreeting()).toBe('FHRS API - Establishment Analysis.');
  });

  it('should provide a top down list of local authorities', async () => {
    await expect(poIndex.getAuthorityOptionCount()).toBeGreaterThan(0);
  });

  it('should have a default selection value', async () => {
    await expect(poIndex.getAuthorityDropdownPlaceholderValue()).toBe('Please select an option......');
  });

  it('should allow for selection of an authority', async () => {
    await poIndex.setAuthorityDropdownValue("Leeds");
    await expect(poIndex.getAuthorityDropdownValue()).toBe("Leeds");
  });

  it('should display the selected authority', async () => {
    await browser.wait(
      ExpectedConditions.textToBePresentInElement(
        poIndex.getAuthorityHeader(), 'Leeds'
      ), 25000
    );
  });

  it('should display a tabular list of ratings with associated percentage', async () => {
    await expect(poIndex.getRatingTableRowCount()).toBeGreaterThan(0);
  });

});
