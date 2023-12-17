import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TradePost } from '../models/api-responses/tradePost';
import { Observable, catchError, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ErrorViewModel } from '../models/api-responses/errorViewModel';
import { CreateTradePostRequest } from '../models/social-requests/createTradePostRequest';
import { ModifyFriendPairRequest } from '../models/social-requests/modifyFriendPairRequest';
import { DeleteTradePostRequest } from '../models/social-requests/deleteTradePostRequest';
import { Notification } from 'src/app/models/api-responses/notification';
import { DeleteNotificationRequest } from '../models/social-requests/deleteNotificationRequest';
import { CreateFriendRequestRequest } from '../models/social-requests/createFriendRequestRequest';
import { UserProfile } from '../models/api-responses/userProfile';
import { AcceptFriendRequestRequest } from '../models/social-requests/acceptFriendRequestRequest';
import { DeclineFriendRequestRequest } from '../models/social-requests/declineFriendRequestRequest';
import { PostReactionRequest } from '../models/social-requests/postReactionRequest';
import { PostCommentRequest } from '../models/social-requests/postCommentRequest';
import { UserStatistics } from '../models/api-responses/userStatistics';

/**
 * Service for handling API requests.
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(
    private http: HttpClient
  ) { }

  public getTradePosts(): Observable<TradePost[] | ErrorViewModel> {
    return this.httpGet<TradePost[]>('social/gettrades');
  }

  public createTradePost(request: CreateTradePostRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/CreateTradePost', request);
  }

  public deleteTradePost(request: DeleteTradePostRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/DeleteTradePost', request);
  }

  public getAllUsers(): Observable<UserProfile[] | ErrorViewModel> {
    return this.httpGet<UserProfile[]>('social/GetAllUsers');
  }

  public addFriend(request: ModifyFriendPairRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/AddFriend', request);
  }

  public removeFriend(request: ModifyFriendPairRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/RemoveFriend', request);
  }

  public getNotifications(): Observable<Notification[] | ErrorViewModel> {
    return this.httpGet<Notification[]>('social/GetNotifications');
  }

  public deleteNotification(request: DeleteNotificationRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/DeleteNotification', request);
  }

  public createFriendRequest(request: CreateFriendRequestRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/CreateFriendRequest', request);
  }

  public acceptFriendRequest(request: AcceptFriendRequestRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/AcceptFriendRequest', request);
  }

  public declineFriendRequest(request: DeclineFriendRequestRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/DeclineFriendRequest', request);
  }

  public reactToPost(request: PostReactionRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/ReactToPost', request);
  }

  public commentOnPost(request: PostCommentRequest): Observable<void | ErrorViewModel> {
    return this.httpPost<void>('social/CommentOnPost', request);
  }

  public getUserStatistics(): Observable<UserStatistics | ErrorViewModel> {
    return this.httpGet<UserStatistics>('social/GetUserStatistics');
  }
 
  /**
   * Http GET wrapper method for API calls.
   */
  httpGet<T>(endpoint: string): Observable<T | ErrorViewModel> {
    const url = `${environment.apiUrl}/${endpoint}`;
    return this.http.get<T>(url).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Http POST wrapper method for API calls.
   */
  httpPost<T>(endpoint: string, data: any): Observable<T | ErrorViewModel>{
    const url = `${environment.apiUrl}/${endpoint}`;
    return this.http.post<T>(url, data).pipe(
      catchError((error: HttpErrorResponse) => this.handleError(error))
    );
  }

  /**
   * Handles errors from API calls.
   */
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
