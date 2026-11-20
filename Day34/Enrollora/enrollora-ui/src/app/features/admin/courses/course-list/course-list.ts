
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { CourseService } from '../course.service';
import {
  CourseResponse,
  CourseFilter,
  SaveCourseRequest,
} from '../course.models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css',
})
export class CourseListComponent implements OnInit {

  private courseService = inject(CourseService);
  private fb = inject(FormBuilder);

  // Course data
  courses = signal<CourseResponse[]>([]);

  // Loading and messages
  isLoading = signal(false);
  isSaving = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  // Pagination
  pageNumber = signal(1);
  pageSize = signal(10);
  totalRecords = signal(0);
  totalPages = signal(0);

  // Form visibility
  showForm = signal(false);
  isEditing = signal(false);
  editingId = signal<number | null>(null);

  // Course form
  courseForm = this.fb.nonNullable.group({
    courseName: ['', [
      Validators.required,
      Validators.maxLength(150),
    ]],
    duration: ['', [
      Validators.required,
      Validators.maxLength(80),
    ]],
    fees: [0, [
      Validators.required,
      Validators.min(0),
    ]],
    capacity: [1, [
      Validators.required,
      Validators.min(1),
    ]],
    isActive: [true],
  });

  // Filter form
  filterForm = this.fb.group({
    search: [''],
    isActive: ['all'],
    availability: ['all'],
    minFees: this.fb.control<number | null>(null),
    maxFees: this.fb.control<number | null>(null),
  });

  ngOnInit(): void {
    this.loadCourses();
  }

  // --------------------------------
  // LOAD COURSES
  // --------------------------------

  loadCourses(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.courseService.getCourses(
      this.pageNumber(),
      this.pageSize(),
      this.buildFilters(),
    ).subscribe({
      next: (response) => {
        this.courses.set(response.items);
        this.totalRecords.set(response.totalRecords);
        this.totalPages.set(response.totalPages);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading courses:', error);
        this.errorMessage.set('Unable to load courses.');
        this.isLoading.set(false);
      },
    });
  }

  // --------------------------------
  // BUILD SEARCH FILTERS
  // --------------------------------

  private buildFilters(): CourseFilter {
    const values = this.filterForm.getRawValue();

    const filters: CourseFilter = {};

    if (values.search?.trim()) {
      filters.search = values.search.trim();
    }

    if (values.isActive !== 'all') {
      filters.isActive = values.isActive === 'active';
    }

    if (values.availability !== 'all') {
      filters.availability = values.availability ?? undefined;
    }

    if (values.minFees !== null && values.minFees !== undefined) {
      filters.minFees = values.minFees;
    }

    if (values.maxFees !== null && values.maxFees !== undefined) {
      filters.maxFees = values.maxFees;
    }

    return filters;
  }

  applyFilters(): void {
    this.pageNumber.set(1);
    this.loadCourses();
  }

  clearFilters(): void {
    this.filterForm.reset({
      search: '',
      isActive: 'all',
      availability: 'all',
      minFees: null,
      maxFees: null,
    });

    this.pageNumber.set(1);
    this.loadCourses();
  }

  // --------------------------------
  // OPEN CREATE FORM
  // --------------------------------

  openCreateForm(): void {
    this.isEditing.set(false);
    this.editingId.set(null);

    this.courseForm.reset({
      courseName: '',
      duration: '',
      fees: 0,
      capacity: 1,
      isActive: true,
    });

    this.errorMessage.set('');
    this.showForm.set(true);
  }

  // --------------------------------
  // OPEN EDIT FORM
  // --------------------------------

  openEditForm(course: CourseResponse): void {
    this.isEditing.set(true);
    this.editingId.set(course.courseId);

    this.courseForm.reset({
      courseName: course.courseName,
      duration: course.duration,
      fees: course.fees,
      capacity: course.capacity,
      isActive: course.isActive,
    });

    this.errorMessage.set('');
    this.showForm.set(true);
  }

  closeForm(): void {
    this.showForm.set(false);
    this.isSaving.set(false);
  }

  // --------------------------------
  // CREATE / UPDATE COURSE
  // --------------------------------

  saveCourse(): void {
    if (this.courseForm.invalid) {
      this.courseForm.markAllAsTouched();
      return;
    }

    const request: SaveCourseRequest =
      this.courseForm.getRawValue();

    this.isSaving.set(true);
    this.errorMessage.set('');

    const id = this.editingId();

    const operation = id !== null
      ? this.courseService.updateCourse(id, request)
      : this.courseService.createCourse(request);

    operation.subscribe({
      next: () => {
        this.successMessage.set(
          id !== null
            ? 'Course updated successfully.'
            : 'Course created successfully.',
        );

        this.closeForm();
        this.pageNumber.set(1);
        this.loadCourses();
      },
      error: (error) => {
        console.error('Error saving course:', error);

        this.errorMessage.set(
          error?.error?.detail ||
          error?.error?.message ||
          error?.error?.title ||
          'Unable to save course. Please try again.',
        );

        this.isSaving.set(false);
      },
    });
  }

  // --------------------------------
  // ACTIVATE / DEACTIVATE COURSE
  // --------------------------------

  toggleCourseStatus(course: CourseResponse): void {

    const action = course.isActive
      ? 'deactivate'
      : 'activate';

    const confirmed = window.confirm(
      `Are you sure you want to ${action} ${course.courseName}?`,
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage.set('');

    // Existing backend provides DELETE for deactivation.
    // PUT supports setting IsActive = true for activation.

    const operation: Observable<unknown> = course.isActive
      ? this.courseService.deactivateCourse(course.courseId)
      : this.courseService.updateCourse(course.courseId, {
          courseName: course.courseName,
          duration: course.duration,
          fees: course.fees,
          capacity: course.capacity,
          isActive: true,
        });

    operation.subscribe({
      next: () => {
        this.successMessage.set(
          `Course ${action}d successfully.`,
        );

        this.loadCourses();
      },
      error: (error) => {
        console.error(`Error trying to ${action} course:`, error);

        this.errorMessage.set(
          error?.error?.detail ||
          error?.error?.message ||
          `Unable to ${action} course.`,
        );
      },
    });
  }

  // --------------------------------
  // PAGINATION
  // --------------------------------

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(page => page + 1);
      this.loadCourses();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(page => page - 1);
      this.loadCourses();
    }
  }
}