import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FriendsDialogComponent } from '../friends-dialog/friends-dialog.component';
import { NotificationsDialogComponent } from '../notifications-dialog/notifications-dialog.component';
import { ApiService } from 'src/app/services/api.service';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { Notification } from 'src/app/models/notification';

@Component({
  selector: 'app-menu-bar',
  templateUrl: './menu-bar.component.html',
  styleUrls: ['./menu-bar.component.scss']
})
export class MenuBarComponent {
  
  public notifications: Notification[] = [];

  constructor(private router: Router, private dialog: MatDialog, private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.getNotifications().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.notifications = resp;
      }
    });
  }

  public logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
  }

  public openFriendsDialog(): void {
    this.dialog.open(FriendsDialogComponent);
  }

  public openNotificationsDialog(): void {
    this.dialog.open(NotificationsDialogComponent);
  }
}
