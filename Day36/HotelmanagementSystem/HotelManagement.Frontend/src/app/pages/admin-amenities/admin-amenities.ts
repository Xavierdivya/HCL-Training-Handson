import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Amenity } from '../../core/models/models';
import { AmenityService } from '../../core/services/amenity';
import { errorMessage } from '../../core/utils/helpers';

@Component({
  selector: 'app-admin-amenities',
  imports: [FormsModule],
  templateUrl: './admin-amenities.html',
  styleUrl: './admin-amenities.css',
})
export class AdminAmenities implements OnInit {
  private amenityService = inject(AmenityService);

  amenities = signal<Amenity[]>([]);
  loading = signal(true);
  error = signal('');
  success = signal('');

  editingId: number | null = null;
  form = { name: '', description: '' };

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.amenityService.getAll().subscribe({
      next: (list) => {
        this.amenities.set(list);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }

  edit(a: Amenity): void {
    this.editingId = a.amenityId;
    this.form = { name: a.name, description: a.description ?? '' };
    this.success.set('');
  }

  reset(): void {
    this.editingId = null;
    this.form = { name: '', description: '' };
  }

  save(): void {
    this.error.set('');
    this.success.set('');

    const request = { name: this.form.name.trim(), description: this.form.description.trim() || null };
    const call =
      this.editingId === null
        ? this.amenityService.create(request)
        : this.amenityService.update(this.editingId, request);

    call.subscribe({
      next: () => {
        this.success.set(this.editingId === null ? 'Amenity added.' : 'Amenity updated.');
        this.reset();
        this.load();
      },
      error: (err) => this.error.set(errorMessage(err)),
    });
  }

  remove(a: Amenity): void {
    if (!confirm(`Delete "${a.name}"?`)) return;
    this.error.set('');
    this.success.set('');

    this.amenityService.delete(a.amenityId).subscribe({
      next: () => {
        this.success.set('Amenity deleted.');
        this.load();
      },
      error: (err) => this.error.set(errorMessage(err)),
    });
  }
}
