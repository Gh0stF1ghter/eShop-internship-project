import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemsDropdownComponent } from './items-dropdown.component';

describe('ItemsDropdownComponent', () => {
  let component: ItemsDropdownComponent;
  let fixture: ComponentFixture<ItemsDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ItemsDropdownComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ItemsDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
