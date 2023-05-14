import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepperEditComponent } from './stepperEdit.component';

describe('StepperComponent', () => {
  let component: StepperEditComponent;
  let fixture: ComponentFixture<StepperEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StepperEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StepperEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
