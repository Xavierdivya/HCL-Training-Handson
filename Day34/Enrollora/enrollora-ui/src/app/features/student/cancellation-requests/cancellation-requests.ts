
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { EnrollmentResponse } from '../../enrollments/enrollment.models';
import { EnrollmentService } from '../../enrollments/enrollment.service';

import {
  CancellationResponse,
  CreateCancellationRequest,
} from './cancellation.models';
import { CancellationService } from './cancellation.service';

@Component({
  selector: 'app-cancellation-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cancellation-requests.html',
  styleUrl: './cancellation-requests.css',
})
export class CancellationRequests implements OnInit {
  private readonly enrollmentService = inject(EnrollmentService);
  private readonly cancellationService = inject(CancellationService);

  activeEnrollments = signal<EnrollmentResponse[]>([]);
  requests = signal<CancellationResponse[]>([]);

  // Only enrollments without an existing Pending request can be selected.
  availableEnrollments = computed(() => {
    const pendingEnrollmentIds = new Set(
      this.requests()
        .filter((request) => request.status === 0)
        .map((request) => request.enrollmentId),
    );

    return this.activeEnrollments().filter(
      (enrollment) => !pendingEnrollmentIds.has(enrollment.enrollmentId),
    );
  });

  selectedEnrollmentId: number | null = null;
  reason = '';

  isLoading = signal(true);
  isSubmitting = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    let completed = 0;

    const finishLoading = (): void => {
      completed++;

      if (completed === 2) {
        this.isLoading.set(false);

        // Clear a selection if it is no longer eligible.
        if (
          this.selectedEnrollmentId !== null &&
          !this.availableEnrollments().some(
            (enrollment) =>
              enrollment.enrollmentId === this.selectedEnrollmentId,
          )
        ) {
          this.selectedEnrollmentId = null;
        }
      }
    };

    this.enrollmentService.getMyEnrollments(1, 100, { status: 0 }).subscribe({
      next: (response) => {
        this.activeEnrollments.set(response.items);
        finishLoading();
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          this.getErrorMessage(
            error,
            'Unable to load your active enrollments.',
          ),
        );
        finishLoading();
      },
    });

    this.cancellationService.getMyRequests(1, 100).subscribe({
      next: (response) => {
        this.requests.set(response.items);
        finishLoading();
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          this.getErrorMessage(
            error,
            'Unable to load cancellation requests.',
          ),
        );
        finishLoading();
      },
    });
  }

  submitRequest(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.isSubmitting()) {
      return;
    }

    if (!this.selectedEnrollmentId) {
      this.errorMessage.set('Please select an enrollment.');
      return;
    }

    const eligibleEnrollment = this.availableEnrollments().some(
      (enrollment) =>
        enrollment.enrollmentId === this.selectedEnrollmentId,
    );

    if (!eligibleEnrollment) {
      this.errorMessage.set(
        'This enrollment already has a pending cancellation request. Refresh your data and try again.',
      );
      return;
    }

    if (!this.reason.trim()) {
      this.errorMessage.set('Please enter a cancellation reason.');
      return;
    }

    const request: CreateCancellationRequest = {
      reason: this.reason.trim(),
    };

    this.isSubmitting.set(true);

    this.cancellationService
      .createRequest(this.selectedEnrollmentId, request)
      .subscribe({
        next: () => {
          this.successMessage.set(
            'Cancellation request submitted successfully.',
          );

          this.selectedEnrollmentId = null;
          this.reason = '';
          this.isSubmitting.set(false);

          this.loadData();
        },
        error: (error: HttpErrorResponse) => {
          this.errorMessage.set(
            this.getErrorMessage(
              error,
              'Unable to submit cancellation request.',
            ),
          );

          this.isSubmitting.set(false);
        },
      });
  }

  statusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Pending';
      case 1:
        return 'Approved';
      case 2:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallback: string,
  ): string {
    const body = error.error;

    if (typeof body === 'string' && body.trim()) {
      return body;
    }

    return body?.detail || body?.message || body?.title || fallback;
  }
}
