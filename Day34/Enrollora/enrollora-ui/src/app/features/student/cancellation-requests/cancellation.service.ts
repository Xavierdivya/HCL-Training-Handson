
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  CancellationPageResponse,
  CreateCancellationRequest,
} from './cancellation.models';

@Injectable({
  providedIn: 'root',
})
export class CancellationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/cancellation-requests`;

  getMyRequests(
    pageNumber = 1,
    pageSize = 10
  ): Observable<CancellationPageResponse> {
    return this.http.get<CancellationPageResponse>(
      `${this.apiUrl}/my`,
      {
        params: {
          pageNumber,
          pageSize,
        },
      }
    );
  }

  createRequest(
    enrollmentId: number,
    request: CreateCancellationRequest
  ): Observable<unknown> {
    return this.http.post(
      `${environment.apiUrl}/enrollments/${enrollmentId}/cancellation-request`,
      request
    );
  }
}