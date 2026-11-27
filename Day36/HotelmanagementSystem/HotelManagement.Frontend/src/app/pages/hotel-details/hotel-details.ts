import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';

import { Hotel, RoomAvailability } from '../../core/models/models';
import { AuthService } from '../../core/services/auth';
import { BookingService } from '../../core/services/booking';
import { HotelService } from '../../core/services/hotel';
import { RoomInventoryService } from '../../core/services/room-inventory';
import { daysFromToday, errorMessage, nightsBetween } from '../../core/utils/helpers';

@Component({
  selector: 'app-hotel-details',
  imports: [FormsModule, RouterLink, CurrencyPipe],
  templateUrl: './hotel-details.html',
  styleUrl: './hotel-details.css',
})
export class HotelDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private hotelService = inject(HotelService);
  private inventoryService = inject(RoomInventoryService);
  private bookingService = inject(BookingService);
  auth = inject(AuthService);

  hotelId = 0;
  hotel = signal<Hotel | null>(null);
  rooms = signal<RoomAvailability[]>([]);
  loading = signal(true);
  loadingRooms = signal(false);
  booking = signal(false);
  error = signal('');
  roomsError = signal('');

  checkIn = daysFromToday(1);
  checkOut = daysFromToday(2);
  guests = 2;
  quantities: Record<number, number> = {};

  ngOnInit(): void {
    this.hotelId = Number(this.route.snapshot.paramMap.get('id'));
    const q = this.route.snapshot.queryParamMap;
    this.checkIn = q.get('checkIn') || this.checkIn;
    this.checkOut = q.get('checkOut') || this.checkOut;
    this.guests = Number(q.get('guests')) || this.guests;

    this.hotelService.getById(this.hotelId).subscribe({
      next: (h) => {
        this.hotel.set(h);
        this.loading.set(false);
        this.checkAvailability();
      },
      error: (err) => {
        this.error.set(errorMessage(err, 'Hotel not found.'));
        this.loading.set(false);
      },
    });
  }

  get nights(): number {
    return nightsBetween(this.checkIn, this.checkOut);
  }

  get total(): number {
    return this.rooms().reduce((sum, r) => sum + (this.quantities[r.roomTypeId] ?? 0) * r.pricePerNight * this.nights, 0);
  }

  get selectedCount(): number {
    return Object.values(this.quantities).reduce((a, b) => a + b, 0);
  }

  checkAvailability(): void {
    this.roomsError.set('');
    if (this.nights < 1) {
      this.roomsError.set('Check-out must be after check-in.');
      this.rooms.set([]);
      return;
    }
    this.loadingRooms.set(true);

    this.inventoryService.checkAvailability(this.hotelId, this.checkIn, this.checkOut, this.guests).subscribe({
      next: (list) => {
        this.rooms.set(list);
        this.quantities = Object.fromEntries(list.map((r) => [r.roomTypeId, 0]));
        this.loadingRooms.set(false);
      },
      error: (err) => {
        this.roomsError.set(errorMessage(err));
        this.loadingRooms.set(false);
      },
    });
  }

  options(room: RoomAvailability): number[] {
    return Array.from({ length: Math.min(room.availableRooms, 10) + 1 }, (_, i) => i);
  }

  book(): void {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }

    const items = this.rooms()
      .filter((r) => (this.quantities[r.roomTypeId] ?? 0) > 0)
      .map((r) => ({ roomTypeId: r.roomTypeId, quantity: this.quantities[r.roomTypeId] }));

    this.roomsError.set('');
    this.booking.set(true);

    this.bookingService
      .create({ hotelId: this.hotelId, checkInDate: this.checkIn, checkOutDate: this.checkOut, items })
      .subscribe({
        next: () => this.router.navigate(['/my-bookings'], { queryParams: { booked: 1 } }),
        error: (err) => {
          this.roomsError.set(errorMessage(err));
          this.booking.set(false);
        },
      });
  }

  stars(n: number): string {
    return '★'.repeat(n);
  }
}
