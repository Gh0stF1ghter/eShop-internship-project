import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypeDropdownComponent } from './type-dropdown.component';

describe('TypeDropdownComponent', () => {
  let component: TypeDropdownComponent;
  let fixture: ComponentFixture<TypeDropdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TypeDropdownComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TypeDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
