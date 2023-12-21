export class Question {
    questionNumber: number;
    questionText: string;
    answerText: string;

    constructor( questionNumber: number, questionText: string, answerText: string) {
        this.questionNumber = questionNumber;
        this.questionText = questionText;
        this.answerText = answerText;
    }
}