import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from '../models/auth-requests/register';
import { JwtAuth } from '../models/auth-responses/jwtAuth';
import { Login } from '../models/auth-requests/login';
import { ErrorViewModel } from '../models/api-responses/errorViewModel';
import { ApiService } from './api.service';
import { jwtDecode } from 'jwt-decode';
import { ActiveUser } from '../models/api-responses/activeUser';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private apiService: ApiService) { }

  public register(user: Register): Observable<JwtAuth | ErrorViewModel> {
    var endpointUrl = 'AuthManagement/Register';
    return this.apiService.httpPost<JwtAuth>(endpointUrl, user);
  }

  public login(user: Login): Observable<JwtAuth | ErrorViewModel> {
    var endpointUrl = 'AuthManagement/Login';
    return this.apiService.httpPost<JwtAuth>(endpointUrl, user);
  }

  public getActiveUserFromJwt(): ActiveUser {
    var activeUser = new ActiveUser();
    const token = localStorage.getItem('jwtToken');

    if (token) {
      var decodedToken: any = jwtDecode(token);
      activeUser = {
        firstName: decodedToken.FirstName,
        lastName:  decodedToken.LastName,
        username:  decodedToken.Username,
        profilePictureUrl: decodedToken.ProfilePictureUrl
      };
    }
    return activeUser;
  }

  public getUserIdFromJwt(): string {
    var userId = '';
    const token = localStorage.getItem('jwtToken');
    if (token) {
      var decodedToken: any = jwtDecode(token);
      userId = decodedToken.UserId;  
    }
    return userId;
  }
}
