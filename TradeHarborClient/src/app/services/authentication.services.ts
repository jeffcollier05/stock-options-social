import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from '../models/auth-requests/register';
import { JwtAuth } from '../models/auth-responses/jwtAuth';
import { Login } from '../models/auth-requests/login';
import { ErrorViewModel } from '../models/api-responses/errorViewModel';
import { ApiService } from './api.service';
import { jwtDecode } from 'jwt-decode';
import { ActiveUser } from '../models/api-responses/activeUser';

/**
 * Service for handling authentication-related functionality.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private apiService: ApiService) { }

  /**
   * Registers a new user.
   * @param {Register} user - The user registration data.
   * @returns {Observable<JwtAuth | ErrorViewModel>} An observable of the authentication result or an error view model.
   */
  public register(user: Register): Observable<JwtAuth | ErrorViewModel> {
    var endpointUrl = 'AuthManagement/Register';
    return this.apiService.httpPost<JwtAuth>(endpointUrl, user);
  }

  /**
   * Logs in a user.
   * @param {Login} user - The user login data.
   * @returns {Observable<JwtAuth | ErrorViewModel>} An observable of the authentication result or an error view model.
   */
  public login(user: Login): Observable<JwtAuth | ErrorViewModel> {
    var endpointUrl = 'AuthManagement/Login';
    return this.apiService.httpPost<JwtAuth>(endpointUrl, user);
  }

  /**
   * Retrieves the active user information from the JWT token.
   * @returns {ActiveUser} The active user information.
   */
  public getActiveUserFromJwt(): ActiveUser {
    // Retrieve JWT from browser storage
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

  /**
   * Retrieves the user ID from the JWT token.
   * @returns {string} The user ID.
   */
  public getUserIdFromJwt(): string {
    // Retrieve JWT from browser storage
    var userId = '';
    const token = localStorage.getItem('jwtToken');
    
    if (token) {
      var decodedToken: any = jwtDecode(token);
      userId = decodedToken.UserId;  
    }
    return userId;
  }
}
