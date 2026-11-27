import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthResponse, CurrentUser, LoginRequest, RegisterRequest, Role } from '../models/models';

const TOKEN_KEY = 'hotel_token';
const USER_KEY = 'hotel_user';
const EXPIRES_KEY = 'hotel_expires';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private url = `${environment.apiUrl}/auth`;

  private userSignal = signal<CurrentUser | null>(this.readUser());

  readonly user = this.userSignal.asReadonly();
  readonly isAdmin = computed(() => this.userSignal()?.role === 'Admin');
  readonly isCustomer = computed(() => this.userSignal()?.role === 'Customer');

  /** Method (not computed) so an expired token is noticed even without a state change. */
  isLoggedIn(): boolean {
    return !!this.userSignal() && !this.isTokenExpired();
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.url}/login`, request).pipe(tap((r) => this.saveSession(r)));
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.url}/register`, request).pipe(tap((r) => this.saveSession(r)));
  }

  me(): Observable<CurrentUser> {
    return this.http.get<CurrentUser>(`${this.url}/me`);
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    localStorage.removeItem(EXPIRES_KEY);
    this.userSignal.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return this.isTokenExpired() ? null : localStorage.getItem(TOKEN_KEY);
  }

  hasRole(roles: Role[]): boolean {
    const role = this.userSignal()?.role;
    return !!role && roles.includes(role);
  }

  /** Where each role lands after signing in. */
  homeFor(role: Role | undefined): string {
    return role === 'Admin' ? '/admin' : '/';
  }

  private saveSession(r: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, r.token);
    localStorage.setItem(EXPIRES_KEY, r.expiresAt);
    const user: CurrentUser = { userId: r.userId, fullName: r.fullName, email: r.email, role: r.role };
    localStorage.setItem(USER_KEY, JSON.stringify(user));
    this.userSignal.set(user);
  }

  private readUser(): CurrentUser | null {
    try {
      const raw = localStorage.getItem(USER_KEY);
      return raw ? (JSON.parse(raw) as CurrentUser) : null;
    } catch {
      return null;
    }
  }

  private isTokenExpired(): boolean {
    const expires = localStorage.getItem(EXPIRES_KEY);
    if (!expires) return !localStorage.getItem(TOKEN_KEY);
    return new Date(expires).getTime() <= Date.now();
  }
}
