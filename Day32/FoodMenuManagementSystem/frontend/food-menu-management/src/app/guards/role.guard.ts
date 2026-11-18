
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export const roleGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Check whether the user is logged in
  if (!authService.isAuthenticated()) {
    return router.createUrlTree(['/login']);
  }

  // Get roles allowed for this route
  const allowedRoles = route.data['roles'] as string[];

  // Get the logged-in user's role
  const userRole = authService.getUserRole();

  // Check role permission
  if (allowedRoles.includes(userRole ?? '')) {
    return true;
  }

  // Access denied
  return router.createUrlTree(['/unauthorized']);
};
