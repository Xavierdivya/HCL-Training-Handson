
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { Category } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './category.html',
  styleUrl: './category.css'
})
export class CategoryComponent implements OnInit {

  private categoryService = inject(CategoryService);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  // User roles
  isAdmin = false;
  isStaff = false;
  canManageCategories = false;

  // Category data
  categories: Category[] = [];
  categoryForm: Category = this.emptyCategory();

  // Form states
  isEditing = false;
  editingId: number | null = null;

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
    this.canManageCategories = this.isAdmin || this.isStaff;

    this.loadCategories();
  }

  // Empty category object
  emptyCategory(): Category {
    return {
      categoryId: 0,
      categoryName: '',
      description: '',
      isActive: true
    };
  }

  // Load all categories
  loadCategories(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getCategories().subscribe({

      next: (data: Category[]) => {

        console.log('Categories loaded:', data);

        this.categories = data ?? [];
        this.isLoading = false;

        console.log('Loading state:', this.isLoading);
        console.log('Category count:', this.categories.length);

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Category API error:', error);

        this.categories = [];
        this.isLoading = false;
        this.errorMessage = 'Unable to load categories.';

        this.cdr.detectChanges();
      }

    });
  }

  // Add or update category
  saveCategory(): void {

    if (!this.canManageCategories) {
      this.errorMessage = 'You are not authorized to manage categories.';
      return;
    }

    if (!this.categoryForm.categoryName.trim()) {
      this.errorMessage = 'Category name is required.';
      return;
    }

    this.isSaving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const payload: Category = {
      ...this.categoryForm,
      categoryName: this.categoryForm.categoryName.trim()
    };

    const request =
      this.isEditing && this.editingId !== null
        ? this.categoryService.updateCategory(this.editingId, payload)
        : this.categoryService.createCategory(payload);

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
            ? 'Category updated successfully.'
            : 'Category added successfully.';

          this.resetForm(false);
          this.loadCategories();
        },

        error: (error) => {

          console.error('Category save failed:', error);

          this.errorMessage =
            error.error?.message || 'Unable to save category.';
        }

      });
  }

  // Edit category
  editCategory(category: Category): void {

    if (!this.canManageCategories) {
      return;
    }

    this.categoryForm = { ...category };

    this.isEditing = true;
    this.editingId = category.categoryId;

    this.successMessage = '';
    this.errorMessage = '';

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // Delete category (Admin only)
  deleteCategory(id: number): void {

    if (!this.isAdmin) {
      this.errorMessage = 'Only Admin can delete categories.';
      return;
    }

    if (!window.confirm('Are you sure you want to delete this category?')) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.categoryService.deleteCategory(id).subscribe({

      next: () => {

        this.successMessage = 'Category deleted successfully.';
        this.loadCategories();
      },

      error: (error) => {

        console.error('Category deletion failed:', error);

        this.errorMessage =
          error.error?.message || 'Unable to delete category.';
      }

    });
  }

  // Reset category form
  resetForm(clearMessages = true): void {

    this.categoryForm = this.emptyCategory();

    this.isEditing = false;
    this.editingId = null;

    if (clearMessages) {
      this.successMessage = '';
      this.errorMessage = '';
    }
  }

}
