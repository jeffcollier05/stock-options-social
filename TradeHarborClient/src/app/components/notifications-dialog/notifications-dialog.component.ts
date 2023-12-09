import { Component, OnInit } from '@angular/core';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { ApiService } from 'src/app/services/api.service';
import { Notification } from 'src/app/models/notification';

@Component({
  selector: 'app-notifications-dialog',
  templateUrl: './notifications-dialog.component.html',
  styleUrls: ['./notifications-dialog.component.scss']
})
export class NotificationsDialogComponent implements OnInit {

  public notifications: Notification[] = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.getNotifications().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.notifications = resp;
      }
    });
  }
}
