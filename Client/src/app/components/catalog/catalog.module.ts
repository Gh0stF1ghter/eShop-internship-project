import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrandsComponent } from './brands/brands.component';
import { VendorsComponent } from './vendors/vendors.component';
import { CatalogRoutingModule } from './catalog-routing.module';
import { BrandComponent } from './brands/brand/brand.component';
import { VendorComponent } from './vendors/vendor/vendor.component';
import { ItemsOfTypeListComponent } from './/items-of-type-list/items-of-type-list.component';
import { CatalogComponent } from './catalog/catalog.component';
import { BasketComponent } from '../basket/basket.component';

@NgModule({
  declarations: [
    ItemsOfTypeListComponent,
    BasketComponent,
    BrandsComponent,
    BrandComponent,
    VendorsComponent,
    VendorComponent
  ],
  imports: [CommonModule, CatalogRoutingModule],
})
export class CatalogModule {}
