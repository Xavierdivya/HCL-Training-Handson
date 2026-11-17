
import { Routes } from '@angular/router';
import { CategoryComponent } from './components/category/category';

export const routes: Routes = [
  {
    path: 'categories',
    component: CategoryComponent
  },
  {
    path: '',
    redirectTo: 'categories',
    pathMatch: 'full'
  }
];
