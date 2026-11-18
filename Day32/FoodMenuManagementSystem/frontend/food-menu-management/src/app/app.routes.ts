import { Routes } from '@angular/router';

import { CategoryComponent } from './components/category/category';
import { FoodItemComponent } from './components/food-item/food-item/food-item';
import { LoginComponent } from './components/auth/login/login';
import { RegisterComponent } from './components/auth/register/register';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: 'categories',
    component: CategoryComponent,
    canActivate: [roleGuard],
    data: { roles: ['Admin', 'Staff', 'Customer'] }
  },
  {
    path: 'food-items',
    component: FoodItemComponent,
    canActivate: [roleGuard],
    data: { roles: ['Admin', 'Staff', 'Customer'] }
  },

  {
    path: 'unauthorized',
    component: UnauthorizedComponent
  },

  { path: '', redirectTo: 'categories', pathMatch: 'full' },
  { path: '**', redirectTo: 'categories' }
];