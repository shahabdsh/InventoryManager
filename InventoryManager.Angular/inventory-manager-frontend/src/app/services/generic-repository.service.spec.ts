import { TestBed } from '@angular/core/testing';

import { GenericRepositoryService } from './generic-repository.service';

describe('GenericRepositoryService', () => {
  let service: GenericRepositoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenericRepositoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
