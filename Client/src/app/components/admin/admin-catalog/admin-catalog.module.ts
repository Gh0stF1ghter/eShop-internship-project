import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminCatalogRoutingModule } from './admin-catalog-routing.module';
import { CreateBrandComponent } from './admin-brands/create-brand/create-brand.component';
import { CreateItemComponent } from './admin-items/create-item/create-item.component';
import { CreateTypeComponent } from './admin-types/create-type/create-type.component';
import { CreateVendorComponent } from './admin-vendors/create-vendor/create-vendor.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CreateItemComponent,
    CreateTypeComponent,
    CreateVendorComponent,
  ],
  imports: [
    AdminCatalogRoutingModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class AdminCatalogModule {}
