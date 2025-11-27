import { TestBed } from '@angular/core/testing';

import { ConciliacaoContabilService } from './conciliacao-contabil.service';

describe('ConciliacaoContabilService', () => {
  let service: ConciliacaoContabilService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConciliacaoContabilService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
