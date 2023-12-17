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
  registerDto = new Register();
  jwtDto = new JwtAuth();
  errorMessage: string = '';
  successRegister: boolean = false;
  waitingOnResponse: boolean = false;

  constructor(private authService: AuthenticationService, private router: Router) {}

  public register(registerDto: Register): void {
    this.waitingOnResponse = true;
    // Allow user to see api call waiting
    of(null).pipe(delay(1000)).subscribe(() => {
      this.authService.register(registerDto).subscribe(resp => {
        this.waitingOnResponse = false;
        if (!(resp instanceof ErrorViewModel)) {
          this.handleRegister(resp);
        } else {
          this.errorMessage = resp.details;
        }
      });
    });
  }

  private handleRegister(jwtAuth: JwtAuth): void {
    localStorage.setItem('jwtToken', jwtAuth.token);
    this.successRegister = true;
    // Allow user to see success message
    of(null).pipe(delay(1000)).subscribe(() => {
      this.router.navigate(['/home']);
    });
  }

  public goToLoginPage(): void {
    this.router.navigate(['/login']);
  }
}
