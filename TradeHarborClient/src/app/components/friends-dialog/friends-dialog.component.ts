import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { AcceptFriendRequestRequest } from 'src/app/models/acceptFriendRequestRequest';
import { CreateFriendRequestRequest } from 'src/app/models/createFriendRequestRequest';
import { DeclineFriendRequestRequest } from 'src/app/models/declineFriendRequestRequest';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { ModifyFriendPairRequest } from 'src/app/models/modifyFriendPairRequest';
import { UserProfileView } from 'src/app/models/userProfileView';
import { ApiService } from 'src/app/services/api.service';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-friends-dialog',
  templateUrl: './friends-dialog.component.html',
  styleUrls: ['./friends-dialog.component.scss']
})
export class FriendsDialogComponent implements OnInit, OnDestroy {
  
  /** Array containing user profiles with additional properties for managing front end operations. */
  public users: UserProfileView[] = [];

  /** Subscription to user data updates. */
  private usersSubscription!: Subscription;

  constructor(
    private apiService: ApiService,
    private snackbar: MatSnackBar,
    private dataService: DataService
  ) { }

  ngOnInit(): void {
    this.subscribeToUsers();
  }

  ngOnDestroy() {
    this.usersSubscription?.unsubscribe();
  }

  /** Subscribes to changes in user data and initializes the users array with additional properties. */
  private subscribeToUsers(): void {
    this.usersSubscription = this.dataService.users$.subscribe(users => {
      // Map users to a front end model for state management
      this.users = users.map(x => (
        {
          userProfile: x,
          acceptRequestWaiting: false,
          declineRequestWaiting: false,
          sendRequestWaiting: false,
          deleteFriendWaiting: false
        }
      ));
    });
  }
  
  /**
   * Removes a friend by sending a request to the API service and updating the local data.
   */
  public removeFriend(view: UserProfileView): void {
    var request: ModifyFriendPairRequest = {
      friendUserId: view.userProfile.userId
    };

    // Set deleteFriendWaiting flag to true to indicate friend removal in progress
    view.deleteFriendWaiting = true;

    // Sends API request to remove friend from authenticated user
    this.apiService.removeFriend(request).subscribe(resp => {
      view.deleteFriendWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        // Update local data after successful friend removal
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)
        
        // Update isFriend status for the specific user
        newUsers[index].isFriend = false;

        // Push the updated user data to the DataService
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend was removed.', 'Close');
      }
    });
  }

  /**
   * Sends a friend request by making a request to the API service and updating the local data.
   */
  public sendFriendRequest(view: UserProfileView): void {
    var request: CreateFriendRequestRequest = {
      receiverUserId: view.userProfile.userId
    };

    // Set sendRequestWaiting flag to true to indicate friend request sending in progress
    view.sendRequestWaiting = true;

    // Sends API request to create friend request from authenticated user
    this.apiService.createFriendRequest(request).subscribe(resp => {
      view.sendRequestWaiting = true;
      if (!(resp instanceof ErrorViewModel)) {
        // Update local data after successful friend request sent
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)

        // Update receivedFriendRequestFromYou status for the specific user
        newUsers[index].receivedFriendRequestFromYou = true;

        // Push the updated user data to the DataService
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend request was sent!', 'Close');
      }
    });
  }

  /**
   * Accepts a friend request to the API service and updating the local data.
   */
  public acceptFriendRequest(view: UserProfileView): void {
    var request: AcceptFriendRequestRequest = {
      requesterUserId: view.userProfile.userId
    };

    // Set acceptRequestWaiting flag to true to indicate friend request accepting in progress
    view.acceptRequestWaiting = true;

    // Sends API request to create friend request from authenticated user
    this.apiService.acceptFriendRequest(request).subscribe(resp => {
      view.acceptRequestWaiting = true;
      if (!(resp instanceof ErrorViewModel)) {
        // Update local data after successful friend acceptance
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)

        // Update statuses for the specific user
        newUsers[index].isFriend = true;
        newUsers[index].sentFriendRequestToYou = false;

        // Push the updated user data to the DataService
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend request was accepted!', 'Close');
      }
    });
  }

  /**
   * Declines a friend request by making a request to the API service and updating the local data.
   */
  public deleteFriendRequest(view: UserProfileView): void {
    var request: DeclineFriendRequestRequest = {
      requesterUserId: view.userProfile.userId
    };

    // Set declineRequestWaiting flag to true to indicate friend request decline in progress
    view.declineRequestWaiting = true;

    // Sends API request to decline friend request for authenticated user
    this.apiService.declineFriendRequest(request).subscribe(resp => {
      view.declineRequestWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        // Update local data after successful friend request decline
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)

        // Update sentFriendRequestToYou status for the specific user
        newUsers[index].sentFriendRequestToYou = false;

        // Push the updated user data to the DataService
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend request was decline.', 'Close');
      }
    });
  }
  
  /**
   * Checks if the user has at least one friend in the current user list.
   */
  public haveAtLeastOneFriend(): boolean {
    return this.users.find(x => x.userProfile.isFriend) != undefined;
  }

  /**
   * Checks if the user has at least one friend request in the current user list.
   */
  public haveAtLeastOneFriendRequest(): boolean {
    return this.users.find(x => x.userProfile.sentFriendRequestToYou) != undefined;
  }
}
