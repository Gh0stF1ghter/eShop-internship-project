import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminCatalogComponent } from './admin-catalog/admin-catalog.component';
import { AdminBrandsComponent } from './admin-catalog/admin-brands/admin-brands.component';
import { AdminVendorsComponent } from './admin-catalog/admin-vendors/admin-vendors.component';
import { AdminTypesComponent } from './admin-catalog/admin-types/admin-types.component';
import { AdminItemsComponent } from './admin-catalog/admin-items/admin-items.component';
import { AdminBrandComponent } from './admin-catalog/admin-brands/admin-brand/admin-brand.component';
import { AdminItemComponent } from './admin-catalog/admin-items/admin-item/admin-item.component';
import { AdminTypeComponent } from './admin-catalog/admin-types/admin-type/admin-type.component';
import { AdminVendorComponent } from './admin-catalog/admin-vendors/admin-vendor/admin-vendor.component';
import { CreateBrandComponent } from './admin-catalog/admin-brands/create-brand/create-brand.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AdminCatalogComponent,
    AdminBrandsComponent,
    CreateBrandComponent,
    AdminVendorsComponent,
    AdminTypesComponent,
    AdminItemsComponent,
    AdminBrandComponent,
    AdminItemComponent,
    AdminTypeComponent,
    AdminVendorComponent,
  ],
  imports: [AdminRoutingModule, CommonModule, FormsModule, ReactiveFormsModule],
})
export class AdminModule {}
