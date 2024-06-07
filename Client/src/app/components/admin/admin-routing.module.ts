import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { adminRoleGuard } from '../../guards/admin-role.guard';
import { AdminComponent } from './admin.component';

const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [adminRoleGuard],
    children: [
      {
        path: 'catalog',
        loadChildren: () => import('./admin-catalog/admin-catalog.module').then(m => m.AdminCatalogModule)
      }
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(adminRoutes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
