import { TestBed } from '@angular/core/testing';

import { ItemOfTypeService } from './item-of-type.service';

describe('ItemOfTypeService', () => {
  let service: ItemOfTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ItemOfTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
