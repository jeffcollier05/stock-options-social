import { Component, OnDestroy, OnInit } from '@angular/core';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { ApiService } from 'src/app/services/api.service';
import { Notification } from 'src/app/models/notification';
import { DeleteNotificationRequest } from 'src/app/models/deleteNotificationRequest';
import { NotificationView } from 'src/app/models/notificationView';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DataService } from 'src/app/services/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notifications-dialog',
  templateUrl: './notifications-dialog.component.html',
  styleUrls: ['./notifications-dialog.component.scss']
})
export class NotificationsDialogComponent implements OnInit, OnDestroy {

  /** Array of notifications with additional properties for front end state management. */
  public notifications: NotificationView[] = [];

  /** Subscription for notifications from the DataService. */
  private notificationSubscription!: Subscription;

  constructor(
    private apiService: ApiService,
    private snackbar: MatSnackBar,
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.subscribeToNotifications();
  }

  ngOnDestroy() {
    this.notificationSubscription?.unsubscribe();
  }

  /**
   * Subscribes to changes in notifications and updates the local notifications array with additional properties.
   */
  private subscribeToNotifications(): void {
    this.notificationSubscription = this.dataService.notifications$.subscribe(notifications => {
      this.notifications = notifications.map(notification => ({ notification: notification, deleteWaiting: false}));
    });
  }

  /**
   * Deletes a notification by sending a request to the API service and updating the local data.
   */
  public deleteNotification(view: NotificationView): void {
    var request: DeleteNotificationRequest = {
      notificationId: view.notification.notificationId
    };

    // Set deleteWaiting flag to true to indicate deletion in progress
    view.deleteWaiting = true;

    // Call the API service to delete the notification
    this.apiService.deleteNotification(request).subscribe(resp => {
      view.deleteWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        // Update local data after successful notification deletion
        var newViews = this.notifications.filter(x => x.notification.notificationId != view.notification.notificationId);
        var newNotifications: Notification[] = newViews.map(x => x.notification);

        // Push the updated notification data to the DataService
        this.dataService.pushNotifications(newNotifications);
        this.snackbar.open('Notification was dismissed.', 'Close');
      }
    });
  }
}
