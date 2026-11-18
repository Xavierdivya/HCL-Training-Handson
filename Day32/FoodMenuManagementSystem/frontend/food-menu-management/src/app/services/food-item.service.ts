
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { FoodItem, CreateFoodItem } from '../models/food-item.model';

@Injectable({
  providedIn: 'root'
})
export class FoodItemService {

  private http = inject(HttpClient);

  private readonly apiUrl = 'http://localhost:5186/api/FoodItem';

  // GET: Retrieve all food items
  getFoodItems(): Observable<FoodItem[]> {
    return this.http.get<FoodItem[]>(this.apiUrl);
  }

  // GET: Retrieve food item by ID
  getFoodItemById(id: number): Observable<FoodItem> {
    return this.http.get<FoodItem>(`${this.apiUrl}/${id}`);
  }

  // POST: Create food item
  createFoodItem(foodItem: CreateFoodItem): Observable<any> {
    return this.http.post<any>(this.apiUrl, foodItem);
  }

  // PUT: Update food item
  updateFoodItem(
    id: number,
    foodItem: CreateFoodItem
  ): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, foodItem);
  }

  // DELETE: Delete food item
  deleteFoodItem(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
