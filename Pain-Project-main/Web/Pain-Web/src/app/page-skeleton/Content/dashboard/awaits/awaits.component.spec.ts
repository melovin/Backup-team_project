import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AwaitsComponent } from './awaits.component';

describe('AwaitsComponent', () => {
  let component: AwaitsComponent;
  let fixture: ComponentFixture<AwaitsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AwaitsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AwaitsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
