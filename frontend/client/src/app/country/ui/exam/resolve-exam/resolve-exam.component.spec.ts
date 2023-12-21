import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResolveExamComponent } from './resolve-exam.component';

describe('ResolveExamComponent', () => {
  let component: ResolveExamComponent;
  let fixture: ComponentFixture<ResolveExamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResolveExamComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ResolveExamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
