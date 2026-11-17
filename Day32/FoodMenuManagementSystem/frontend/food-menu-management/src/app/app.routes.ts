
import { Routes } from '@angular/router';

import { CategoryComponent } from './components/category/category';
import { FoodItemComponent } from './components/food-item/food-item/food-item';
export const routes: Routes = [
  {
    path: 'categories',
    component: CategoryComponent
  },
  {
    path: 'food-items',
    component: FoodItemComponent
  },
  {
    path: '',
    redirectTo: 'categories',
    pathMatch: 'full'
  }
];
