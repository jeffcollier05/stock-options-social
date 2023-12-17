import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { delay, of } from 'rxjs';
import { ErrorViewModel } from 'src/app/models/api-responses/errorViewModel';
import { JwtAuth } from 'src/app/models/auth-responses/jwtAuth';
import { Register } from 'src/app/models/auth-requests/register';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-registerpage',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent {
  /** The registration data. */
  registerDto = new Register();

  /** The JWT authentication data. */
  jwtDto = new JwtAuth();

  /** Error message in case of registration failure. */
  errorMessage: string = '';

  /** Indicates whether the registration was successful. */
  successRegister: boolean = false;

  /** Indicates whether the component is waiting for a response from the server. */
  waitingOnResponse: boolean = false;

  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) { }

  /**
   * Initiates the registration process by sending API call.
   */
  public register(registerDto: Register): void {
    // Set status of API call attempting to register user.
    this.waitingOnResponse = true;

    // Simulate a delay to allow users to see the API call waiting
    of(null).pipe(delay(1000)).subscribe(() => {
      this.authService.register(registerDto).subscribe(resp => {
        this.waitingOnResponse = false;
        if (!(resp instanceof ErrorViewModel)) {

          // Handle succesful registration
          this.handleRegister(resp);
        } else {
          this.errorMessage = resp.details;
        }
      });
    });
  }

  /**
   * Handles the successful registration.
   */
  private handleRegister(jwtAuth: JwtAuth): void {
    // Store JWT in local browser storage
    localStorage.setItem('jwtToken', jwtAuth.token);
    this.successRegister = true;

    // Simulate a delay to allow users to see the success message
    of(null).pipe(delay(1000)).subscribe(() => {
      this.router.navigate(['/home']);
    });
  }

  /**
   * Navigates to the login page.
   */
  public goToLoginPage(): void {
    this.router.navigate(['/login']);
  }
}
