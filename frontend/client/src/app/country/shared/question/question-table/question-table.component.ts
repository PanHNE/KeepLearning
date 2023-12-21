import { Component, Input } from '@angular/core';
import { Question } from '../../../models/Question';

@Component({
  selector: 'app-question-table',
  standalone: true,
  imports: [],
  templateUrl: './question-table.component.html',
  styleUrl: './question-table.component.scss'
})
export class QuestionTableComponent{
  @Input({ required: true }) questions: Question[] = [];
  @Input({ required: true }) questionCategory: string = "";
  @Input({ required: true }) answerCategory: string = "";
}
