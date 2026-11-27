// Mirrors the DTOs of the HotelBooking .NET backend.
// DateOnly values travel as 'yyyy-MM-dd' strings.

export type Role = 'Admin' | 'Customer';

// ----- Auth -----
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  userId: string;
  fullName: string;
  email: string;
  role: Role;
}

export interface CurrentUser {
  userId: string;
  fullName: string;
  email: string;
  role: Role;
}

// ----- Amenities -----
export interface Amenity {
  amenityId: number;
  name: string;
  description?: string | null;
}

export interface AmenityRequest {
  name: string;
  description?: string | null;
}

// ----- Hotels -----
export interface HotelImage {
  hotelImageId: number;
  imageUrl: string;
  isPrimary: boolean;
}

export interface Hotel {
  hotelId: number;
  name: string;
  description: string;
  address: string;
  city: string;
  state: string;
  country: string;
  starRating: number;
  contactNumber: string;
  isActive: boolean;
  images: HotelImage[];
  amenities: string[]; // amenity names
}

export interface HotelSearch {
  searchTerm?: string;
  city?: string;
  starRating?: number | null;
  checkInDate?: string;
  checkOutDate?: string;
  guests?: number | null;
  pageNumber?: number;
  pageSize?: number;
}

export interface HotelRequest {
  name: string;
  description: string;
  address: string;
  city: string;
  state: string;
  country: string;
  starRating: number;
  contactNumber: string;
  isActive?: boolean; // only used on update
  imageUrls: string[];
  amenityIds: number[];
}

// ----- Room types -----
export interface RoomType {
  roomTypeId: number;
  hotelId: number;
  hotelName: string;
  name: string;
  description?: string | null;
  pricePerNight: number;
  capacity: number;
  totalRooms: number;
  isActive: boolean;
  amenities: string[];
}

export interface RoomTypeRequest {
  hotelId?: number; // only used on create
  name: string;
  description?: string | null;
  pricePerNight: number;
  capacity: number;
  totalRooms: number;
  isActive?: boolean; // only used on update
  amenityIds: number[];
}

// ----- Room inventory -----
export interface RoomAvailabilityDate {
  date: string;
  availableRooms: number;
}

export interface RoomAvailability {
  roomTypeId: number;
  roomTypeName: string;
  pricePerNight: number;
  capacity: number;
  availableRooms: number;
  dailyAvailability: RoomAvailabilityDate[];
}

export interface RoomInventory {
  inventoryId: number;
  roomTypeId: number;
  roomTypeName: string;
  inventoryDate: string;
  totalRooms: number;
  availableRooms: number;
}

export interface SetRoomInventoryRequest {
  inventoryDate: string;
  totalRooms: number;
  availableRooms: number;
}

// ----- Bookings -----
export interface BookingItem {
  bookingItemId: number;
  roomTypeId: number;
  roomTypeName: string;
  quantity: number;
  pricePerNight: number;
  subTotal: number;
}

export interface Booking {
  bookingId: number;
  bookingReference: string;
  hotelId: number;
  hotelName: string;
  userId: string;
  customerName: string;
  checkInDate: string;
  checkOutDate: string;
  bookingDate: string;
  totalAmount: number;
  bookingStatus: string;
  items: BookingItem[];
}

export interface CreateBookingRequest {
  hotelId: number;
  checkInDate: string;
  checkOutDate: string;
  items: { roomTypeId: number; quantity: number }[];
}

// The backend enum (HotelBooking.Enums.BookingStatus) was not part of the upload.
// These numeric values assume the usual order - adjust here if your enum differs.
export const BOOKING_STATUSES = [
  { label: 'Pending', value: 0 },
  { label: 'Confirmed', value: 1 },
  { label: 'Cancelled', value: 2 },
  { label: 'Completed', value: 3 },
];
