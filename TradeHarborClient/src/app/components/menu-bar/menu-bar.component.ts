import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FriendsDialogComponent } from '../friends-dialog/friends-dialog.component';
import { NotificationsDialogComponent } from '../notifications-dialog/notifications-dialog.component';
import { ApiService } from 'src/app/services/api.service';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { Notification } from 'src/app/models/notification';
import { DataService } from 'src/app/services/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-menu-bar',
  templateUrl: './menu-bar.component.html',
  styleUrls: ['./menu-bar.component.scss']
})
export class MenuBarComponent {
  
  public notifications: Notification[] = [];
  private dataSubscription!: Subscription;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private apiService: ApiService,
    private dataService: DataService
    ) { }

  ngOnInit(): void {
    this.dataSubscription = this.dataService.notifications$.subscribe(notifications => {
      this.notifications = notifications;
    });
  }

  ngOnDestroy() {
    this.dataSubscription?.unsubscribe();
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
