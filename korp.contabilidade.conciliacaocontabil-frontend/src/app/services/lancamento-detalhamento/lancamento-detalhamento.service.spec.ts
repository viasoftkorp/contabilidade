import { TestBed } from '@angular/core/testing';

import { LancamentoDetalhamentoService } from './lancamento-detalhamento.service';

describe('LancamentoDetalhamentoService', () => {
  let service: LancamentoDetalhamentoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LancamentoDetalhamentoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
