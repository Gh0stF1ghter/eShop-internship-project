import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCatalogComponent } from './admin-catalog.component';

describe('AdminCatalogComponent', () => {
  let component: AdminCatalogComponent;
  let fixture: ComponentFixture<AdminCatalogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminCatalogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminCatalogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
