import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { delay, of } from 'rxjs';
import { ErrorViewModel } from 'src/app/models/api-responses/errorViewModel';
import { JwtAuth } from 'src/app/models/auth-responses/jwtAuth';
import { Login } from 'src/app/models/auth-requests/login';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {
  /** The login data. */
  loginDto = new Login();

  /** The JWT authentication data. */
  jwtDto = new JwtAuth();

  /** Error message in case of login failure. */
  errorMessage: string = '';

  /** Indicates whether the login was successful. */
  successLogin: boolean = false;

  /** Indicates whether the component is waiting for a response from the server. */
  waitingOnResponse: boolean = false;

  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) {
    // TEMP VALUES FOR DEVELOPMENT
    this.loginDto.email = 'jeffcollier@gmail.com';
    this.loginDto.password = 'Password123!';
  }

  /**
   * Initiates the login process.
   */
  public login(loginDto: Login): void {
    // Set status of API call attempting to login user.
    this.waitingOnResponse = true;

    // Simulate a delay to allow users to see the API call waiting
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

  /**
   * Handles the successful login.
   */
  private handleLogin(jwtAuth: JwtAuth): void {
    // Store JWT in local browser storage
    localStorage.setItem('jwtToken', jwtAuth.token);
    this.successLogin = true;

    // Simulate a delay to allow users to see the success message
    of(null).pipe(delay(1000)).subscribe(() => {
      this.router.navigate(['/home']);
    });
  }
  
  /**
   * Navigates to the register page.
   */
  public goToRegisterPage(): void {
    this.router.navigate(['/register']);
  }
}
