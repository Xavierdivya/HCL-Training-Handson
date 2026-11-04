import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Movie } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class MovieService {
  private readonly url = `${environment.apiBaseUrl}/Movies`;
  constructor(private http: HttpClient) {}

  getMovies(pageNumber = 1, pageSize = 20): Observable<Movie[] | { items?: Movie[]; data?: Movie[] }> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<Movie[] | { items?: Movie[]; data?: Movie[] }>(this.url, { params });
  }
  getMovie(id: number): Observable<Movie> { return this.http.get<Movie>(`${this.url}/${id}`); }
  createMovie(movie: Movie): Observable<Movie> { return this.http.post<Movie>(this.url, movie); }
  updateMovie(id: number, movie: Movie): Observable<Movie> { return this.http.put<Movie>(`${this.url}/${id}`, movie); }
  deleteMovie(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}
