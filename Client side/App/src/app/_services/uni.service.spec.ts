import { TestBed } from '@angular/core/testing';

import { UniService } from './uni.service';

describe('UniServicesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UniService = TestBed.get(UniService);
    expect(service).toBeTruthy();
  });
});
