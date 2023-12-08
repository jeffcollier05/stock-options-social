import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FriendsDialogComponent } from '../friends-dialog/friends-dialog.component';

@Component({
  selector: 'app-menu-bar',
  templateUrl: './menu-bar.component.html',
  styleUrls: ['./menu-bar.component.scss']
})
export class MenuBarComponent {

  constructor(private router: Router, private dialog: MatDialog) { }

  public logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
  }

  public openFriendsDialog(): void {
    this.dialog.open(FriendsDialogComponent);
  }
}
