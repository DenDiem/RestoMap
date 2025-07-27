import { inject} from '@angular/core';
import {
  HttpErrorResponse,
  HttpResponse,
  HttpInterceptorFn,
} from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BASE_URL } from '../../app.config';

export const authorizeInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = inject(BASE_URL);
  const loginUrl = `${baseUrl}Identity/Account/Login`;
  const locationHref = `${loginUrl}?ReturnUrl=${window.location.pathname}`;

  return next(req).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.url?.startsWith(loginUrl)) {
          window.location.href = locationHref;
        }
        return throwError(() => error);
      }),
      // HACK: As of .NET 8 preview 5, some non-error responses still need to be redirected to login page.
      map((event) => {
        if (event instanceof HttpResponse && event.url?.startsWith(loginUrl)) {
          window.location.href = locationHref;
        }
        return event;
      })
    );
  }
