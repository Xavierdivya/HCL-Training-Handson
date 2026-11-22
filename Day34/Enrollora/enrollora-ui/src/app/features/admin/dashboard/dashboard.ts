
import {
  Component,
  inject,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
} from '@angular/core';
import { DecimalPipe, DatePipe } from '@angular/common';import { Subscription, timer } from 'rxjs';

import { DashboardService } from './dashboard.service';
import { DashboardSummary } from './dashboard.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [DecimalPipe, DatePipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit, OnDestroy {
  private readonly dashboardService = inject(DashboardService);
  private readonly cdr = inject(ChangeDetectorRef);

  private refreshSubscription?: Subscription;
  private isRequestInProgress = false;

  summary: DashboardSummary | null = null;
  isLoading = false;
  isRefreshing = false;
  errorMessage = '';
  lastUpdated: Date | null = null;

  ngOnInit(): void {
    this.loadDashboard();

    // Refresh dashboard data every 60 seconds.
    this.refreshSubscription = timer(60000, 60000).subscribe(() => {
      this.refreshDashboard();
    });
  }

  ngOnDestroy(): void {
    this.refreshSubscription?.unsubscribe();
  }

  loadDashboard(): void {
    if (this.isRequestInProgress) {
      return;
    }

    this.isRequestInProgress = true;
    this.isLoading = true;
    this.errorMessage = '';

    this.dashboardService.getSummary().subscribe({
      next: (response: DashboardSummary) => {
        this.summary = response;
        this.lastUpdated = new Date();
        this.isLoading = false;
        this.isRefreshing = false;
        this.isRequestInProgress = false;
        this.cdr.markForCheck();
      },
      error: (error: unknown) => {
        console.error('Dashboard loading failed:', error);

        this.errorMessage = this.summary
          ? 'Unable to refresh dashboard data. Showing the last successfully loaded data.'
          : 'Unable to load dashboard data. Please try again.';

        this.isLoading = false;
        this.isRefreshing = false;
        this.isRequestInProgress = false;
        this.cdr.markForCheck();
      },
    });
  }

  refreshDashboard(): void {
    if (this.isRequestInProgress) {
      return;
    }

    this.isRefreshing = true;
    this.loadDashboard();
  }
}
