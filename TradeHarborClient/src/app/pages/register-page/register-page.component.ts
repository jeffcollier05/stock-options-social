import { Component } from '@angular/core';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { JwtAuth } from 'src/app/models/jwtAuth';
import { Register } from 'src/app/models/register';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-registerpage',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent {
  registerDto = new Register();
  jwtDto = new JwtAuth();

  constructor(private authService: AuthenticationService) {}

  public register(registerDto: Register): void {
    this.authService.register(registerDto).subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        var jwtAuth = resp;
        console.log(jwtAuth.token);
      } else {
        console.log(resp.message)
      }
    });
  }

  public loginAccount(): void {
    window.location.href = '/login';
  }
}
