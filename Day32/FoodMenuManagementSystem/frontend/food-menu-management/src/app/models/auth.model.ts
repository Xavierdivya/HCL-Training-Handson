export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  userId: number;
  fullName: string;
  email: string;
  role: string;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}