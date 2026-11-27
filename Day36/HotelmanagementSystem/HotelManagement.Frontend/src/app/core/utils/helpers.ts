import { HttpErrorResponse } from '@angular/common/http';

/** Date -> 'yyyy-MM-dd' (local time, as the backend expects DateOnly). */
export function toIsoDate(date: Date): string {
  const y = date.getFullYear();
  const m = String(date.getMonth() + 1).padStart(2, '0');
  const d = String(date.getDate()).padStart(2, '0');
  return `${y}-${m}-${d}`;
}

/** Today + n days as 'yyyy-MM-dd'. */
export function daysFromToday(n: number): string {
  const d = new Date();
  d.setDate(d.getDate() + n);
  return toIsoDate(d);
}

/** Number of nights between two 'yyyy-MM-dd' dates. */
export function nightsBetween(from: string, to: string): number {
  if (!from || !to) return 0;
  const ms = new Date(to).getTime() - new Date(from).getTime();
  return Math.max(0, Math.round(ms / 86_400_000));
}

/** Pulls a readable message out of any backend error shape. */
export function errorMessage(err: unknown, fallback = 'Something went wrong. Please try again.'): string {
  if (err instanceof HttpErrorResponse) {
    if (err.status === 0) return 'Cannot reach the server. Is the backend running?';
    const body = err.error;
    if (typeof body === 'string' && body.trim()) return body;
    if (body?.message) return body.message;
    if (body?.errors) {
      const first = Object.values(body.errors as Record<string, string[]>)[0];
      if (first?.length) return first[0];
    }
    if (body?.title) return body.title;
    if (err.status === 403) return 'You do not have permission to do that.';
  }
  return fallback;
}
