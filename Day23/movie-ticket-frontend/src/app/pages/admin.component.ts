import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Movie } from '../models/api.models';
import { MovieService } from '../services/movie.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <p class="eyebrow">ADMINISTRATION</p><h1>Admin dashboard</h1>
    <p class="notice">Admin-only frontend route. Backend Admin authorization remains mandatory for every management API.</p>
    <section class="admin-form">
      <h2>{{ editingId ? 'Update movie' : 'Add movie' }}</h2>
      <form (ngSubmit)="save()">
        <label>Title<input name="title" [(ngModel)]="movie.title" required></label>
        <label>Description<textarea name="description" [(ngModel)]="movie.description"></textarea></label>
        <div class="two-col">
          <label>Duration (minutes)<input type="number" name="duration" [(ngModel)]="movie.durationMinutes"></label>
          <label>Release date<input type="date" name="releaseDate" [(ngModel)]="movie.releaseDate"></label>
        </div>
        <div class="two-col">
          <label>Language<input name="language" [(ngModel)]="movie.language"></label>
          <label>Genre<input name="genre" [(ngModel)]="movie.genre"></label>
        </div>
        <p class="error" *ngIf="error">{{ error }}</p><p class="success" *ngIf="message">{{ message }}</p>
        <button class="btn btn-primary">{{ editingId ? 'Save changes' : 'Create movie' }}</button>
        <button type="button" class="btn btn-outline" (click)="reset()">Clear</button>
      </form>
    </section>
  `
})
export class AdminComponent {
  movie: Movie = this.blank(); editingId: number | null = null; error = ''; message = '';
  constructor(private service: MovieService) {}
  save(): void {
    this.error = ''; this.message = '';
    const request = this.editingId ? this.service.updateMovie(this.editingId, this.movie) : this.service.createMovie(this.movie);
    request.subscribe({ next: () => { this.message = 'Movie saved successfully.'; this.reset(); }, error: err => this.error = err?.error?.message ?? 'Save failed. Check Admin token and movie DTO fields.' });
  }
  reset(): void { this.movie = this.blank(); this.editingId = null; }
  private blank(): Movie { return { title: '', description: '', durationMinutes: 120, releaseDate: '', language: 'English', genre: '' }; }
}
