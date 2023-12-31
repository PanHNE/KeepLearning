import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Checkbox } from '../model/checkbox';
import { CheckboxComponent } from '../component/checkbox.component';

@Component({
  standalone: true,
  selector: 'app-checkbox-list',
  templateUrl: './checkbox-list.component.html',
  styleUrl: './checkbox-list.component.css',
  imports: [CheckboxComponent]
})
export class CheckboxListComponent {
  @Input({ required: true}) checkboxes!: Checkbox[];
  @Input({ required: true }) inOneLine!: boolean;

  @Output() changeCheckForCheckboxesEvent = new EventEmitter();

  checkOrUncheckChild(checkbox: Checkbox) {
    this.checkboxes = this.changeCheckForElement(this.checkboxes, checkbox);
    this.changeCheckForCheckboxesEvent.emit(this.checkboxes)
  }

  changeCheckForElement(checkboxes: Checkbox[], checkbox: Checkbox): Checkbox[] {
    let index = checkboxes.findIndex(c => c.value === checkbox.value);
    checkboxes.splice(index, 1, checkbox);
    return checkboxes;
  }
}
