import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { Notification } from 'src/app/models/api-responses/notification';
import { ErrorViewModel } from '../models/api-responses/errorViewModel';
import { UserProfile } from '../models/api-responses/userProfile';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private notificationSubject = new BehaviorSubject<Notification[]>([]);
  private userSubject = new BehaviorSubject<UserProfile[]>([]);

  constructor(private apiService: ApiService) {
    this.getNotifications();
    this.getAllUsers();
  }

  public notifications$ = this.notificationSubject.asObservable();
  public users$ = this.userSubject.asObservable();

  public getNotifications(): void {
    this.apiService.getNotifications().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.notificationSubject.next(resp);
      }
    });
  }

  public pushNotifications(notifications: Notification[]): void {
    this.notificationSubject.next(notifications);
  }

  public getAllUsers(): void {
    this.apiService.getAllUsers().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.userSubject.next(resp);
      }
    });
  }

  public pushAllUsers(users: UserProfile[]): void {
    this.userSubject.next(users);
  }
}
