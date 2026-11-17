import { Routes } from '@angular/router';

import { CategoryComponent } from './components/category/category';
import { FoodItemComponent } from './components/food-item/food-item/food-item';

import { LoginComponent } from './components/auth/login/login';
import { RegisterComponent } from './components/auth/register/register';

import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  // Public routes
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },

  // Protected routes
  {
    path: 'categories',
    component: CategoryComponent,
    canActivate: [authGuard]
  },
  {
    path: 'food-items',
    component: FoodItemComponent,
    canActivate: [authGuard]
  },

  // Default route
  {
    path: '',
    redirectTo: 'categories',
    pathMatch: 'full'
  },

  {
    path: '**',
    redirectTo: 'categories'
  }
];