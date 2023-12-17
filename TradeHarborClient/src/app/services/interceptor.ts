import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { Router } from '@angular/router';

/**
 * Interceptor for handling authentication-related tasks for HTTP requests.
 */
@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

    constructor(
        private router: Router
    ) { }

    /**
     * Intercepts HTTP requests to modify or handle authentication-related tasks.
     * @param {HttpRequest<any>} request - The incoming HTTP request.
     * @param {HttpHandler} next - The next HTTP handler in the chain.
     * @returns {Observable<HttpEvent<any>>} An observable of the HTTP event.
     */
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // Can't expect local browser to have JWT if users are trying to register or login.
        // Only modify / filter requests if its an action within the application.
        if (!this.isAuthenticationRoute(request)) {
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

    /**
     * Checks if the provided URL corresponds to an authentication route.
     * @param {HttpRequest<any>} request - The HTTP request.
     * @returns {boolean} True if the URL corresponds to an authentication route, otherwise false.
     * @private
     */
    private isAuthenticationRoute(request: HttpRequest<any>): boolean {
        const endings = ['/login', '/register'];
        return endings.some(x => request.url.toLocaleLowerCase().endsWith(x));
    }

    /**
     * Checks if the JWT token is expired and performs necessary actions.
     * @param {string} token - The JWT token.
     * @private
     */
    private checkJwtExpired(token: string): void {
        const decodedToken: JwtPayload = jwtDecode(token);
        if (decodedToken?.exp !== undefined && decodedToken.exp * 1000 < Date.now()) {
            // Token is expired, redirect to login page
            localStorage.removeItem('jwtToken');
            this.router.navigate(['/login']);
        }
    }

    /**
     * Modifies the HTTP request to include the Authorization header with the JWT token.
     * @param {HttpRequest<any>} request - The HTTP request.
     * @param {string} token - The JWT token.
     * @returns {HttpRequest<any>} The modified HTTP request.
     * @private
     */
    private modifyRequest(request: HttpRequest<any>, token: string): HttpRequest<any> {
        const modifiedRequest = request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });
        return modifiedRequest;
    }
}
