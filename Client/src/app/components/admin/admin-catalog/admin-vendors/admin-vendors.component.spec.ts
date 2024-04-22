import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVendorsComponent } from './admin-vendors.component';

describe('AdminVendorsComponent', () => {
  let component: AdminVendorsComponent;
  let fixture: ComponentFixture<AdminVendorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminVendorsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminVendorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
