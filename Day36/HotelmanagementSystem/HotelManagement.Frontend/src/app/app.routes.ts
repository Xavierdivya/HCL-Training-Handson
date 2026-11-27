import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

import { Home } from './pages/home/home';
import { HotelDetails } from './pages/hotel-details/hotel-details';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { MyBookings } from './pages/my-bookings/my-bookings';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { AdminHotels } from './pages/admin-hotels/admin-hotels';
import { AdminRooms } from './pages/admin-rooms/admin-rooms';
import { AdminBookings } from './pages/admin-bookings/admin-bookings';
import { AdminAmenities } from './pages/admin-amenities/admin-amenities';

export const routes: Routes = [
  // Public
  { path: '', component: Home },
  { path: 'hotels/:id', component: HotelDetails },
  { path: 'login', component: Login },
  { path: 'register', component: Register },

  // Customer
  {
    path: 'my-bookings',
    component: MyBookings,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Customer'] },
  },

  // Admin
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { roles: ['Admin'] },
    children: [
      { path: '', component: AdminDashboard },
      { path: 'hotels', component: AdminHotels },
      { path: 'hotels/:id/rooms', component: AdminRooms },
      { path: 'bookings', component: AdminBookings },
      { path: 'amenities', component: AdminAmenities },
    ],
  },

  { path: '**', redirectTo: '' },
];
