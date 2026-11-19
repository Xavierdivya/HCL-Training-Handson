import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html'
})
export class RegisterComponent {

  isSubmitting = signal(false);
  errorMessage = signal('');
  showPassword = signal(false);

  registerForm;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.nonNullable.group({
      fullName: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100)
      ]],
      email: ['', [
        Validators.required,
        Validators.email,
        Validators.maxLength(150)
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(8)
      ]]
    });
  }

  togglePassword(): void {
    this.showPassword.update(value => !value);
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    this.authService.register(this.registerForm.getRawValue()).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.router.navigate(['/categories']);
      },
      error: error => {
        this.isSubmitting.set(false);
        this.errorMessage.set(
          error.error?.message ||
          'Registration failed. Please try again.'
        );
      }
    });
  }
}