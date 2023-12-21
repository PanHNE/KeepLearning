import { Component, Input, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ContinentsCheckboxComponent } from '../../../shared/continents/continents-checkbox/continents-checkbox.component';
import { CategorySelectComponent } from '../../../shared/category/category-select/category-select.component';
import { NumberOfQuestionsSelectComponent } from '../../../shared/question/number-of-questions-select/number-of-questions-select.component';
import { Checkbox } from '../../../../common/checkbox/model/checkbox';

@Component({
  standalone: true,
  selector: 'app-generate-exam',
  templateUrl: './generate-exam.component.html',
  styleUrl: './generate-exam.component.scss',
  imports: [
    ContinentsCheckboxComponent,
    CategorySelectComponent,
    NumberOfQuestionsSelectComponent,
    RouterLink
  ]
})
export class GenerateExamComponent implements OnInit {
  @Input() continentsCheckbox: Checkbox[] = [];
  public continents: string[] = [];
  public url = 'http://localhost:4200/country/resolveExam'

  private route: ActivatedRoute = inject(ActivatedRoute);

  ngOnInit(): void {
    this.route.queryParamMap.subscribe( params => {
      this.continents = params.getAll('continents');
    });
  }

  checkOrUncheckChild(itemValue: string) {
    this.removeOrAddContinent(itemValue);
  }

  removeOrAddContinent(continent: string) {
    let foundContinent = this.continents.find(c => c === continent);

    if (foundContinent === undefined) {
      this.continents.push(continent)
    } else {
      this.continents = this.continents.filter(c => c !== continent);
    }
  }

  updateCheckedContinents(checkboxes: Checkbox[]) {
    this.continentsCheckbox = checkboxes;
    this.setContinentsToParam();
  }

  setContinentsToParam() {
    let checkedContinents = this.continentsCheckbox.filter(c => c.isChecked);
    this.continents = checkedContinents.map( c => c.value);
  }
}
