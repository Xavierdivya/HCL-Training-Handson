import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth';
import { errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private auth = inject(AuthService);
  private router = inject(Router);

  form = { fullName: '', email: '', password: '', confirmPassword: '' };
  loading = signal(false);
  error = signal('');

  get passwordsMatch(): boolean {
    return this.form.password === this.form.confirmPassword;
  }

  submit(): void {
    if (!this.passwordsMatch) {
      this.error.set('Passwords do not match.');
      return;
    }
    this.error.set('');
    this.loading.set(true);

    this.auth.register(this.form).subscribe({
      next: (res) => this.router.navigateByUrl(this.auth.homeFor(res.role)),
      error: (err) => {
        this.loading.set(false);
        this.error.set(errorMessage(err));
      },
    });
  }
}
