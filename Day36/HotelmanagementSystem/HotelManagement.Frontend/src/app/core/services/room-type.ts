import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { RoomType, RoomTypeRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class RoomTypeService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/roomtypes`;

  getByHotel(hotelId: number): Observable<RoomType[]> {
    return this.http.get<RoomType[]>(`${this.url}/hotel/${hotelId}`);
  }

  getById(id: number): Observable<RoomType> {
    return this.http.get<RoomType>(`${this.url}/${id}`);
  }

  create(request: RoomTypeRequest): Observable<RoomType> {
    return this.http.post<RoomType>(this.url, request);
  }

  update(id: number, request: RoomTypeRequest): Observable<RoomType> {
    return this.http.put<RoomType>(`${this.url}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
