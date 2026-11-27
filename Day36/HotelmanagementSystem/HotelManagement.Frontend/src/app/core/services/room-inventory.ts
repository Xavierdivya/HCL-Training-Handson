import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { RoomAvailability, RoomInventory, SetRoomInventoryRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class RoomInventoryService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/roominventory`;

  checkAvailability(hotelId: number, checkInDate: string, checkOutDate: string, guests: number): Observable<RoomAvailability[]> {
    const params = new HttpParams()
      .set('hotelId', hotelId)
      .set('checkInDate', checkInDate)
      .set('checkOutDate', checkOutDate)
      .set('guests', guests);
    return this.http.get<RoomAvailability[]>(`${this.url}/availability`, { params });
  }

  getInventory(roomTypeId: number, fromDate: string, toDate: string): Observable<RoomInventory[]> {
    const params = new HttpParams().set('fromDate', fromDate).set('toDate', toDate);
    return this.http.get<RoomInventory[]>(`${this.url}/roomtype/${roomTypeId}`, { params });
  }

  setInventory(roomTypeId: number, request: SetRoomInventoryRequest): Observable<void> {
    return this.http.put<void>(`${this.url}/roomtype/${roomTypeId}`, request);
  }
}
