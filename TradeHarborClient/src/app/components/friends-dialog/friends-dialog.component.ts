import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { FriendProfile } from 'src/app/models/friendProfile';
import { ModifyFriendPairRequest } from 'src/app/models/modifyFriendPairRequest';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-friends-dialog',
  templateUrl: './friends-dialog.component.html',
  styleUrls: ['./friends-dialog.component.scss']
})
export class FriendsDialogComponent implements OnInit {

  public friendProfiles: FriendProfile[] = [];
  public users: FriendProfile[] = [];

  constructor(
    private apiService: ApiService,
    private snackbar: MatSnackBar) { }

  ngOnInit(): void {
    this.apiService.getFriendsForUser().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.friendProfiles = resp;
      }
    });

    this.apiService.getAllUsers().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        var userId = this.getUserIdFromJwt();
        var otherUsers = resp.filter(x => x.userId != userId);
        this.users = otherUsers;
      }
    });
  }

  private getUserIdFromJwt(): string {
    var userId = '';
    const token = localStorage.getItem('jwtToken');
    if (token) {
      var decodedToken: any = jwtDecode(token);
      userId = decodedToken.UserId;  
    }
    return userId;
  }

  public removeFriend(userId: string): void {
    var request: ModifyFriendPairRequest = {
      friendUserId: userId
    };

    this.apiService.removeFriend(request).subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        var newFriends = this.friendProfiles.filter(x => x.userId != userId);
        this.friendProfiles = newFriends;
        this.snackbar.open('Friend was removed.', 'Close');
      }
    });
  }

  public addFriend(userId: string): void {
    var request: ModifyFriendPairRequest = {
      friendUserId: userId
    };

    this.apiService.addFriend(request).subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        var profile = this.users.find(x => x.userId == userId);
        if (profile != undefined) {
          this.friendProfiles.push(profile);
        }
        this.snackbar.open('Friend was added!', 'Close');
      }
    });
  }

  public isUserMyFriend(userId: string): boolean {
    var profile = this.friendProfiles.find(x => x.userId == userId);
    var isFriend = profile != undefined;
    return isFriend;
  }
}
