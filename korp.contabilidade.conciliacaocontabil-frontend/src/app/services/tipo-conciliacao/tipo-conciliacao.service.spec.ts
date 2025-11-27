import { TestBed } from '@angular/core/testing';

import { TipoConciliacaoService } from './tipo-conciliacao.service';

describe('TipoConciliacaoService', () => {
  let service: TipoConciliacaoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TipoConciliacaoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
