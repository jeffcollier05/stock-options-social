import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreatePostDialogComponent } from '../create-post-dialog/create-post-dialog.component';
import { ActiveUser } from 'src/app/models/api-responses/activeUser';
import { AuthenticationService } from 'src/app/services/authentication.services';
import { ApiService } from 'src/app/services/api.service';
import { ErrorViewModel } from 'src/app/models/api-responses/errorViewModel';
import { UserStatistics } from 'src/app/models/api-responses/userStatistics';
import { DataService } from 'src/app/services/data.service';
import { Subscription } from 'rxjs';
import { UserProfile } from 'src/app/models/api-responses/userProfile';

@Component({
  selector: 'app-action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.scss']
})
export class ActionBarComponent implements OnInit, OnDestroy {

  /** The active user of the application. */
  public activeUser: ActiveUser = new ActiveUser();

  /** Statistics related to the user. */
  public userStatistics: UserStatistics = new UserStatistics();

  /** List of user profiles. */
  public users: UserProfile[] = [];

  /** Subscription to user data updates. */
  private usersSubscription!: Subscription;

  constructor(
    private dialog: MatDialog,
    private authService: AuthenticationService,
    private apiService: ApiService,
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.activeUser = this.authService.getActiveUserFromJwt();
    this.getUserStatistics();
    this.subscribeToUsers();
  }

  ngOnDestroy(): void {
    this.usersSubscription?.unsubscribe();
  }

  /** Opens the create post dialog. */
  public openCreatePostDialog(): void {
    this.dialog.open(CreatePostDialogComponent);
  }

  /** Retrieves user statistics from the API. */
  private getUserStatistics(): void {
    this.apiService.getUserStatistics().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.userStatistics = resp;
      }
    });
  }

  /** Subscribes to updates in the list of users. */
  private subscribeToUsers(): void {
    this.usersSubscription = this.dataService.users$.subscribe(users => {
      const numberOfUsers = 4;
      const shuffledArray = users.slice().sort(() => Math.random() - 0.5);
      this.users = shuffledArray.slice(0, numberOfUsers);
    });
  }
}
