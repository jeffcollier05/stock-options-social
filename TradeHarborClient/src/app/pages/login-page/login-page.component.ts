import { Component } from '@angular/core';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { JwtAuth } from 'src/app/models/jwtAuth';
import { Login } from 'src/app/models/login';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  loginDto = new Login();
  jwtDto = new JwtAuth();

  errorMessage: string = '';
  successLogin: boolean = false;

  constructor(private authService: AuthenticationService) {}

  public login(loginDto: Login): void {
    this.authService.login(loginDto).subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        var jwtAuth = resp;
        localStorage.setItem('jwtToken', jwtAuth.token);
        this.successLogin = true;
        window.location.href = '/home';
      } else {
        this.errorMessage = resp.details;
      }
    });
  }
  
  public registerAccount(): void {
    window.location.href = '/register';
  }
}
