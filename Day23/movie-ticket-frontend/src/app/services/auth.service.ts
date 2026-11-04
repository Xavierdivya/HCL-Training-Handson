import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'movie_ticket_jwt';
  private readonly profileKey = 'movie_ticket_profile';
  private profile = signal<AuthResponse | null>(this.readProfile());

  constructor(private http: HttpClient, private router: Router) {}

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiBaseUrl}/Auth/login`, data).pipe(
      tap(response => {
        const token = response.token ?? response.accessToken;
        if (!token) throw new Error('Login response did not contain token or accessToken. Update AuthService to match backend response.');
        localStorage.setItem(this.tokenKey, token);
        localStorage.setItem(this.profileKey, JSON.stringify(response));
        this.profile.set(response);
      })
    );
  }

  register(data: RegisterRequest): Observable<unknown> {
    return this.http.post(`${environment.apiBaseUrl}/Auth/register`, data);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.profileKey);
    this.profile.set(null);
    void this.router.navigate(['/login']);
  }

  getToken(): string | null { return localStorage.getItem(this.tokenKey); }
  isLoggedIn(): boolean { return !!this.getToken(); }
  userEmail(): string { return String(this.profile()?.email ?? 'Account'); }

  isAdmin(): boolean {
    const roles = this.profile()?.roles;
    return Array.isArray(roles) && roles.some(role => role.toLowerCase() === 'admin');
  }

  private readProfile(): AuthResponse | null {
    try {
      const value = localStorage.getItem(this.profileKey);
      return value ? JSON.parse(value) as AuthResponse : null;
    } catch { return null; }
  }
}
