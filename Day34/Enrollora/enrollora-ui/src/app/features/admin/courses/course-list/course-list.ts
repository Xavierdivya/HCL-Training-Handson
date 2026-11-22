
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Observable } from 'rxjs';

import { CourseService } from '../course.service';
import {
  CourseResponse,
  CourseFilter,
  SaveCourseRequest,
} from '../course.models';

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

  courses = signal<CourseResponse[]>([]);

  isLoading = signal(false);
  isSaving = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  pageNumber = signal(1);
  pageSize = signal(10);
  totalRecords = signal(0);
  totalPages = signal(0);

  showForm = signal(false);
  isEditing = signal(false);
  editingId = signal<number | null>(null);

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

  filterForm = this.fb.group({
    search: [''],
    isActive: ['all'],
    availability: ['all'],
    minFees: this.fb.control<number | null>(null),
    maxFees: this.fb.control<number | null>(null),
    minCapacity: this.fb.control<number | null>(null),
    maxCapacity: this.fb.control<number | null>(null),
  });

  ngOnInit(): void {
    this.loadCourses();
  }

  // Summary figures below are intentionally calculated
  // from the currently displayed page, not the entire database.

  activeCoursesOnPage(): number {
    return this.courses().filter(course => course.isActive).length;
  }

  inactiveCoursesOnPage(): number {
    return this.courses().filter(course => !course.isActive).length;
  }

  availableSeatsOnPage(): number {
    return this.courses().reduce(
      (total, course) => total + course.availableSeats,
      0,
    );
  }

  loadCourses(): void {
    if (this.isLoading()) {
      return;
    }

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

    if (values.minCapacity !== null && values.minCapacity !== undefined) {
      filters.minCapacity = values.minCapacity;
    }

    if (values.maxCapacity !== null && values.maxCapacity !== undefined) {
      filters.maxCapacity = values.maxCapacity;
    }

    return filters;
  }

  applyFilters(): void {
    const values = this.filterForm.getRawValue();

    if (
      values.minFees !== null &&
      values.maxFees !== null &&
      values.minFees > values.maxFees
    ) {
      this.errorMessage.set('Minimum fee cannot be greater than maximum fee.');
      return;
    }

    if (
      values.minCapacity !== null &&
      values.maxCapacity !== null &&
      values.minCapacity > values.maxCapacity
    ) {
      this.errorMessage.set(
        'Minimum capacity cannot be greater than maximum capacity.',
      );
      return;
    }

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
      minCapacity: null,
      maxCapacity: null,
    });

    this.pageNumber.set(1);
    this.loadCourses();
  }

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

  saveCourse(): void {
    if (this.courseForm.invalid || this.isSaving()) {
      this.courseForm.markAllAsTouched();
      return;
    }

    const request: SaveCourseRequest = this.courseForm.getRawValue();

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

  toggleCourseStatus(course: CourseResponse): void {
    const action = course.isActive ? 'deactivate' : 'activate';

    const confirmed = window.confirm(
      `Are you sure you want to ${action} ${course.courseName}?`,
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage.set('');

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

  nextPage(): void {
    if (this.pageNumber() < this.totalPages() && !this.isLoading()) {
      this.pageNumber.update(page => page + 1);
      this.loadCourses();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1 && !this.isLoading()) {
      this.pageNumber.update(page => page - 1);
      this.loadCourses();
    }
  }
}
