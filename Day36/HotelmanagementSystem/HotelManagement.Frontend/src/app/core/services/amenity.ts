import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Amenity, AmenityRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AmenityService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/amenities`;

  getAll(): Observable<Amenity[]> {
    return this.http.get<Amenity[]>(this.url);
  }

  create(request: AmenityRequest): Observable<Amenity> {
    return this.http.post<Amenity>(this.url, request);
  }

  update(id: number, request: AmenityRequest): Observable<Amenity> {
    return this.http.put<Amenity>(`${this.url}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
