import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth';
import { errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private auth = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  email = '';
  password = '';
  loading = signal(false);
  error = signal('');

  submit(): void {
    this.error.set('');
    this.loading.set(true);

    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: (res) => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
        this.router.navigateByUrl(returnUrl || this.auth.homeFor(res.role));
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(errorMessage(err, 'Invalid email or password.'));
      },
    });
  }
}
