import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreatePostDialogComponent } from '../create-post-dialog/create-post-dialog.component';
import { JwtPayload, jwtDecode } from 'jwt-decode';
import { ActiveUser } from 'src/app/models/activeUser';

@Component({
  selector: 'app-action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.scss']
})
export class ActionBarComponent {

  public activeUser: ActiveUser = new ActiveUser();

  constructor(private dialog: MatDialog) { 
    this.getActiveUserFromJwt();
    console.log(this.activeUser.profilePictureUrl);
  }

  public openCreatePostDialog(): void {
    this.dialog.open(CreatePostDialogComponent);
  }

  public getActiveUserFromJwt(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      var decodedToken: any = jwtDecode(token);
      var activeUser: ActiveUser = {
        firstName: decodedToken.FirstName,
        lastName:  decodedToken.LastName,
        username:  decodedToken.Username,
        profilePictureUrl: decodedToken.ProfilePictureUrl
      };
      this.activeUser = activeUser;
    }
  }

}
