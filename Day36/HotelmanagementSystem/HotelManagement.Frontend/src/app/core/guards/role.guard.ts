import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';
import { Role } from '../models/models';

/** Route data: { roles: ['Admin'] } - sends other users back to their own home page. */
export const roleGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const roles = (route.data['roles'] ?? []) as Role[];

  if (auth.hasRole(roles)) return true;

  return router.createUrlTree([auth.homeFor(auth.user()?.role)]);
};
