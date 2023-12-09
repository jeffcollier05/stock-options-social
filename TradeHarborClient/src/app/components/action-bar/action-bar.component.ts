import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreatePostDialogComponent } from '../create-post-dialog/create-post-dialog.component';
import { ActiveUser } from 'src/app/models/activeUser';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.scss']
})
export class ActionBarComponent {

  public activeUser: ActiveUser = new ActiveUser();

  constructor(private dialog: MatDialog, private authService: AuthenticationService) { 
    this.activeUser = this.authService.getActiveUserFromJwt()
  }

  public openCreatePostDialog(): void {
    this.dialog.open(CreatePostDialogComponent);
  }

}
