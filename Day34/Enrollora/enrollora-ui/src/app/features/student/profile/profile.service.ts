
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  StudentProfile,
  UpdateStudentProfileRequest,
} from './profile.models';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/students/me`;

  // GET: Retrieve the logged-in student's profile
  getMyProfile(): Observable<StudentProfile> {
    return this.http.get<StudentProfile>(this.apiUrl);
  }

  // PUT: Update the logged-in student's profile
  updateMyProfile(
    request: UpdateStudentProfileRequest,
  ): Observable<StudentProfile> {
    return this.http.put<StudentProfile>(this.apiUrl, request);
  }
}
