import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Movie } from '../models/api.models';
import { MovieService } from '../services/movie.service';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="section-heading"><div><p class="eyebrow">NOW SHOWING</p><h1>Explore movies</h1></div></div>
    <p class="error" *ngIf="error">{{ error }}</p>
    <div class="movie-grid" *ngIf="movies.length; else empty">
      <article class="movie-card" *ngFor="let movie of movies">
        <div class="movie-poster"><span>{{ movie.genre || 'FEATURED' }}</span><b>{{ movie.title }}</b></div>
        <div class="movie-info"><h3>{{ movie.title }}</h3><p>{{ movie.language || 'Language not listed' }} <span *ngIf="movie.durationMinutes">· {{ movie.durationMinutes }} min</span></p><a [routerLink]="['/movies', movie.movieId ?? movie.id]">View details →</a></div>
      </article>
    </div>
    <ng-template #empty><p class="empty-state">No movies returned yet. Confirm that the backend is running and has movie records.</p></ng-template>
  `
})
export class MoviesComponent implements OnInit {
  movies: Movie[] = []; error = '';
  constructor(private service: MovieService) {}
  ngOnInit(): void {
    this.service.getMovies().subscribe({
      next: result => { this.movies = Array.isArray(result) ? result : result.items ?? result.data ?? []; },
      error: err => { this.error = `Could not load movies (${err.status || 'network error'}). Check API URL and CORS.`; }
    });
  }
}
