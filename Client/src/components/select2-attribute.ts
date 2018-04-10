import { inject, customAttribute } from 'aurelia-framework';
import 'select2';
import 'select2/dist/css/select2.css';
import * as $ from 'jquery'

/** Custom control that allows for the use of a Select2 control within aurelia's binding system. */
@customAttribute('select2')
@inject(Element)
export class Select2CustomAttribute {
 private element;

  constructor(element) {
    this.element = element;
  }

  attached() {
    $(this.element).select2({
                placeholder: 'Please select an option......'
            })
      .on('change', (event) => {
          if (event.originalEvent) { return; }

          this.element.dispatchEvent(new Event('change'))
        });
  }

  detached() {
    $(this.element).select2('destroy');
  }
}
