import { Component } from '@angular/core';
import { Exam } from '../../../models/Exam';
import { QuestionTableComponent } from '../../../shared/question/question-table/question-table.component';

@Component({
  selector: 'app-resolve-exam',
  standalone: true,
  imports: [
    QuestionTableComponent
  ],
  templateUrl: './resolve-exam.component.html',
  styleUrl: './resolve-exam.component.scss'
})
export class ResolveExamComponent {
  public exam!: Exam;
  public questionCategory!: string;
  public answerCategory!: string;
}
