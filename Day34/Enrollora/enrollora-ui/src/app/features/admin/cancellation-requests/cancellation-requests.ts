
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CancellationResponse } from '../../student/cancellation-requests/cancellation.models';
import { AdminCancellationService } from './cancellation.service';

@Component({
  selector: 'app-admin-cancellation-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cancellation-requests.html',
  styleUrl: './cancellation-requests.css',
})
export class AdminCancellationRequests implements OnInit {
  private readonly service = inject(AdminCancellationService);

  requests = signal<CancellationResponse[]>([]);
  isLoading = signal(true);
  errorMessage = signal('');
  successMessage = signal('');
  remarks: Record<number, string> = {};
  processingId = signal<number | null>(null);

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.service.getAllRequests().subscribe({
      next: (response) => {
        this.requests.set(response.items);
        this.isLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || error.error?.message ||
          'Unable to load cancellation requests.',
        );
        this.isLoading.set(false);
      },
    });
  }

  review(id: number, approve: boolean): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.processingId.set(id);

    const adminRemarks = this.remarks[id]?.trim() ?? '';
    const action = approve
      ? this.service.approve(id, adminRemarks)
      : this.service.reject(id, adminRemarks);

    action.subscribe({
      next: () => {
        this.successMessage.set(
          approve
            ? 'Cancellation request approved successfully.'
            : 'Cancellation request rejected successfully.',
        );
        this.processingId.set(null);
        this.loadRequests();
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage.set(
          error.error?.detail || error.error?.message ||
          'Unable to review this request.',
        );
        this.processingId.set(null);
      },
    });
  }

  statusLabel(status: number): string {
    return ['Pending', 'Approved', 'Rejected'][status] ?? 'Unknown';
  }
}
