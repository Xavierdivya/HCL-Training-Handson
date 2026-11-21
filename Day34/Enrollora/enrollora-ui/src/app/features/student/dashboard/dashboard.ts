
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { CourseResponse } from '../../admin/courses/course.models';
import { CourseService } from '../../admin/courses/course.service';
import { EnrollmentResponse } from '../../enrollments/enrollment.models';
import { EnrollmentService } from '../../enrollments/enrollment.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  styleUrl: './dashboard.css',
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  private readonly courseService = inject(CourseService);
  private readonly enrollmentService = inject(EnrollmentService);

  availableCourses = signal<CourseResponse[]>([]);
  recentEnrollments = signal<EnrollmentResponse[]>([]);

  activeEnrollmentCount = signal(0);
  cancelledEnrollmentCount = signal(0);
  totalEnrollmentCount = signal(0);
  availableCourseCount = signal(0);
  openSeatCount = signal(0);

  isLoading = signal(true);
  courseError = signal('');
  enrollmentError = signal('');

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading.set(true);
    this.courseError.set('');
    this.enrollmentError.set('');

    let courseRequestComplete = false;
    let enrollmentRequestComplete = false;

    const markComplete = (): void => {
      if (courseRequestComplete && enrollmentRequestComplete) {
        this.isLoading.set(false);
      }
    };

    // Load available courses
    this.courseService
      .getCourses(1, 6, { isActive: true })
      .subscribe({
        next: (response) => {
          const eligibleCourses = response.items.filter(
            (course) => course.isActive && course.availableSeats > 0
          );

          this.availableCourses.set(eligibleCourses);

          this.availableCourseCount.set(eligibleCourses.length);

          this.openSeatCount.set(
            eligibleCourses.reduce(
              (total, course) => total + course.availableSeats,
              0
            )
          );

          courseRequestComplete = true;
          markComplete();
        },
        error: (error: HttpErrorResponse) => {
          this.courseError.set(
            this.getErrorMessage(
              error,
              'Unable to load available courses.'
            )
          );

          courseRequestComplete = true;
          markComplete();
        },
      });

    // Load recent enrollments
    this.enrollmentService
      .getMyEnrollments(1, 6)
      .subscribe({
        next: (response) => {
          this.recentEnrollments.set(response.items);
          this.totalEnrollmentCount.set(response.totalRecords);

          this.loadEnrollmentStatistics(markComplete, () => {
            enrollmentRequestComplete = true;
          });
        },
        error: (error: HttpErrorResponse) => {
          this.enrollmentError.set(
            this.getErrorMessage(
              error,
              'Unable to load your enrollments.'
            )
          );

          enrollmentRequestComplete = true;
          markComplete();
        },
      });

    // Keep loading until both course and enrollment requests finish.
    const originalMarkComplete = markComplete;
    void originalMarkComplete;
  }

  private loadEnrollmentStatistics(
    markComplete: () => void,
    onComplete: () => void
  ): void {
    let activeComplete = false;
    let cancelledComplete = false;

    const checkStatistics = (): void => {
      if (activeComplete && cancelledComplete) {
        onComplete();
        markComplete();
      }
    };

    // Active = 0
    this.enrollmentService
      .getMyEnrollments(1, 1, { status: 0 })
      .subscribe({
        next: (response) => {
          this.activeEnrollmentCount.set(response.totalRecords);
          activeComplete = true;
          checkStatistics();
        },
        error: () => {
          this.activeEnrollmentCount.set(0);
          activeComplete = true;
          checkStatistics();
        },
      });

    // Cancelled = 1
    this.enrollmentService
      .getMyEnrollments(1, 1, { status: 1 })
      .subscribe({
        next: (response) => {
          this.cancelledEnrollmentCount.set(response.totalRecords);
          cancelledComplete = true;
          checkStatistics();
        },
        error: () => {
          this.cancelledEnrollmentCount.set(0);
          cancelledComplete = true;
          checkStatistics();
        },
      });
  }

  statusLabel(status: number): string {
    return status === 0 ? 'Active' : 'Cancelled';
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallback: string
  ): string {
    const body = error.error;

    if (typeof body === 'string' && body.trim()) {
      return body;
    }

    if (body?.detail) {
      return body.detail;
    }

    if (body?.message) {
      return body.message;
    }

    if (body?.title) {
      return body.title;
    }

    return fallback;
  }
}
