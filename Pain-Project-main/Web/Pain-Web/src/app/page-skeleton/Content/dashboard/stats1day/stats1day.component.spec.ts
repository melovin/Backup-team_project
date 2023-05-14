import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Stats1dayComponent } from './stats1day.component';

describe('Stats1dayComponent', () => {
  let component: Stats1dayComponent;
  let fixture: ComponentFixture<Stats1dayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Stats1dayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Stats1dayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
