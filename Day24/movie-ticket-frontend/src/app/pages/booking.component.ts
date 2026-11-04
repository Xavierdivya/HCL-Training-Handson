import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BookingService } from '../services/booking.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  template: `
    <section class="form-card wide">
      <p class="eyebrow">RESERVE YOUR SEATS</p><h1>Book tickets</h1>
      <p class="muted">Show ID: {{ showId || 'Not selected' }}</p>
      <p class="notice">Enter seat IDs from your backend seat data. Seat selection UI can be connected once we confirm your Seats API response.</p>
      <form (ngSubmit)="submit()">
        <label>Seat IDs (comma separated)<input name="seatIds" [(ngModel)]="seatIdsText" placeholder="1,2" required></label>
        <label>Customer name<input name="customerName" [(ngModel)]="customerName" required></label>
        <label>Customer email<input type="email" name="customerEmail" [(ngModel)]="customerEmail" required></label>
        <label>Customer phone<input name="customerPhone" [(ngModel)]="customerPhone" required></label>
        <p class="error" *ngIf="error">{{ error }}</p>
        <p class="success" *ngIf="result">Booking submitted. Reference: {{ result.bookingReference || result.bookingId || result.id || 'See API response' }}</p>
        <button class="btn btn-primary btn-full" [disabled]="loading">{{ loading ? 'Submitting…' : 'Confirm booking' }}</button>
      </form>
      <a routerLink="/shows" class="back-link">Back to shows</a>
    </section>
  `
})
export class BookingComponent {
  showId = 0; seatIdsText = ''; customerName = ''; customerEmail = ''; customerPhone = '';
  error = ''; loading = false; result: any = null;
  constructor(route: ActivatedRoute, private bookings: BookingService) {
    this.showId = Number(route.snapshot.queryParamMap.get('showId')) || 0;
  }
  submit(): void {
    const seatIds = this.seatIdsText.split(',').map(v => Number(v.trim())).filter(v => Number.isInteger(v) && v > 0);
    if (!this.showId || !seatIds.length) { this.error = 'Provide a valid Show ID and at least one Seat ID.'; return; }
    this.error = ''; this.loading = true; this.result = null;
    this.bookings.createBooking({ showId: this.showId, seatIds, customerName: this.customerName, customerEmail: this.customerEmail, customerPhone: this.customerPhone }).subscribe({
      next: value => { this.loading = false; this.result = value; },
      error: err => { this.loading = false; this.error = err?.error?.message ?? 'Booking failed. Verify CreateBookingDto and API response.'; }
    });
  }
}
