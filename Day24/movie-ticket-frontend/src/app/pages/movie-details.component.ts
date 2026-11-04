import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Movie } from '../models/api.models';
import { MovieService } from '../services/movie.service';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <a routerLink="/movies" class="back-link">← All movies</a>
    <p class="error" *ngIf="error">{{ error }}</p>
    <section class="detail-card" *ngIf="movie">
      <div class="detail-poster">{{ movie.genre || 'MOVIE' }}</div>
      <div><p class="eyebrow">MOVIE DETAILS</p><h1>{{ movie.title }}</h1><p>{{ movie.description }}</p>
      <p class="muted">{{ movie.language }} <span *ngIf="movie.durationMinutes">· {{ movie.durationMinutes }} minutes</span></p>
      <a class="btn btn-primary" routerLink="/shows">Browse shows</a></div>
    </section>
  `
})
export class MovieDetailsComponent implements OnInit {
  movie?: Movie; error = '';
  constructor(private route: ActivatedRoute, private service: MovieService) {}
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.service.getMovie(id).subscribe({ next: value => this.movie = value, error: () => this.error = 'Unable to load movie details.' });
  }
}
