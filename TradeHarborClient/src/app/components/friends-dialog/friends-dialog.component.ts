import { Component, OnInit } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { FriendProfile } from 'src/app/models/friendProfile';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-friends-dialog',
  templateUrl: './friends-dialog.component.html',
  styleUrls: ['./friends-dialog.component.scss']
})
export class FriendsDialogComponent implements OnInit {

  public friendProfiles: FriendProfile[] = [];
  public users: FriendProfile[] = [];

  constructor(private apiService: ApiService) { }

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
}
