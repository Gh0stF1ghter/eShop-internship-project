import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTypeComponent } from './admin-type.component';

describe('AdminTypeComponent', () => {
  let component: AdminTypeComponent;
  let fixture: ComponentFixture<AdminTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminTypeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
