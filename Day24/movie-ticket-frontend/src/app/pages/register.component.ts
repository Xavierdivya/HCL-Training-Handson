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
      <p class="eyebrow">JOIN CINEBOOK</p><h1>Create account</h1>
      <form (ngSubmit)="submit()">
        <label>Full name<input name="fullName" [(ngModel)]="fullName" required maxlength="100"></label>
        <label>Email<input type="email" name="email" [(ngModel)]="email" required email></label>
        <label>Password<input type="password" name="password" [(ngModel)]="password" required minlength="8"></label>
        <p class="error" *ngIf="error">{{ error }}</p><p class="success" *ngIf="success">{{ success }}</p>
        <button class="btn btn-primary btn-full" [disabled]="loading">{{ loading ? 'Creating…' : 'Register' }}</button>
      </form>
      <p class="muted">Already registered? <a routerLink="/login">Login</a></p>
    </section>
  `
})
export class RegisterComponent {
  fullName = ''; email = ''; password = ''; error = ''; success = ''; loading = false;
  constructor(private auth: AuthService, private router: Router) {}
  submit(): void {
    this.error = ''; this.success = ''; this.loading = true;
    this.auth.register({ fullName: this.fullName, email: this.email, password: this.password }).subscribe({
      next: () => { this.loading = false; this.success = 'Registration successful. You can now log in.'; void this.router.navigate(['/login']); },
      error: err => { this.loading = false; this.error = err?.error?.message ?? err?.error ?? 'Registration failed. Check the backend response.'; }
    });
  }
}
