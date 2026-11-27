import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth';

/** Adds the JWT to every request and signs the user out when the API answers 401. */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.getToken();

  const request = token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

  return next(request).pipe(
    catchError((err: HttpErrorResponse) => {
      // A 401 on the login call just means wrong credentials, so leave that to the page.
      if (err.status === 401 && !req.url.includes('/auth/login') && token) {
        auth.logout();
      }
      return throwError(() => err);
    }),
  );
};
