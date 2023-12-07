import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { delay, of } from 'rxjs';
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
  waitingOnResponse: boolean = false;

  constructor(private authService: AuthenticationService, private router: Router) {
    // TEMP VALUES FOR DEVELOPMENT
    this.loginDto.email = 'jeff05@gmaill.com';
    this.loginDto.password = 'Password123!';
  }

  public login(loginDto: Login): void {
    this.waitingOnResponse = true;
    // Allow user to see api call waiting
    of(null).pipe(delay(1000)).subscribe(() => {
      this.authService.login(loginDto).subscribe(resp => {
        this.waitingOnResponse = false;
        if (!(resp instanceof ErrorViewModel)) {
          // Successful login
          this.handleLogin(resp);
        } else {
          // Show error
          this.errorMessage = resp.details;
        }
      });
    });
  }

  private handleLogin(jwtAuth: JwtAuth): void {
    localStorage.setItem('jwtToken', jwtAuth.token);
    this.successLogin = true;
    // Allow user to see success message
    of(null).pipe(delay(1000)).subscribe(() => {
      this.router.navigate(['/home']);
    });
  }
  
  public goToRegisterPage(): void {
    this.router.navigate(['/register']);
  }
}
