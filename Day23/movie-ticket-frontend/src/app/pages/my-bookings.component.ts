import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BookingService } from '../services/booking.service';
import { Booking } from '../models/api.models';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <p class="eyebrow">YOUR TICKETS</p><h1>Find a booking</h1>
    <section class="lookup-card">
      <label>Booking ID<input type="number" name="bookingId" [(ngModel)]="bookingId"></label>
      <button class="btn btn-primary" (click)="lookup()">Find booking</button>
    </section>
    <p class="error" *ngIf="error">{{ error }}</p>
    <article class="booking-result" *ngIf="booking">
      <h2>Booking details</h2>
      <p><b>Reference:</b> {{ booking.bookingReference || '—' }}</p>
      <p><b>Status:</b> {{ booking.bookingStatus || booking.status || '—' }}</p>
      <p><b>Total:</b> {{ booking.totalAmount ?? '—' }}</p>
      <button class="btn btn-outline" *ngIf="booking.bookingId || booking.id" (click)="cancel()">Cancel booking</button>
    </article>
  `
})
export class MyBookingsComponent {
  bookingId: number | null = null; booking?: Booking; error = '';
  constructor(private service: BookingService) {}
  lookup(): void {
    if (!this.bookingId) { this.error = 'Enter a booking ID.'; return; }
    this.error = ''; this.booking = undefined;
    this.service.getBooking(this.bookingId).subscribe({ next: value => this.booking = value, error: () => this.error = 'Booking not found or API request failed.' });
  }
  cancel(): void {
    const id = this.booking?.bookingId ?? this.booking?.id;
    if (!id) return;
    this.service.cancelBooking(id).subscribe({ next: () => { this.error = 'Cancellation request completed.'; this.lookup(); }, error: () => this.error = 'Unable to cancel booking.' });
  }
}
