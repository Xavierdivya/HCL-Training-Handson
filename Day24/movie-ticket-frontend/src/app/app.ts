import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <header class="topbar">
      <a class="brand" routerLink="/">CineBook</a>
      <nav>
        <a routerLink="/movies" routerLinkActive="active">Movies</a>
        <a routerLink="/shows" routerLinkActive="active">Shows</a>
        <a *ngIf="auth.isLoggedIn()" routerLink="/my-bookings" routerLinkActive="active">My bookings</a>
        <a *ngIf="auth.isAdmin()" routerLink="/admin" routerLinkActive="active">Admin</a>
      </nav>
      <div class="account">
        <ng-container *ngIf="auth.isLoggedIn(); else guest">
          <span class="user-label">{{ auth.userEmail() }}</span>
          <button class="btn btn-outline" (click)="auth.logout()">Logout</button>
        </ng-container>
        <ng-template #guest>
          <a routerLink="/login">Login</a>
          <a class="btn btn-primary" routerLink="/register">Sign up</a>
        </ng-template>
      </div>
    </header>
    <main class="page-shell"><router-outlet /></main>
    <footer>Movie Ticket Booking · Day23 Angular Frontend</footer>
  `
})
export class App {
  constructor(public auth: AuthService) {}
}
