import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FriendsDialogComponent } from '../friends-dialog/friends-dialog.component';
import { NotificationsDialogComponent } from '../notifications-dialog/notifications-dialog.component';
import { Notification } from 'src/app/models/api-responses/notification';
import { DataService } from 'src/app/services/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-menu-bar',
  templateUrl: './menu-bar.component.html',
  styleUrls: ['./menu-bar.component.scss']
})
export class MenuBarComponent implements OnInit, OnDestroy {
  
  /** Array of notifications for the authenticated user. */
  public notifications: Notification[] = [];

  /** Subscription to notifications from the DataService. */
  private notificationSubscription!: Subscription;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.subscribeToNotifications();
  }

  ngOnDestroy() {
    this.notificationSubscription?.unsubscribe();
  }

  /**
   * Subscribes to changes in notifications and updates the local notifications array.
   */
  private subscribeToNotifications(): void {
    this.notificationSubscription = this.dataService.notifications$.subscribe(notifications => {
      this.notifications = notifications;
    });
  }

  /**
   * Logs out the user and navigates to the login page.
   */
  public logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
  }

  /**
   * Opens the friends dialog.
   */
  public openFriendsDialog(): void {
    this.dialog.open(FriendsDialogComponent);
  }

  /**
   * Opens the notifications dialog.
   */
  public openNotificationsDialog(): void {
    this.dialog.open(NotificationsDialogComponent);
  }
}
