import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Hotel } from '../../core/models/models';
import { HotelService } from '../../core/services/hotel';
import { daysFromToday, errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-home',
  imports: [FormsModule, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private hotelService = inject(HotelService);

  hotels = signal<Hotel[]>([]);
  loading = signal(true);
  error = signal('');

  search = {
    searchTerm: '',
    city: '',
    starRating: null as number | null,
    checkInDate: daysFromToday(1),
    checkOutDate: daysFromToday(2),
    guests: 2,
  };

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');

    this.hotelService.getAll(this.search).subscribe({
      next: (list) => {
        this.hotels.set(list);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }

  reset(): void {
    this.search.searchTerm = '';
    this.search.city = '';
    this.search.starRating = null;
    this.load();
  }

  primaryImage(hotel: Hotel): string | null {
    const img = hotel.images?.find((i) => i.isPrimary) ?? hotel.images?.[0];
    return img?.imageUrl ?? null;
  }

  stars(n: number): string {
    return '★'.repeat(n);
  }
}
