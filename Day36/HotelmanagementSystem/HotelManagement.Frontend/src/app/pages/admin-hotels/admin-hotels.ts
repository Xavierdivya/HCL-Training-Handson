import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Amenity, Hotel, HotelRequest } from '../../core/models/models';
import { AmenityService } from '../../core/services/amenity';
import { HotelService } from '../../core/services/hotel';
import { errorMessage } from '../../core/utils/helpers';

interface HotelForm {
  name: string;
  description: string;
  address: string;
  city: string;
  state: string;
  country: string;
  starRating: number;
  contactNumber: string;
  isActive: boolean;
  imageUrls: string; // one url per line
  amenityIds: number[];
}

const emptyForm = (): HotelForm => ({
  name: '',
  description: '',
  address: '',
  city: '',
  state: '',
  country: '',
  starRating: 3,
  contactNumber: '',
  isActive: true,
  imageUrls: '',
  amenityIds: [],
});

@Component({
  selector: 'app-admin-hotels',
  imports: [FormsModule, RouterLink],
  templateUrl: './admin-hotels.html',
  styleUrl: './admin-hotels.css',
})
export class AdminHotels implements OnInit {
  private hotelService = inject(HotelService);
  private amenityService = inject(AmenityService);

  hotels = signal<Hotel[]>([]);
  amenities = signal<Amenity[]>([]);
  loading = signal(true);
  saving = signal(false);
  error = signal('');
  success = signal('');

  showForm = false;
  editingId: number | null = null;
  form: HotelForm = emptyForm();

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    forkJoin({
      hotels: this.hotelService.getAll({ pageSize: 100 }),
      amenities: this.amenityService.getAll(),
    }).subscribe({
      next: (r) => {
        this.hotels.set(r.hotels);
        this.amenities.set(r.amenities);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }

  add(): void {
    this.editingId = null;
    this.form = emptyForm();
    this.showForm = true;
    this.success.set('');
  }

  edit(h: Hotel): void {
    this.editingId = h.hotelId;
    this.form = {
      name: h.name,
      description: h.description,
      address: h.address,
      city: h.city,
      state: h.state,
      country: h.country,
      starRating: h.starRating,
      contactNumber: h.contactNumber,
      isActive: h.isActive,
      imageUrls: h.images.map((i) => i.imageUrl).join('\n'),
      // the API returns amenity names, the update call needs ids
      amenityIds: this.amenities()
        .filter((a) => h.amenities.includes(a.name))
        .map((a) => a.amenityId),
    };
    this.showForm = true;
    this.success.set('');
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancel(): void {
    this.showForm = false;
    this.editingId = null;
  }

  toggleAmenity(id: number, checked: boolean): void {
    this.form.amenityIds = checked
      ? [...this.form.amenityIds, id]
      : this.form.amenityIds.filter((x) => x !== id);
  }

  save(): void {
    this.error.set('');
    this.success.set('');
    this.saving.set(true);

    const request: HotelRequest = {
      name: this.form.name.trim(),
      description: this.form.description.trim(),
      address: this.form.address.trim(),
      city: this.form.city.trim(),
      state: this.form.state.trim(),
      country: this.form.country.trim(),
      starRating: Number(this.form.starRating),
      contactNumber: this.form.contactNumber.trim(),
      imageUrls: this.form.imageUrls.split('\n').map((u) => u.trim()).filter(Boolean),
      amenityIds: this.form.amenityIds,
    };

    const call =
      this.editingId === null
        ? this.hotelService.create(request)
        : this.hotelService.update(this.editingId, { ...request, isActive: this.form.isActive });

    call.subscribe({
      next: () => {
        this.success.set(this.editingId === null ? 'Hotel added.' : 'Hotel updated.');
        this.saving.set(false);
        this.cancel();
        this.load();
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.saving.set(false);
      },
    });
  }

  remove(h: Hotel): void {
    if (!confirm(`Delete "${h.name}"? This cannot be undone.`)) return;
    this.error.set('');
    this.success.set('');

    this.hotelService.delete(h.hotelId).subscribe({
      next: () => {
        this.success.set('Hotel deleted.');
        this.load();
      },
      error: (err) => this.error.set(errorMessage(err)),
    });
  }
}
