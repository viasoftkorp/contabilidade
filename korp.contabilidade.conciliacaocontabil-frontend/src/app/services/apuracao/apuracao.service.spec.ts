import { TestBed } from '@angular/core/testing';

import { ApuracaoService } from './apuracao.service';

describe('ApuracaoService', () => {
  let service: ApuracaoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApuracaoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
