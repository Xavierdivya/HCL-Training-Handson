export interface AuthResponse {
  token?: string;
  accessToken?: string;
  email?: string;
  fullName?: string;
  roles?: string[];
  [key: string]: unknown;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface Movie {
  movieId?: number;
  id?: number;
  title: string;
  description?: string;
  durationMinutes?: number;
  releaseDate?: string;
  language?: string;
  genre?: string;
}

export interface Show {
  showId?: number;
  id?: number;
  movieId?: number;
  theatreId?: number;
  screenId?: number;
  showDateTime?: string;
  showTime?: string;
  [key: string]: unknown;
}

export interface Booking {
  bookingId?: number;
  id?: number;
  bookingReference?: string;
  bookingStatus?: string;
  status?: string;
  totalAmount?: number;
  customerName?: string;
  showId?: number;
  [key: string]: unknown;
}

export interface CreateBookingRequest {
  showId: number;
  seatIds: number[];
  customerName: string;
  customerEmail: string;
  customerPhone: string;
}
