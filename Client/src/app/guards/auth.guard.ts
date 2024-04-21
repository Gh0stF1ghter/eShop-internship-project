import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthService);

  let tokenId = authService.getUserId()
  if (tokenId === route.params['userId']) {
    return true;
  }

  console.log('Ids dont match' + tokenId + ' and ' + route.params['userId']);
  
  return false;
};