import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Booking, CreateBookingRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/bookings`;

  // ----- Customer -----
  create(request: CreateBookingRequest): Observable<Booking> {
    return this.http.post<Booking>(this.url, request);
  }

  getMine(): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.url}/my-bookings`);
  }

  cancel(id: number): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}/cancel`, {});
  }

  // ----- Admin -----
  getAll(): Observable<Booking[]> {
    return this.http.get<Booking[]>(this.url);
  }

  getById(id: number): Observable<Booking> {
    return this.http.get<Booking>(`${this.url}/${id}`);
  }

  updateStatus(id: number, status: number): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}/status`, { status });
  }
}
