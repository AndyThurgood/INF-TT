import {browser, element, by, By, $, $$, ExpectedConditions} from 'aurelia-protractor-plugin/protractor';
import { get } from 'https';

export class PageObject_Index {
  
  getAuthorityOptionCount(){
    return element.all(by.tagName('option')).count();
  }

  getAuthorityDropdownPlaceholderValue(){
    return element(by.className('select2-selection__placeholder')).getText();
  }

  setAuthorityDropdownValue(value){
    return element(by.cssContainingText('option', value)).click();
  }

  getAuthorityDropdownValue(){
    return element(by.className('select2-selection__rendered')).getText();
  }

  getAuthorityHeader(){
    return element(by.tagName('h3')).element(by.tagName('strong'));
  }

  getRatingTableRowCount(){
    return element.all(by.css('tbody tr')).count();
  }
}
