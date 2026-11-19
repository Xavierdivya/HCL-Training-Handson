import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  Router,
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
  NavigationEnd
} from '@angular/router';
import { filter } from 'rxjs';

import { AuthService } from './services/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  private router = inject(Router);
  private authService = inject(AuthService);

  isAuthenticated = false;
  showNavbar = false;

  userName = '';
  userRole = '';

  ngOnInit(): void {
    this.updateLayout();

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.updateLayout());
  }

  updateLayout(): void {
    this.isAuthenticated = this.authService.isAuthenticated();

    this.showNavbar =
      this.isAuthenticated &&
      !['/login', '/register'].includes(this.router.url.split('?')[0]);

    const user = this.authService.getCurrentUser();

    this.userName = user?.fullName ?? '';
    this.userRole = user?.role ?? '';
  }

  logout(): void {
    this.authService.logout();
    this.updateLayout();
    this.router.navigate(['/login']);
  }
}