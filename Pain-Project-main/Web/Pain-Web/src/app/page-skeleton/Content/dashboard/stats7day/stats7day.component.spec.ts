import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Stats7dayComponent } from './stats7day.component';

describe('Stats7dayComponent', () => {
  let component: Stats7dayComponent;
  let fixture: ComponentFixture<Stats7dayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Stats7dayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Stats7dayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
