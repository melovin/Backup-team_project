import { TestBed } from '@angular/core/testing';

import { GuardClientDirtyGuard } from './guard-client-dirty.guard';

describe('GuardClientDirtyGuard', () => {
  let guard: GuardClientDirtyGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(GuardClientDirtyGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
