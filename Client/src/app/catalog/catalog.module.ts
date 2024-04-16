import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrandsComponent } from './brands/brands.component';
import { VendorsComponent } from './vendors/vendors.component';
import { CatalogRoutingModule } from './catalog-routing.module';

@NgModule({
  declarations: [BrandsComponent, VendorsComponent],
  imports: [CommonModule, CatalogRoutingModule],
})
export class CatalogModule {}
