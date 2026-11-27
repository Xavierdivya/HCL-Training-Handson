import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { Booking } from '../../core/models/models';
import { BookingService } from '../../core/services/booking';
import { errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-my-bookings',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css',
})
export class MyBookings implements OnInit {
  private bookingService = inject(BookingService);
  private route = inject(ActivatedRoute);

  bookings = signal<Booking[]>([]);
  loading = signal(true);
  error = signal('');
  success = signal('');

  ngOnInit(): void {
    if (this.route.snapshot.queryParamMap.get('booked')) {
      this.success.set('Your booking has been placed.');
    }
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.bookingService.getMine().subscribe({
      next: (list) => {
        this.bookings.set([...list].sort((a, b) => b.bookingDate.localeCompare(a.bookingDate)));
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }

  canCancel(b: Booking): boolean {
    return ['pending', 'confirmed'].includes(b.bookingStatus.toLowerCase());
  }

  cancel(b: Booking): void {
    if (!confirm(`Cancel booking ${b.bookingReference}?`)) return;
    this.error.set('');
    this.success.set('');

    this.bookingService.cancel(b.bookingId).subscribe({
      next: () => {
        this.success.set('Booking cancelled.');
        this.load();
      },
      error: (err) => this.error.set(errorMessage(err)),
    });
  }
}
