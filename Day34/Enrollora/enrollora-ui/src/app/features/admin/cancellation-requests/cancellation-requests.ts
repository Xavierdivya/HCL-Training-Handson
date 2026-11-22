
import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AdminCancellationService } from './cancellation.service';
import {
  CancellationPageResponse,
  CancellationResponse,
} from '../../student/cancellation-requests/cancellation.models';

@Component({
  selector: 'app-admin-cancellation-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cancellation-requests.html',
  styles: [`
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 1rem;
      flex-wrap: wrap;
      margin-bottom: 1.5rem;
    }

    .request-list {
      display: grid;
      gap: 1rem;
    }

    .request-card {
      border: 1px solid var(--border-color, #ddd);
      border-radius: 12px;
      padding: 1.25rem;
      background: var(--card-background, #fff);
    }

    .request-header,
    .request-actions,
    .pagination {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 1rem;
      flex-wrap: wrap;
    }

    .request-details {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
      gap: 1rem;
      margin: 1rem 0;
    }

    .detail-label {
      display: block;
      font-size: .8rem;
      color: #777;
      margin-bottom: .25rem;
    }

    .reason,
    .remarks {
      margin: 1rem 0;
    }

    .remarks textarea {
      display: block;
      width: 100%;
      min-height: 75px;
      box-sizing: border-box;
      margin-top: .5rem;
      padding: .65rem;
      border: 1px solid #ccc;
      border-radius: 8px;
      font: inherit;
    }

    .status {
      display: inline-block;
      padding: .3rem .7rem;
      border-radius: 20px;
      font-size: .8rem;
      font-weight: 600;
    }

    .status.pending {
      background: #fff3cd;
      color: #856404;
    }

    .status.approved {
      background: #d1e7dd;
      color: #0f5132;
    }

    .status.rejected {
      background: #f8d7da;
      color: #842029;
    }

    button {
      cursor: pointer;
      padding: .55rem .9rem;
      border: 1px solid #ccc;
      border-radius: 7px;
      background: #fff;
    }

    button:disabled {
      cursor: not-allowed;
      opacity: .55;
    }

    .approve-button {
      background: #198754;
      color: #fff;
      border-color: #198754;
    }

    .reject-button {
      background: #dc3545;
      color: #fff;
      border-color: #dc3545;
    }

    .error-message {
      color: #b02a37;
      margin: 1rem 0;
    }

    .success-message {
      color: #146c43;
      margin: 1rem 0;
    }

    .empty-state {
      padding: 2rem;
      text-align: center;
      border: 1px dashed #ccc;
      border-radius: 10px;
    }

    .pagination {
      margin-top: 1.5rem;
    }

    .pagination-info {
      color: #777;
      font-size: .9rem;
    }
  `],
})
export class AdminCancellationRequests {
  private readonly cancellationService = inject(AdminCancellationService);

  requests = signal<CancellationResponse[]>([]);
  isLoading = signal(false);
  error = signal('');
  successMessage = signal('');

  processingId = signal<number | null>(null);

  pageNumber = signal(1);
  pageSize = 20;
  totalRecords = signal(0);
  totalPages = signal(0);

  remarksByRequest = signal<Record<number, string>>({});

  constructor() {
    this.loadRequests();
  }

  loadRequests(page = this.pageNumber()): void {
    if (this.isLoading()) {
      return;
    }

    this.isLoading.set(true);
    this.error.set('');
    this.successMessage.set('');

    this.cancellationService
      .getAllRequests(page, this.pageSize)
      .subscribe({
        next: (response: CancellationPageResponse) => {
          // If the last item on a page was reviewed, move to the
          // previous valid page when necessary.
          if (
            response.items.length === 0 &&
            page > 1 &&
            response.totalPages > 0 &&
            page > response.totalPages
          ) {
            this.isLoading.set(false);
            this.loadRequests(response.totalPages);
            return;
          }

          this.requests.set(response.items);
          this.pageNumber.set(response.pageNumber);
          this.totalRecords.set(response.totalRecords);
          this.totalPages.set(response.totalPages);
          this.isLoading.set(false);
        },
        error: (err: unknown) => {
          this.error.set(this.getErrorMessage(err));
          this.isLoading.set(false);
        },
      });
  }

  previousPage(): void {
    if (this.pageNumber() > 1 && !this.isLoading()) {
      this.loadRequests(this.pageNumber() - 1);
    }
  }

  nextPage(): void {
    if (
      this.pageNumber() < this.totalPages() &&
      !this.isLoading()
    ) {
      this.loadRequests(this.pageNumber() + 1);
    }
  }

  setRemarks(requestId: number, value: string): void {
    this.remarksByRequest.update((current) => ({
      ...current,
      [requestId]: value,
    }));
  }

  getRemarks(requestId: number): string {
    return this.remarksByRequest()[requestId] ?? '';
  }

  approveRequest(request: CancellationResponse): void {
    this.reviewRequest(request, 'approve');
  }

  rejectRequest(request: CancellationResponse): void {
    this.reviewRequest(request, 'reject');
  }

  private reviewRequest(
    request: CancellationResponse,
    action: 'approve' | 'reject',
  ): void {
    if (request.status !== 0 || this.processingId() !== null) {
      return;
    }

    const remarks = this.getRemarks(request.cancellationRequestId).trim();

    this.processingId.set(request.cancellationRequestId);
    this.error.set('');
    this.successMessage.set('');

    const review$ =
      action === 'approve'
        ? this.cancellationService.approve(
            request.cancellationRequestId,
            remarks,
          )
        : this.cancellationService.reject(
            request.cancellationRequestId,
            remarks,
          );

    review$.subscribe({
      next: () => {
        this.processingId.set(null);
        this.successMessage.set(
          action === 'approve'
            ? 'Cancellation request approved successfully.'
            : 'Cancellation request rejected successfully.',
        );

        this.loadRequests();
      },
      error: (err: unknown) => {
        this.error.set(this.getErrorMessage(err));
        this.processingId.set(null);
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

  statusClass(status: number): string {
    switch (status) {
      case 0:
        return 'pending';
      case 1:
        return 'approved';
      case 2:
        return 'rejected';
      default:
        return '';
    }
  }

  getErrorMessage(error: unknown): string {
    if (typeof error === 'string') {
      return error;
    }

    if (error && typeof error === 'object') {
      const response = error as {
        error?: {
          message?: string;
          detail?: string;
          title?: string;
        };
        message?: string;
      };

      return (
        response.error?.detail ||
        response.error?.message ||
        response.error?.title ||
        response.message ||
        'Unable to process the cancellation request. Please try again.'
      );
    }

    return 'Unable to process the cancellation request. Please try again.';
  }
}
