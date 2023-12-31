import { Component, Input, OnInit, Output, inject, EventEmitter } from '@angular/core';
import { ContinentMapper } from '../../../mappers/continent.mapper';
import { ContinentService } from '../../../services/continent.service';
import { CheckboxListComponent } from '../../../../common/checkbox/components/checkbox-list.component';
import { Checkbox } from '../../../../common/checkbox/model/checkbox';

@Component({
  selector: 'app-continents-checkbox',
  standalone: true,
  imports: [CheckboxListComponent],
  templateUrl: './continents-checkbox.component.html',
  styleUrl: './continents-checkbox.component.scss'
})
export class ContinentsCheckboxComponent implements OnInit { 
  @Input({ required: true }) inOneLine!: boolean;
  @Input({ required: true }) isForm!: boolean;
  @Input() continentsFromPath: string[] = [];

  @Output() updateCheckoboxesEvent = new EventEmitter<Checkbox[]>();

  public continentCheckboxes: Checkbox[] = [];

  private continentMapper: ContinentMapper = inject(ContinentMapper);
  private continentService: ContinentService = inject(ContinentService);

  ngOnInit(): void {
    this.getContinents();
  }

  getContinents(){
    this.continentService.getContinents().subscribe({
      next: (result) => {
        this.continentCheckboxes = this.continentMapper.mapToCheckbox(result, this.continentsFromPath);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  updateCheckoboxes(checkboxes: Checkbox[]) {
    this.continentCheckboxes = checkboxes;
    this.updateCheckoboxesEvent.emit(this.continentCheckboxes)
  }
}
