import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Booking, CreateBookingRequest } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private readonly url = `${environment.apiBaseUrl}/Bookings`;
  constructor(private http: HttpClient) {}
  createBooking(data: CreateBookingRequest): Observable<Booking> { return this.http.post<Booking>(this.url, data); }
  getBooking(id: number): Observable<Booking> { return this.http.get<Booking>(`${this.url}/${id}`); }
  cancelBooking(id: number): Observable<unknown> { return this.http.put(`${this.url}/${id}/cancel`, {}); }
}
