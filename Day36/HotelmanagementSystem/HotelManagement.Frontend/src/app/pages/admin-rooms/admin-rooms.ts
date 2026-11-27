import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Amenity, Hotel, RoomInventory, RoomType, RoomTypeRequest } from '../../core/models/models';
import { AmenityService } from '../../core/services/amenity';
import { HotelService } from '../../core/services/hotel';
import { RoomInventoryService } from '../../core/services/room-inventory';
import { RoomTypeService } from '../../core/services/room-type';
import { daysFromToday, errorMessage } from '../../core/utils/helpers';

interface RoomForm {
  name: string;
  description: string;
  pricePerNight: number;
  capacity: number;
  totalRooms: number;
  isActive: boolean;
  amenityIds: number[];
}

const emptyRoom = (): RoomForm => ({
  name: '',
  description: '',
  pricePerNight: 0,
  capacity: 2,
  totalRooms: 1,
  isActive: true,
  amenityIds: [],
});

@Component({
  selector: 'app-admin-rooms',
  imports: [CurrencyPipe, DatePipe, FormsModule, RouterLink],
  templateUrl: './admin-rooms.html',
  styleUrl: './admin-rooms.css',
})
export class AdminRooms implements OnInit {
  private route = inject(ActivatedRoute);
  private hotelService = inject(HotelService);
  private roomTypeService = inject(RoomTypeService);
  private amenityService = inject(AmenityService);
  private inventoryService = inject(RoomInventoryService);

  hotelId = 0;
  hotel = signal<Hotel | null>(null);
  roomTypes = signal<RoomType[]>([]);
  amenities = signal<Amenity[]>([]);
  inventory = signal<RoomInventory[]>([]);
  loading = signal(true);
  error = signal('');
  success = signal('');

  // room type form
  showForm = false;
  editingId: number | null = null;
  form: RoomForm = emptyRoom();

  // inventory panel
  inventoryRoomId: number | null = null;
  fromDate = daysFromToday(0);
  toDate = daysFromToday(14);
  setDate = daysFromToday(1);
  setTotal = 1;
  setAvailable = 1;

  ngOnInit(): void {
    this.hotelId = Number(this.route.snapshot.paramMap.get('id'));

    forkJoin({
      hotel: this.hotelService.getById(this.hotelId),
      amenities: this.amenityService.getAll(),
    }).subscribe({
      next: (r) => {
        this.hotel.set(r.hotel);
        this.amenities.set(r.amenities);
        this.loadRooms();
      },
      error: (err) => {
        this.error.set(errorMessage(err, 'Hotel not found.'));
        this.loading.set(false);
      },
    });
  }

  loadRooms(): void {
    this.roomTypeService.getByHotel(this.hotelId).subscribe({
      next: (list) => {
        this.roomTypes.set(list);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(errorMessage(err));
        this.loading.set(false);
      },
    });
  }

  // ----- room types -----
  add(): void {
    this.editingId = null;
    this.form = emptyRoom();
    this.showForm = true;
    this.success.set('');
  }

  edit(r: RoomType): void {
    this.editingId = r.roomTypeId;
    this.form = {
      name: r.name,
      description: r.description ?? '',
      pricePerNight: r.pricePerNight,
      capacity: r.capacity,
      totalRooms: r.totalRooms,
      isActive: r.isActive,
      amenityIds: this.amenities()
        .filter((a) => r.amenities.includes(a.name))
        .map((a) => a.amenityId),
    };
    this.showForm = true;
    this.success.set('');
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

    const base = {
      name: this.form.name.trim(),
      description: this.form.description.trim() || null,
      pricePerNight: Number(this.form.pricePerNight),
      capacity: Number(this.form.capacity),
      totalRooms: Number(this.form.totalRooms),
      amenityIds: this.form.amenityIds,
    };

    const call =
      this.editingId === null
        ? this.roomTypeService.create({ ...base, hotelId: this.hotelId } as RoomTypeRequest)
        : this.roomTypeService.update(this.editingId, { ...base, isActive: this.form.isActive });

    call.subscribe({
      next: () => {
        this.success.set(this.editingId === null ? 'Room type added.' : 'Room type updated.');
        this.cancel();
        this.loadRooms();
      },
      error: (err) => this.error.set(errorMessage(err)),
    });
  }

  remove(r: RoomType): void {
    if (!confirm(`Delete "${r.name}"?`)) return;
    this.error.set('');
    this.success.set('');

    this.roomTypeService.delete(r.roomTypeId).subscribe({
      next: () => {
        this.success.set('Room type deleted.');
        if (this.inventoryRoomId === r.roomTypeId) {
          this.inventoryRoomId = null;
          this.inventory.set([]);
        }
        this.loadRooms();
      },
      error: (err) => this.error.set(errorMessage(err)),
    });
  }

  // ----- inventory -----
  manageInventory(r: RoomType): void {
    this.inventoryRoomId = r.roomTypeId;
    this.setTotal = r.totalRooms;
    this.setAvailable = r.totalRooms;
    this.loadInventory();
    setTimeout(() => document.getElementById('inventory')?.scrollIntoView({ behavior: 'smooth' }));
  }

  loadInventory(): void {
    if (this.inventoryRoomId === null) return;
    this.inventoryService.getInventory(this.inventoryRoomId, this.fromDate, this.toDate).subscribe({
      next: (list) => this.inventory.set(list),
      error: (err) => this.error.set(errorMessage(err)),
    });
  }

  saveInventory(): void {
    if (this.inventoryRoomId === null) return;
    this.error.set('');
    this.success.set('');

    this.inventoryService
      .setInventory(this.inventoryRoomId, {
        inventoryDate: this.setDate,
        totalRooms: Number(this.setTotal),
        availableRooms: Number(this.setAvailable),
      })
      .subscribe({
        next: () => {
          this.success.set('Inventory saved.');
          this.loadInventory();
        },
        error: (err) => this.error.set(errorMessage(err)),
      });
  }

  inventoryRoomName(): string {
    return this.roomTypes().find((r) => r.roomTypeId === this.inventoryRoomId)?.name ?? '';
  }
}
