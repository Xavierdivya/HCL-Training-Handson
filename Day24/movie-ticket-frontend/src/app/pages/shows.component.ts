import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Show } from '../models/api.models';
import { ShowService } from '../services/show.service';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  template: `
    <p class="eyebrow">PICK YOUR SHOW</p><h1>Available shows</h1>
    <p class="error" *ngIf="error">{{ error }}</p>
    <div class="show-list" *ngIf="shows.length; else empty">
      <article class="show-card" *ngFor="let show of shows">
        <div><b>Show #{{ show.showId ?? show.id }}</b><p>Movie ID: {{ show.movieId ?? '—' }} · Screen: {{ show.screenId ?? '—' }}</p><small>{{ show.showDateTime || show.showTime || 'Show time details' }}</small></div>
        <button class="btn btn-primary" (click)="book(show)">Book seats</button>
      </article>
    </div>
    <ng-template #empty><p class="empty-state">No shows found. Check the Shows API route and available show records.</p></ng-template>
  `
})
export class ShowsComponent implements OnInit {
  shows: Show[] = []; error = '';
  constructor(private service: ShowService, private auth: AuthService, private router: Router) {}
  ngOnInit(): void {
    this.service.getShows().subscribe({
      next: result => this.shows = Array.isArray(result) ? result : result.items ?? result.data ?? [],
      error: err => this.error = `Could not load shows (${err.status || 'network error'}). Verify GET /api/Shows.`
    });
  }
  book(show: Show): void {
    if (!this.auth.isLoggedIn()) { void this.router.navigate(['/login']); return; }
    void this.router.navigate(['/booking'], { queryParams: { showId: show.showId ?? show.id } });
  }
}
