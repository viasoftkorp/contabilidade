import { TestBed } from '@angular/core/testing';

import { LegacyAdministrationService } from './legacy-administration.service';

describe('LegacyAdministrationProxyService', () => {
  let service: LegacyAdministrationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LegacyAdministrationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
