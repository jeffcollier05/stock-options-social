import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TradePost } from '../models/tradePost';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ErrorViewModel } from '../models/errorViewModel';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  public getTradePosts(): Observable<TradePost[] | ErrorViewModel> {
    return this.httpGet<TradePost[]>(environment.apiUrl + '/trades/gettrades')
  }

  // Wrapper for GET requests
  httpGet<T>(endpoint: string): Observable<T | ErrorViewModel> {
    const url = `${environment.apiUrl}/${endpoint}`;
    return this.http.get<T>(url).pipe(
      catchError(this.handleError)
    );
  }

  // Wrapper for POST requests
  httpPost<T>(endpoint: string, data: any): Observable<T | ErrorViewModel>{
    const url = `${environment.apiUrl}/${endpoint}`;
    return this.http.post<T>(url, data).pipe(
      catchError((error: HttpErrorResponse) => this.handleError(error))
    );
  }

  // Handles errors for requests
  private handleError(error: HttpErrorResponse): Observable<ErrorViewModel> {
    var message: string;
    var details: string;

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      message = 'An error occurred on the client side.',
      details = error.error.message
    } else {
      // Server-side error
      message = 'An error occurred on the server side.',
      details = error.error ? error.error : 'No additional details available.'
    }

    return of(new ErrorViewModel(message, details));
  }
}
