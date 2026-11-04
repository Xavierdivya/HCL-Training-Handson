import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Show } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class ShowService {
  private readonly url = `${environment.apiBaseUrl}/Shows`;
  constructor(private http: HttpClient) {}
  getShows(): Observable<Show[] | { items?: Show[]; data?: Show[] }> { return this.http.get<Show[] | { items?: Show[]; data?: Show[] }>(this.url); }
}
