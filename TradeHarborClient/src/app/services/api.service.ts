import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TradePost } from '../models/tradePost';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ErrorViewModel } from '../models/errorViewModel';
import { CreateTradePostRequest } from '../models/createTradePostRequest';
import { FriendProfile } from '../models/friendProfile';
import { ModifyFriendPairRequest } from '../models/modifyFriendPairRequest';
import { DeleteTradePostRequest } from '../models/deleteTradePostRequest';
import { Notification } from 'src/app/models/notification';
import { DeleteNotificationRequest } from '../models/deleteNotificationRequest';
import { CreateFriendRequestRequest } from '../models/createFriendRequestRequest';
import { UserProfile } from '../models/userProfile';
import { AcceptFriendRequestRequest } from '../models/acceptFriendRequestRequest';
import { DeclineFriendRequestRequest } from '../models/declineFriendRequestRequest';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  public getTradePosts(): Observable<TradePost[] | ErrorViewModel> {
    return this.httpGet<TradePost[]>('trades/gettrades');
  }

  public createTradePost(request: CreateTradePostRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/CreateTradePost', request);
  }

  public deleteTradePost(request: DeleteTradePostRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/DeleteTradePost', request);
  }

  public getFriendsForUser(): Observable<FriendProfile[] | ErrorViewModel> {
    return this.httpGet<FriendProfile[]>('trades/GetFriendsForUser');
  }

  public getAllUsers(): Observable<UserProfile[] | ErrorViewModel> {
    return this.httpGet<UserProfile[]>('trades/GetAllUsers');
  }

  public addFriend(request: ModifyFriendPairRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/AddFriend', request);
  }

  public removeFriend(request: ModifyFriendPairRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/RemoveFriend', request);
  }

  public getNotifications(): Observable<Notification[] | ErrorViewModel> {
    return this.httpGet<Notification[]>('trades/GetNotifications');
  }

  public deleteNotification(request: DeleteNotificationRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/DeleteNotification', request);
  }

  public createFriendRequest(request: CreateFriendRequestRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/CreateFriendRequest', request);
  }

  public acceptFriendRequest(request: AcceptFriendRequestRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/AcceptFriendRequest', request);
  }

  public declineFriendRequest(request: DeclineFriendRequestRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('trades/DeclineFriendRequest', request);
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
