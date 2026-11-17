
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { Category } from '../../../models/category.model';
import {
  FoodItem,
  CreateFoodItem
} from '../../../models/food-item.model';

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

  private categoryService = inject(CategoryService);
  private foodItemService = inject(FoodItemService);

  categories: Category[] = [];
  foodItems: FoodItem[] = [];

  foodItemForm: CreateFoodItem = this.emptyFoodItem();

  searchText = '';

  isLoading = false;
  isSaving = false;

  isEditing = false;
  editingFoodItemId = 0;

  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategories();
    this.loadFoodItems();
  }

  // Empty form
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

  // GET categories
  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to load categories.';
      }
    });
  }

  // GET all food items
  loadFoodItems(): void {
    this.isLoading = true;

    this.foodItemService.getFoodItems()
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe({
        next: (data) => {
          this.foodItems = data;
        },
        error: (error) => {
          console.error(error);
          this.errorMessage = 'Unable to load food items.';
        }
      });
  }

  // Search food items
  get filteredFoodItems(): FoodItem[] {
    const search = this.searchText.toLowerCase().trim();

    return this.foodItems.filter(item =>
      item.foodName.toLowerCase().includes(search)
    );
  }

  // Get category name using CategoryId
  getCategoryName(categoryId: number): string {
    const category = this.categories.find(
      c => c.categoryId === categoryId
    );

    return category?.categoryName ?? 'Unknown';
  }

  // POST / PUT: Save food item
  saveFoodItem(): void {

    if (!this.foodItemForm.foodName.trim()) {
      this.errorMessage = 'Food name is required.';
      return;
    }

    if (this.foodItemForm.price <= 0) {
      this.errorMessage = 'Price must be greater than zero.';
      return;
    }

    if (this.foodItemForm.categoryId <= 0) {
      this.errorMessage = 'Please select a category.';
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';
    this.isSaving = true;

    const request = this.isEditing
      ? this.foodItemService.updateFoodItem(
          this.editingFoodItemId,
          this.foodItemForm
        )
      : this.foodItemService.createFoodItem(
          this.foodItemForm
        );

    request
      .pipe(
        finalize(() => this.isSaving = false)
      )
      .subscribe({
        next: (response) => {

          this.successMessage = response.message ||
            (this.isEditing
              ? 'Food item updated successfully.'
              : 'Food item created successfully.');

          this.resetForm();
          this.loadFoodItems();
        },
        error: (error) => {
          console.error(error);
          this.errorMessage = error.error?.title ||
            error.error ||
            'Unable to save food item.';
        }
      });
  }

  // GET by ID and load into form for editing
  editFoodItem(id: number): void {

    this.errorMessage = '';
    this.successMessage = '';

    this.foodItemService.getFoodItemById(id).subscribe({
      next: (item) => {

        this.foodItemForm = {
          foodName: item.foodName,
          description: item.description ?? '',
          price: item.price,
          imageUrl: item.imageUrl ?? '',
          isAvailable: item.isAvailable,
          categoryId: item.categoryId
        };

        this.isEditing = true;
        this.editingFoodItemId = item.foodItemId;

        window.scrollTo({
          top: 0,
          behavior: 'smooth'
        });
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to retrieve food item.';
      }
    });
  }

  // DELETE food item
  deleteFoodItem(id: number): void {

    const confirmed = window.confirm(
      'Are you sure you want to delete this food item?'
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.foodItemService.deleteFoodItem(id).subscribe({
      next: (response) => {
        this.successMessage =
          typeof response === 'string'
            ? response
            : response.message || 'Food item deleted successfully.';

        if (this.editingFoodItemId === id) {
          this.resetForm();
        }

        this.loadFoodItems();
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to delete food item.';
      }
    });
  }

  // Reset form
  resetForm(): void {
    this.foodItemForm = this.emptyFoodItem();

    this.isEditing = false;
    this.editingFoodItemId = 0;

    this.errorMessage = '';
  }
}
