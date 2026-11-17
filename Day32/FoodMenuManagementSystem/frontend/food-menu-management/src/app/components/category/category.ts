
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Category } from '../../models/category.model';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './category.html',
  styleUrl: './category.css'
})
export class CategoryComponent implements OnInit {

  // Inject CategoryService
  private categoryService = inject(CategoryService);

  // Store all categories
  categories: Category[] = [];

  // Category form data
  categoryForm: Category = this.emptyCategory();

  // Track editing mode
  isEditing = false;
  editingId: number | null = null;

  // Loading and messages
  isLoading = false;
  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategories();
  }

  // Create an empty category object
  emptyCategory(): Category {
    return {
      categoryId: 0,
      categoryName: '',
      description: '',
      isActive: true
    };
  }

  // GET: Load all categories
  loadCategories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load categories.';
        this.isLoading = false;
      }
    });
  }

  // POST: Add a new category
  saveCategory(): void {

    if (!this.categoryForm.categoryName.trim()) {
      this.errorMessage = 'Category name is required.';
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    if (this.isEditing && this.editingId !== null) {

      // PUT: Update existing category
      this.categoryService.updateCategory(
        this.editingId,
        this.categoryForm
      ).subscribe({
        next: () => {
          this.successMessage = 'Category updated successfully.';
          this.resetForm();
          this.loadCategories();
        },
        error: () => {
          this.errorMessage = 'Failed to update category.';
        }
      });

    } else {

      // POST: Create new category
      this.categoryService.createCategory(
        this.categoryForm
      ).subscribe({
        next: () => {
          this.successMessage = 'Category added successfully.';
          this.resetForm();
          this.loadCategories();
        },
        error: () => {
          this.errorMessage = 'Failed to add category.';
        }
      });
    }
  }

  // Load category details into the form for editing
  editCategory(category: Category): void {
    this.categoryForm = { ...category };
    this.isEditing = true;
    this.editingId = category.categoryId;

    this.successMessage = '';
    this.errorMessage = '';
  }

  // DELETE: Remove a category
  deleteCategory(id: number): void {

    if (!confirm('Are you sure you want to delete this category?')) {
      return;
    }

    this.categoryService.deleteCategory(id).subscribe({
      next: () => {
        this.successMessage = 'Category deleted successfully.';
        this.loadCategories();
      },
      error: () => {
        this.errorMessage = 'Failed to delete category.';
      }
    });
  }

  // Reset the form
  resetForm(): void {
    this.categoryForm = this.emptyCategory();
    this.isEditing = false;
    this.editingId = null;
  }
}
