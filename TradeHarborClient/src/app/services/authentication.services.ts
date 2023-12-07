import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Register } from '../models/register';
import { JwtAuth } from '../models/jwtAuth';
import { Login } from '../models/login';
import { ErrorViewModel } from '../models/errorViewModel';
import { ApiService } from './api.service';

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
}
