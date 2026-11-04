import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <section class="form-card">
      <p class="eyebrow">WELCOME BACK</p><h1>Sign in</h1>
      <form (ngSubmit)="submit()">
        <label>Email<input type="email" name="email" [(ngModel)]="email" required email></label>
        <label>Password<input type="password" name="password" [(ngModel)]="password" required></label>
        <p class="error" *ngIf="error">{{ error }}</p>
        <button class="btn btn-primary btn-full" [disabled]="loading">{{ loading ? 'Signing in…' : 'Login' }}</button>
      </form>
      <p class="muted">New here? <a routerLink="/register">Create an account</a></p>
    </section>
  `
})
export class LoginComponent {
  email = ''; password = ''; error = ''; loading = false;
  constructor(private auth: AuthService, private router: Router) {}
  submit(): void {
    this.error = ''; this.loading = true;
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: () => { this.loading = false; void this.router.navigate(['/movies']); },
      error: err => { this.loading = false; this.error = err?.error?.message ?? err?.error ?? err?.message ?? 'Login failed. Check your details and backend API.'; }
    });
  }
}
