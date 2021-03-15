import { TestBed } from '@angular/core/testing';

import { SubwayService } from './subway.service';

describe('SubwayService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SubwayService = TestBed.get(SubwayService);
    expect(service).toBeTruthy();
  });
});
