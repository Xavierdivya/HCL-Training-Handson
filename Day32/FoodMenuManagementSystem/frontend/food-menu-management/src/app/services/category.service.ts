
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private http = inject(HttpClient);

private apiUrl = 'http://localhost:5000/api/Category';
  // GET: Retrieve all categories
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl);
  }

  // GET: Retrieve category by ID
  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  // POST: Create category
  createCategory(category: Category): Observable<Category> {
    return this.http.post<Category>(this.apiUrl, category);
  }

  // PUT: Update category
  updateCategory(
    id: number,
    category: Category
  ): Observable<Category> {
    return this.http.put<Category>(
      `${this.apiUrl}/${id}`,
      category
    );
  }

  // DELETE: Delete category
  deleteCategory(id: number): Observable<string> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      responseType: 'text'
    });
  }
}
