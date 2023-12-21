import { Question } from "./Question";

export class Exam {
    name: string;
    category: string;
    continents: string[];
    questions: Question[];
  
    constructor(name: string, category: string, continents: string[], questions: Question[]) {
        this.name = name;
        this.category = category;
        this.continents = continents;
        this.questions = questions;
    }
  } 