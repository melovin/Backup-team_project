import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddconfigbuttonComponent } from './addconfigbutton.component';

describe('AddconfigbuttonComponent', () => {
  let component: AddconfigbuttonComponent;
  let fixture: ComponentFixture<AddconfigbuttonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddconfigbuttonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddconfigbuttonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
