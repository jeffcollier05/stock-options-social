import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreatePostDialogComponent } from '../create-post-dialog/create-post-dialog.component';
import { ActiveUser } from 'src/app/models/activeUser';
import { AuthenticationService } from 'src/app/services/authentication.services';
import { ApiService } from 'src/app/services/api.service';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { UserStatistics } from 'src/app/models/userStatistics';

@Component({
  selector: 'app-action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.scss']
})
export class ActionBarComponent {

  public activeUser: ActiveUser = new ActiveUser();
  public userStatistics: UserStatistics = new UserStatistics();

  constructor(
    private dialog: MatDialog,
    private authService: AuthenticationService,
    private apiService: ApiService
    ) { 
    this.activeUser = this.authService.getActiveUserFromJwt()
    this.getUserStatistics();
  }

  public openCreatePostDialog(): void {
    this.dialog.open(CreatePostDialogComponent);
  }

  
  private getUserStatistics(): void {
    this.apiService.getUserStatistics().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.userStatistics = resp;
      }
    });
  }

}
