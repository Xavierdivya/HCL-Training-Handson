import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { BOOKING_STATUSES, Booking } from '../../core/models/models';
import { BookingService } from '../../core/services/booking';
import { errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-admin-bookings',
  imports: [CurrencyPipe, DatePipe, FormsModule],
  templateUrl: './admin-bookings.html',
  styleUrl: './admin-bookings.css',
})
export class AdminBookings implements OnInit {
  private bookingService = inject(BookingService);

  statuses = BOOKING_STATUSES;
  bookings = signal<Booking[]>([]);
  filter = signal('');
  loading = signal(true);
  error = signal('');
  success = signal('');
  expandedId: number | null = null;

  visible = computed(() => {
    const f = this.filter();
    const sorted = [...this.bookings()].sort((a, b) => b.bookingDate.localeCompare(a.bookingDate));
    return f ? sorted.filter((b) => b.bookingStatus === f) : sorted;
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.bookingService.getAll().subscribe({
      next: (list) => {
        this.bookings.set(list);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }

  toggle(b: Booking): void {
    this.expandedId = this.expandedId === b.bookingId ? null : b.bookingId;
  }

  changeStatus(b: Booking, value: string): void {
    this.error.set('');
    this.success.set('');

    this.bookingService.updateStatus(b.bookingId, Number(value)).subscribe({
      next: () => {
        this.success.set(`Booking ${b.bookingReference} updated.`);
        this.load();
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.load();
      },
    });
  }

  /** Dropdown value for a booking's current status. */
  statusValue(b: Booking): number {
    return this.statuses.find((s) => s.label.toLowerCase() === b.bookingStatus.toLowerCase())?.value ?? 0;
  }
}
