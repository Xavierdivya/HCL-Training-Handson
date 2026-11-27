import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Hotel, HotelRequest, HotelSearch } from '../models/models';

@Injectable({ providedIn: 'root' })
export class HotelService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/hotels`;

  getAll(search: HotelSearch = {}): Observable<Hotel[]> {
    let params = new HttpParams();
    for (const [key, value] of Object.entries({ pageNumber: 1, pageSize: 50, ...search })) {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    }
    // The list may come back as a plain array or wrapped in a paged object - handle both.
    return this.http
      .get<any>(this.url, { params })
      .pipe(map((r) => (Array.isArray(r) ? r : (r?.items ?? r?.data ?? r?.results ?? []))));
  }

  getById(id: number): Observable<Hotel> {
    return this.http.get<Hotel>(`${this.url}/${id}`);
  }

  create(request: HotelRequest): Observable<Hotel> {
    return this.http.post<Hotel>(this.url, request);
  }

  update(id: number, request: HotelRequest): Observable<Hotel> {
    return this.http.put<Hotel>(`${this.url}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }

  updateAmenities(id: number, amenityIds: number[]): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}/amenities`, { amenityIds });
  }
}
