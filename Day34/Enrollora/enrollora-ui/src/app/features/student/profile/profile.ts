
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';

import {
  StudentProfile,
  UpdateStudentProfileRequest,
} from './profile.models';

import { ProfileService } from './profile.service';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  private readonly profileService = inject(ProfileService);

  profile = signal<StudentProfile | null>(null);

  isLoading = signal(true);
  isEditing = signal(false);
  isSaving = signal(false);

  successMessage = signal('');
  errorMessage = signal('');

  editForm: UpdateStudentProfileRequest = {
    name: '',
    department: '',
    age: 18,
    cgpa: 0,
  };

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.profileService.getMyProfile().subscribe({
      next: (response) => {
        this.profile.set(response);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          this.getErrorMessage(error, 'Unable to load your profile.'),
        );
        this.isLoading.set(false);
      },
    });
  }

  startEditing(): void {
    const currentProfile = this.profile();

    if (!currentProfile) {
      return;
    }

    this.editForm = {
      name: currentProfile.name,
      department: currentProfile.department,
      age: currentProfile.age,
      cgpa: currentProfile.cgpa,
    };

    this.successMessage.set('');
    this.errorMessage.set('');
    this.isEditing.set(true);
  }

  cancelEditing(): void {
    this.isEditing.set(false);
    this.errorMessage.set('');
  }

  saveProfile(form: NgForm): void {
    if (form.invalid || this.isSaving()) {
      form.control.markAllAsTouched();
      return;
    }

    this.isSaving.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.profileService.updateMyProfile(this.editForm).subscribe({
      next: (response) => {
        this.profile.set(response);
        this.isEditing.set(false);
        this.isSaving.set(false);
        this.successMessage.set('Profile updated successfully.');
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          this.getErrorMessage(error, 'Unable to update your profile.'),
        );
        this.isSaving.set(false);
      },
    });
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallback: string,
  ): string {
    const body = error.error;

    if (typeof body === 'string' && body.trim()) {
      return body;
    }

    if (body?.detail) return body.detail;
    if (body?.message) return body.message;
    if (body?.title) return body.title;

    return fallback;
  }
}
