import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import {
  RegisterRequest,
  LoginRequest,
  AuthResponse,
  ApiResponse
} from '../../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiUrl = 'http://localhost:5186/api/Auth';
  private readonly storageKey = 'foodMenuAuth';

  constructor(private http: HttpClient) {}

  register(data: RegisterRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(
      `${this.apiUrl}/register`,
      data
    ).pipe(
      tap(response => this.saveAuth(response.data))
    );
  }

  login(data: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(
      `${this.apiUrl}/login`,
      data
    ).pipe(
      tap(response => this.saveAuth(response.data))
    );
  }

  private saveAuth(data: AuthResponse): void {
    sessionStorage.setItem(this.storageKey, JSON.stringify(data));
  }

  getCurrentUser(): AuthResponse | null {
    const userData = sessionStorage.getItem(this.storageKey);

    if (!userData) {
      return null;
    }

    try {
      return JSON.parse(userData) as AuthResponse;
    } catch {
      this.logout();
      return null;
    }
  }

  getToken(): string | null {
    return this.getCurrentUser()?.token ?? null;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  getUserRole(): string | null {
    return this.getCurrentUser()?.role ?? null;
  }

  logout(): void {
    sessionStorage.removeItem(this.storageKey);
  }
}