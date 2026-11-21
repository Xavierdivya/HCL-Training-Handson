
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import {
  CancellationPageResponse,
  CancellationResponse,
} from '../../student/cancellation-requests/cancellation.models';

@Injectable({
  providedIn: 'root',
})
export class AdminCancellationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/cancellation-requests`;

  getAllRequests(
    pageNumber = 1,
    pageSize = 20,
  ): Observable<CancellationPageResponse> {
    return this.http.get<CancellationPageResponse>(this.apiUrl, {
      params: { pageNumber, pageSize },
    });
  }

  approve(
    id: number,
    adminRemarks: string,
  ): Observable<CancellationResponse> {
    return this.http.put<CancellationResponse>(
      `${this.apiUrl}/${id}/approve`,
      { adminRemarks },
    );
  }

  reject(
    id: number,
    adminRemarks: string,
  ): Observable<CancellationResponse> {
    return this.http.put<CancellationResponse>(
      `${this.apiUrl}/${id}/reject`,
      { adminRemarks },
    );
  }
}
