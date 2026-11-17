
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { Category } from '../../../models/category.model';
import { CreateFoodItem } from '../../../models/food-item.model';

import { CategoryService } from '../../../services/category.service';
import { FoodItemService } from '../../../services/food-item.service';

@Component({
  selector: 'app-food-item',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './food-item.html',
  styleUrl: './food-item.css'
})
export class FoodItemComponent implements OnInit {

  // Inject services
  private categoryService = inject(CategoryService);
  private foodItemService = inject(FoodItemService);

  // Store categories for dropdown
  categories: Category[] = [];

  // Food item form data
  foodItemForm: CreateFoodItem = this.emptyFoodItem();

  // Loading and messages
  isLoading = false;
  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategories();
  }

  // Create an empty food item
  emptyFoodItem(): CreateFoodItem {
    return {
      foodName: '',
      description: '',
      price: 0,
      imageUrl: '',
      isAvailable: true,
      categoryId: 0
    };
  }

  // GET: Load categories for dropdown
  loadCategories(): void {

    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
      },
      error: (error) => {
        console.error('Category loading error:', error);
        this.errorMessage = 'Unable to load categories.';
      }
    });
  }

  // POST: Create food item
  saveFoodItem(): void {

    // Validate food name
    if (!this.foodItemForm.foodName.trim()) {
      this.errorMessage = 'Food name is required.';
      return;
    }

    // Validate price
    if (this.foodItemForm.price <= 0) {
      this.errorMessage = 'Price must be greater than zero.';
      return;
    }

    // Validate category
    if (this.foodItemForm.categoryId <= 0) {
      this.errorMessage = 'Please select a category.';
      return;
    }

    // Clear previous messages
    this.errorMessage = '';
    this.successMessage = '';

    // Show loading state
    this.isLoading = true;

    // Call POST API
    this.foodItemService.createFoodItem(this.foodItemForm)
      .pipe(
        finalize(() => {
          // Reset loading state after request completes
          this.isLoading = false;
        })
      )
      .subscribe({

        // API success
        next: (response) => {
          this.successMessage = response.message;

          // Reset form after successful creation
          this.foodItemForm = this.emptyFoodItem();
        },

        // API error
        error: (error) => {
          console.error('Food item creation error:', error);
          this.errorMessage = 'Failed to create food item.';
        }

      });
  }

  // Reset the form manually
  resetForm(): void {
    this.foodItemForm = this.emptyFoodItem();
    this.successMessage = '';
    this.errorMessage = '';
  }
}
