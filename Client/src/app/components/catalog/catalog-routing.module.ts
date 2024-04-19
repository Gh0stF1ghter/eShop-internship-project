import { NgModule } from '@angular/core';
import { BrandsComponent } from './brands/brands.component';
import { VendorsComponent } from './vendors/vendors.component';
import { RouterModule, Routes } from '@angular/router';
import { ItemsOfTypeListComponent } from './items-of-type-list/items-of-type-list.component';
import { CatalogComponent } from './catalog/catalog.component';
import { BrandComponent } from './brands/brand/brand.component';
import { VendorComponent } from './vendors/vendor/vendor.component';

const catalogRoutes: Routes = [
  {
    path: '',
    component: CatalogComponent,
    children: [
      {
        path: 'types/:typeId',
        component: ItemsOfTypeListComponent,
      },
      {
        path: 'brands',
        component: BrandsComponent,
        children: [
          {
            path: ':brandId',
            component: BrandComponent,      
          },
        ],
      },
      {
        path: 'vendors',
        component: VendorsComponent,
        children: [
          {
            path: ':vendorId',
            component: VendorComponent,
          },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(catalogRoutes)],
  exports: [RouterModule],
})
export class CatalogRoutingModule {}
