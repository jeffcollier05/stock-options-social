import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreatePostDialogComponent } from '../create-post-dialog/create-post-dialog.component';
import { ActiveUser } from 'src/app/models/activeUser';
import { AuthenticationService } from 'src/app/services/authentication.services';
import { ApiService } from 'src/app/services/api.service';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { UserStatistics } from 'src/app/models/userStatistics';
import { DataService } from 'src/app/services/data.service';
import { Subscription } from 'rxjs';
import { UserProfile } from 'src/app/models/userProfile';

@Component({
  selector: 'app-action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.scss']
})
export class ActionBarComponent implements OnInit {

  public activeUser: ActiveUser = new ActiveUser();
  public userStatistics: UserStatistics = new UserStatistics();
  private usersSubscription!: Subscription;
  public users: UserProfile[] = [];

  constructor(
    private dialog: MatDialog,
    private authService: AuthenticationService,
    private apiService: ApiService,
    private dataService: DataService
    ) { 
    this.activeUser = this.authService.getActiveUserFromJwt()
    this.getUserStatistics();
  }

  ngOnInit(): void {
    this.usersSubscription = this.dataService.users$.subscribe(users => {
      var numberOfUsers = 4;
      var shuffledArray = users.slice().sort(() => Math.random() - 0.5);
      this.users = shuffledArray.slice(0, numberOfUsers);
    });
  }

  ngOnDestroy() {
    this.usersSubscription?.unsubscribe();
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
