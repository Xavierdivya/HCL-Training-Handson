import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Booking, Hotel } from '../../core/models/models';
import { AmenityService } from '../../core/services/amenity';
import { BookingService } from '../../core/services/booking';
import { HotelService } from '../../core/services/hotel';
import { errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard implements OnInit {
  private hotelService = inject(HotelService);
  private bookingService = inject(BookingService);
  private amenityService = inject(AmenityService);

  hotels = signal<Hotel[]>([]);
  bookings = signal<Booking[]>([]);
  amenityCount = signal(0);
  loading = signal(true);
  error = signal('');

  pending = computed(() => this.bookings().filter((b) => b.bookingStatus.toLowerCase() === 'pending').length);
  revenue = computed(() =>
    this.bookings()
      .filter((b) => b.bookingStatus.toLowerCase() !== 'cancelled')
      .reduce((sum, b) => sum + b.totalAmount, 0),
  );
  recent = computed(() => [...this.bookings()].sort((a, b) => b.bookingDate.localeCompare(a.bookingDate)).slice(0, 5));

  ngOnInit(): void {
    forkJoin({
      hotels: this.hotelService.getAll({ pageSize: 100 }),
      bookings: this.bookingService.getAll(),
      amenities: this.amenityService.getAll(),
    }).subscribe({
      next: (r) => {
        this.hotels.set(r.hotels);
        this.bookings.set(r.bookings);
        this.amenityCount.set(r.amenities.length);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }
}
