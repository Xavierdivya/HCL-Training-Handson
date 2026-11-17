
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

  private readonly apiUrl = 'http://localhost:5000/api/Auth';
  private readonly storageKey = 'foodMenuAuth';

  constructor(private http: HttpClient) {}

  // REGISTER
  register(data: RegisterRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(
      `${this.apiUrl}/register`,
      data
    ).pipe(
      tap(response => this.saveAuth(response.data))
    );
  }

  // LOGIN
  login(data: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(
      `${this.apiUrl}/login`,
      data
    ).pipe(
      tap(response => this.saveAuth(response.data))
    );
  }

  // SAVE AUTHENTICATION DETAILS
  private saveAuth(data: AuthResponse): void {
    sessionStorage.setItem(this.storageKey, JSON.stringify(data));
  }

  // GET CURRENT USER
  getCurrentUser(): AuthResponse | null {
    const userData = sessionStorage.getItem(this.storageKey);

    return userData ? JSON.parse(userData) as AuthResponse : null;
  }

  // GET TOKEN
  getToken(): string | null {
    return this.getCurrentUser()?.token ?? null;
  }

  // CHECK LOGIN STATUS
  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  // GET USER ROLE
  getUserRole(): string | null {
    return this.getCurrentUser()?.role ?? null;
  }

  // LOGOUT
  logout(): void {
    sessionStorage.removeItem(this.storageKey);
  }
}
