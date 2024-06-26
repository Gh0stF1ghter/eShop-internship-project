import { NgModule } from '@angular/core';
import { adminRoleGuard } from '../../../guards/admin-role.guard';
import { RouterModule, Routes } from '@angular/router';
import { AdminBrandsComponent } from './admin-brands/admin-brands.component';
import { AdminTypesComponent } from './admin-types/admin-types.component';
import { AdminTypeComponent } from './admin-types/admin-type/admin-type.component';
import { AdminBrandComponent } from './admin-brands/admin-brand/admin-brand.component';
import { AdminVendorsComponent } from './admin-vendors/admin-vendors.component';
import { AdminVendorComponent } from './admin-vendors/admin-vendor/admin-vendor.component';
import { AdminItemsComponent } from './admin-items/admin-items.component';
import { AdminItemComponent } from './admin-items/admin-item/admin-item.component';
import { AdminCatalogComponent } from './admin-catalog.component';
import { CreateBrandComponent } from './admin-brands/create-brand/create-brand.component';
import { CreateVendorComponent } from './admin-vendors/create-vendor/create-vendor.component';
import { CreateItemComponent } from './admin-items/create-item/create-item.component';
import { CreateTypeComponent } from './admin-types/create-type/create-type.component';

const catalogRoutes: Routes = [
  {
    path: '',
    component: AdminCatalogComponent,
    canActivate: [adminRoleGuard],
    children: [
      {
        path: 'types',
        component: AdminTypesComponent,
        children: [
          {
            path:'create',
            component: CreateTypeComponent
          },
          {
            path: ':typeId',
            component: AdminTypeComponent,
          },
        ],
      },
      {
        path: 'brands',
        component: AdminBrandsComponent,
        children: [
          {
            path: 'create',
            component: CreateBrandComponent,
          },
          {
            path: ':brandId',
            component: AdminBrandComponent,
          },
        ],
      },
      {
        path: 'vendors',
        component: AdminVendorsComponent,
        children: [
          {
            path: 'create',
            component: CreateVendorComponent
          },
          {
            path: ':vendorId',
            component: AdminVendorComponent,
          },
        ],
      },
      {
        path: 'items',
        component: AdminItemsComponent,
        children: [
          {
            path: 'create',
            component: CreateItemComponent
          },
          {
            path: ':typeId/:itemId',
            component: AdminItemComponent,
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
export class AdminCatalogRoutingModule {}
