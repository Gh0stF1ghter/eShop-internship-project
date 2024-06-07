import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { userRoles } from '../constants/userRoles';

export const adminRoleGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthService);

  let role = authService.getUserRole()
  if (role === userRoles.admin) {
    return true;
  }

  console.log('Action not permitted for role ' + role);
  
  return false;
};
