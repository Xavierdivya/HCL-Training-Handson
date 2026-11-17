
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { FoodItem, CreateFoodItem } from '../models/food-item.model';

@Injectable({
  providedIn: 'root'
})
export class FoodItemService {

  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5186/api/FoodItem';

  // POST: Create food item
  createFoodItem(
    foodItem: CreateFoodItem
  ): Observable<{ message: string; foodItem: FoodItem }> {
    return this.http.post<{ message: string; foodItem: FoodItem }>(
      this.apiUrl,
      foodItem
    );
  }
}
