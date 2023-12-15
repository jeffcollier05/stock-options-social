import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';
import { Subscription } from 'rxjs';
import { AcceptFriendRequestRequest } from 'src/app/models/acceptFriendRequestRequest';
import { CreateFriendRequestRequest } from 'src/app/models/createFriendRequestRequest';
import { DeclineFriendRequestRequest } from 'src/app/models/declineFriendRequestRequest';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { ModifyFriendPairRequest } from 'src/app/models/modifyFriendPairRequest';
import { UserProfile } from 'src/app/models/userProfile';
import { UserProfileView } from 'src/app/models/userProfileView';
import { ApiService } from 'src/app/services/api.service';
import { AuthenticationService } from 'src/app/services/authentication.services';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-friends-dialog',
  templateUrl: './friends-dialog.component.html',
  styleUrls: ['./friends-dialog.component.scss']
})
export class FriendsDialogComponent implements OnInit {
  
  public users: UserProfileView[] = [];
  private usersSubscription!: Subscription;

  constructor(
    private apiService: ApiService,
    private snackbar: MatSnackBar,
    private authService: AuthenticationService,
    private dataService: DataService
    ) { }

  ngOnInit(): void {
    this.usersSubscription = this.dataService.users$.subscribe(users => {
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

  ngOnDestroy() {
    this.usersSubscription?.unsubscribe();
  }
  
  public removeFriend(view: UserProfileView): void {
    var request: ModifyFriendPairRequest = {
      friendUserId: view.userProfile.userId
    };

    view.deleteFriendWaiting = true;
    this.apiService.removeFriend(request).subscribe(resp => {
      view.deleteFriendWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)
        newUsers[index].isFriend = false;
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend was removed.', 'Close');
      }
    });
  }

  public sendFriendRequest(view: UserProfileView): void {
    var request: CreateFriendRequestRequest = {
      receiverUserId: view.userProfile.userId
    };

    view.sendRequestWaiting = true;
    this.apiService.createFriendRequest(request).subscribe(resp => {
      view.sendRequestWaiting = true;
      if (!(resp instanceof ErrorViewModel)) {
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)
        newUsers[index].receivedFriendRequestFromYou = true;
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend request was sent!', 'Close');
      }
    });
  }

  public acceptFriendRequest(view: UserProfileView): void {
    var request: AcceptFriendRequestRequest = {
      requesterUserId: view.userProfile.userId
    };

    view.acceptRequestWaiting = true;
    this.apiService.acceptFriendRequest(request).subscribe(resp => {
      view.acceptRequestWaiting = true;
      if (!(resp instanceof ErrorViewModel)) {
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)
        newUsers[index].isFriend = true;
        newUsers[index].sentFriendRequestToYou = false;
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend request was accepted!', 'Close');
      }
    });
  }

  public deleteFriendRequest(view: UserProfileView): void {
    var request: DeclineFriendRequestRequest = {
      requesterUserId: view.userProfile.userId
    };

    view.declineRequestWaiting = true;
    this.apiService.declineFriendRequest(request).subscribe(resp => {
      view.declineRequestWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        var newUsers = this.users.map(x => x.userProfile);
        var index = newUsers.findIndex(x => x.userId == view.userProfile.userId)
        newUsers[index].sentFriendRequestToYou = false;
        this.dataService.pushAllUsers(newUsers);
        this.snackbar.open('Friend request was decline.', 'Close');
      }
    });
  }

  public haveAtLeastOneFriend(): boolean {
    return this.users.find(x => x.userProfile.isFriend) != undefined;
  }

  public haveAtLeastOneFriendRequest(): boolean {
    return this.users.find(x => x.userProfile.sentFriendRequestToYou) != undefined;
  }
}
