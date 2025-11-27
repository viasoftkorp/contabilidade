import { TestBed } from '@angular/core/testing';

import { ApuracaoDetalhamentoService } from './apuracao-detalhamento.service';

describe('ApuracaoDetalhamentoService', () => {
  let service: ApuracaoDetalhamentoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApuracaoDetalhamentoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
