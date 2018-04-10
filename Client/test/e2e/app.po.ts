import {browser, element, by, By, $, $$, ExpectedConditions} from 'aurelia-protractor-plugin/protractor';

export class PageObject_App {
  
  getCurrentPageTitle() {
    return browser.getTitle();
  }

  getGreeting() {
    return element(by.tagName('h1')).getText();
  }

  getSubGreeting() {
    return element.all(by.tagName('h5')).first().getText();
  }
}
