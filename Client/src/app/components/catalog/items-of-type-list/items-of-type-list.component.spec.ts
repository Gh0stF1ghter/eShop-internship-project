import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemsOfTypeListComponent } from './items-of-type-list.component';

describe('ItemsOfTypeListComponent', () => {
  let component: ItemsOfTypeListComponent;
  let fixture: ComponentFixture<ItemsOfTypeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemsOfTypeListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ItemsOfTypeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
