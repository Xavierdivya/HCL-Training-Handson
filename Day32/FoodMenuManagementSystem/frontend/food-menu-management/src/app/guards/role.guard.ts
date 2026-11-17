
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export const roleGuard: CanActivateFn = (route) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = route.data['roles'] as string[];

  if (!authService.isAuthenticated()) {
    return router.createUrlTree(['/login']);
  }

  const userRole = authService.getUserRole();

  if (allowedRoles.includes(userRole ?? '')) {
    return true;
  }

  return router.createUrlTree(['/unauthorized']);
};
