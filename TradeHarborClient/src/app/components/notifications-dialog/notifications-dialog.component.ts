import { Component, OnInit } from '@angular/core';
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
export class NotificationsDialogComponent implements OnInit {

  public notifications: NotificationView[] = [];
  private notificationSubscription!: Subscription;

  constructor(
    private apiService: ApiService,
    private snackbar: MatSnackBar,
    private dataService: DataService
    ) { }

  ngOnInit(): void {
    this.notificationSubscription = this.dataService.notifications$.subscribe(notifications => {
      this.notifications = notifications.map(notification => ({ notification: notification, deleteWaiting: false}));
    });
  }

  ngOnDestroy() {
    this.notificationSubscription?.unsubscribe();
  }

  public deleteNotification(view: NotificationView): void {
    var request: DeleteNotificationRequest = {
      notificationId: view.notification.notificationId
    };

    view.deleteWaiting = true;
    this.apiService.deleteNotification(request).subscribe(resp => {
      view.deleteWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        var newViews = this.notifications.filter(x => x.notification.notificationId != view.notification.notificationId);
        var newNotifications: Notification[] = newViews.map(x => x.notification);
        this.dataService.pushNotifications(newNotifications);
        this.snackbar.open('Notification was dismissed.', 'Close');
      }
    });
  }
}
