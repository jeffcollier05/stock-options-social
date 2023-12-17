import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { Notification } from 'src/app/models/api-responses/notification';
import { ErrorViewModel } from '../models/api-responses/errorViewModel';
import { UserProfile } from '../models/api-responses/userProfile';

/**
 * Service for managing and sharing data between components.
 */
@Injectable({
  providedIn: 'root'
})
export class DataService {
  /**
   * BehaviorSubject for managing and observing notifications.
   */
  private notificationSubject = new BehaviorSubject<Notification[]>([]);

  /**
   * BehaviorSubject for managing and observing user profiles.
   */
  private userSubject = new BehaviorSubject<UserProfile[]>([]);

  /**
   * Constructor for DataService.
   * @param {ApiService} apiService - The API service for making data-related requests.
   */
  constructor(private apiService: ApiService) {
    // Initialize the service by fetching initial data.
    this.getNotifications();
    this.getAllUsers();
  }

  /**
   * Observable stream of notifications.
   */
  public notifications$ = this.notificationSubject.asObservable();

  /**
   * Observable stream of user profiles.
   */
  public users$ = this.userSubject.asObservable();

  /**
   * Fetches notifications from the API and updates the notificationSubject.
   */
  public getNotifications(): void {
    this.apiService.getNotifications().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.notificationSubject.next(resp);
      }
    });
  }

  /**
   * Updates the notificationSubject with the provided notifications.
   * @param {Notification[]} notifications - The notifications to update.
   */
  public pushNotifications(notifications: Notification[]): void {
    this.notificationSubject.next(notifications);
  }

  /**
   * Fetches all user profiles from the API and updates the userSubject.
   */
  public getAllUsers(): void {
    this.apiService.getAllUsers().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.userSubject.next(resp);
      }
    });
  }

  /**
   * Updates the userSubject with the provided user profiles.
   * @param {UserProfile[]} users - The user profiles to update.
   */
  public pushAllUsers(users: UserProfile[]): void {
    this.userSubject.next(users);
  }
}
