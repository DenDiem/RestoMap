import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { APP_ID, ApplicationConfig, InjectionToken, provideExperimentalZonelessChangeDetection } from "@angular/core";
import { authorizeInterceptor } from "./shared/interceptors/authorize.interceptor";
import { provideRouter } from "@angular/router";
import { APP_ROUTES } from "./app.routes";
import { provideStore } from '@ngrx/store';

export const BASE_URL = new InjectionToken<string>('BASE_URL');

export const appConfig: ApplicationConfig = {
  providers: [
    { provide: BASE_URL, useFactory: (): string => document.getElementsByTagName('base')[0].href },
    { provide: APP_ID, useValue: 'ng-cli-universal' },
    provideHttpClient(withInterceptors([authorizeInterceptor])),
    provideExperimentalZonelessChangeDetection(),
    provideRouter(APP_ROUTES),
    provideStore()
],
};