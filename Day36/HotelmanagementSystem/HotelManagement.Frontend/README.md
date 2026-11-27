# Hotel Management - Angular Frontend

Angular 20 (standalone components, signals) frontend for the **HotelBooking** .NET API.

## Run it

```bash
npm install
npm start          # http://localhost:4200
```

Start the backend first (it listens on `http://localhost:5041`). `proxy.conf.json` forwards
every `/api/...` call from the Angular dev server to the backend, so you do not need to set up CORS during development.

For a deployed build, set the full API address in `src/environments/environment.ts`
and enable CORS for your frontend origin in the backend's `Program.cs`.

## Project structure

```
src/app
├── core
│   ├── guards          auth.guard.ts, role.guard.ts
│   ├── interceptors    auth.interceptor.ts   (adds JWT, signs out on 401)
│   ├── models          models.ts             (interfaces for every DTO)
│   ├── services        auth, hotel, room-type, room-inventory, booking, amenity
│   └── utils           helpers.ts            (dates + error messages)
├── pages
│   ├── home                 search + hotel list
│   ├── hotel-details        photos, availability check, booking
│   ├── login / register
│   ├── my-bookings          customer: view and cancel bookings
│   ├── admin-dashboard      totals and recent bookings
│   ├── admin-hotels         add / edit / delete hotels
│   ├── admin-rooms          room types + day-by-day inventory
│   ├── admin-bookings       view and change booking status
│   └── admin-amenities      add / edit / delete amenities
└── shared
    └── navbar
```

## Routes

| Path | Who | Page |
|------|-----|------|
| `/` | everyone | Hotel search |
| `/hotels/:id` | everyone | Hotel details and booking |
| `/login`, `/register` | everyone | Auth |
| `/my-bookings` | Customer | My bookings |
| `/admin` | Admin | Dashboard |
| `/admin/hotels` | Admin | Manage hotels |
| `/admin/hotels/:id/rooms` | Admin | Room types and inventory |
| `/admin/bookings` | Admin | All bookings |
| `/admin/amenities` | Admin | Amenities |

## Things to check against your backend

- **BookingStatus enum**: it was not in the upload, so `BOOKING_STATUSES` in `core/models/models.ts`
  assumes `Pending=0, Confirmed=1, Cancelled=2, Completed=3`. Change it if your enum differs.
- **Hotel list response**: `HotelService.getAll` accepts either a plain array or a paged object
  (`items` / `data` / `results`).
- **Currency**: prices display in INR. Change the currency code in the page templates if needed.
- **Admin account**: registration creates a Customer. Make an Admin by seeding one or changing a role in your database.
