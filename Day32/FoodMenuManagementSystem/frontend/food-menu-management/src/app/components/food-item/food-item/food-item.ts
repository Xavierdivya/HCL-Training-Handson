
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

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
import { AuthService } from '../../../services/auth/auth.service';

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
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  // User roles
  isAdmin = false;
  isStaff = false;
  canManageFoodItems = false;

  // Food data
  categories: Category[] = [];
  foodItems: FoodItem[] = [];

  foodItemForm: CreateFoodItem = this.emptyFoodItem();

  // Search
  searchText = '';

  // Form states
  isEditing = false;
  editingFoodItemId: number | null = null;

  // Loading states
  isLoading = false;
  isSaving = false;

  // Messages
  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {

    const role = this.authService.getUserRole();

    console.log('Logged-in role:', role);

    this.isAdmin = role === 'Admin';
    this.isStaff = role === 'Staff';
    this.canManageFoodItems = this.isAdmin || this.isStaff;

    this.loadCategories();
    this.loadFoodItems();
  }

  // Empty food item object
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

  // Load all categories
  loadCategories(): void {

    this.categoryService.getCategories().subscribe({

      next: (data: Category[]) => {

        this.categories = data ?? [];

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Category loading failed:', error);

        this.errorMessage = 'Unable to load categories.';

        this.cdr.detectChanges();
      }

    });
  }

  // Load all food items
  loadFoodItems(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.foodItemService.getFoodItems()
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({

        next: (data: FoodItem[]) => {

          console.log('Food items loaded:', data);

          this.foodItems = data ?? [];

          this.cdr.detectChanges();
        },

        error: (error) => {

          console.error('Food item loading failed:', error);

          this.foodItems = [];
          this.errorMessage = 'Unable to load food items.';

          this.cdr.detectChanges();
        }

      });
  }

  // Search and filter food items
  get filteredFoodItems(): FoodItem[] {

    const search = this.searchText.toLowerCase().trim();

    return this.foodItems.filter(item =>
      item.foodName.toLowerCase().includes(search) ||
      (item.description ?? '').toLowerCase().includes(search) ||
      this.getCategoryName(item.categoryId).toLowerCase().includes(search)
    );
  }

  // Get category name
  getCategoryName(categoryId: number): string {

    const category = this.categories.find(
      item => item.categoryId === categoryId
    );

    return category?.categoryName ?? 'Unknown';
  }

  // Add or update food item
  saveFoodItem(): void {

    if (!this.canManageFoodItems) {
      this.errorMessage = 'You are not authorized to manage food items.';
      return;
    }

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

    this.isSaving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const payload: CreateFoodItem = {
      ...this.foodItemForm,
      foodName: this.foodItemForm.foodName.trim()
    };

    const request =
      this.isEditing && this.editingFoodItemId !== null
        ? this.foodItemService.updateFoodItem(
            this.editingFoodItemId,
            payload
          )
        : this.foodItemService.createFoodItem(payload);

    request
      .pipe(
        finalize(() => {
          this.isSaving = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({

        next: () => {

          this.successMessage = this.isEditing
            ? 'Food item updated successfully.'
            : 'Food item added successfully.';

          this.resetForm(false);
          this.loadFoodItems();
        },

        error: (error) => {

          console.error('Food item save failed:', error);

          this.errorMessage =
            error.error?.message ||
            error.error?.title ||
            'Unable to save food item.';
        }

      });
  }

  // Edit food item
  editFoodItem(id: number): void {

    if (!this.canManageFoodItems) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.foodItemService.getFoodItemById(id).subscribe({

      next: (item: FoodItem) => {

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

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Food item retrieval failed:', error);

        this.errorMessage = 'Unable to retrieve food item.';
      }

    });
  }

  // Delete food item (Admin only)
  deleteFoodItem(id: number): void {

    if (!this.isAdmin) {
      this.errorMessage = 'Only Admin can delete food items.';
      return;
    }

    if (!window.confirm('Are you sure you want to delete this food item?')) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.foodItemService.deleteFoodItem(id).subscribe({

      next: () => {

        this.successMessage = 'Food item deleted successfully.';

        if (this.editingFoodItemId === id) {
          this.resetForm(false);
        }

        this.loadFoodItems();
      },

      error: (error) => {

        console.error('Food item deletion failed:', error);

        this.errorMessage =
          error.error?.message || 'Unable to delete food item.';
      }

    });
  }

  // Reset food item form
  resetForm(clearMessages = true): void {

    this.foodItemForm = this.emptyFoodItem();

    this.isEditing = false;
    this.editingFoodItemId = null;

    if (clearMessages) {
      this.successMessage = '';
      this.errorMessage = '';
    }
  }

}
