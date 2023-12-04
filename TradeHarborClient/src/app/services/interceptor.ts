import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { jwtDecode,  JwtPayload } from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

    constructor(private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!this.IsAuthenticationRoute(request)) {
            const token = localStorage.getItem('jwtToken');
            if (token) {
                // Check if the token is expired
                this.checkJwtExpired(token);
                request = this.modifyRequest(request, token);
            } else {
                // No token present, redirect to login page
                this.router.navigate(['/login']);
            }
        }
        
        return next.handle(request);
    }

    private IsAuthenticationRoute(request: HttpRequest<any>): boolean {
        var endings = ['/login', '/register'];
        return endings.some(x => request.url.toLocaleLowerCase().endsWith(x));
    }

    private checkJwtExpired(token: string): void {
        var decodedToken: JwtPayload = jwtDecode(token);
        if (decodedToken?.exp != undefined && decodedToken.exp * 1000 < Date.now()) {
            // Token is expired, redirect to login page
            localStorage.removeItem('jwtToken');
            this.router.navigate(['/login']);
        }
    }

    private modifyRequest(request: HttpRequest<any>, token: string): HttpRequest<any> {
        var modifiedRequest = request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
        return modifiedRequest;
    }
}