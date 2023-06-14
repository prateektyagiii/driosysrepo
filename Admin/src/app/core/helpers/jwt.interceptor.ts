import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthenticationService } from '../services/auth.service';
import { AuthfakeauthenticationService } from '../services/authfake.service';
import {ApicallService} from '../services/apicall.service';

import { environment } from '../../../environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService, private authfackservice: AuthfakeauthenticationService,private apicallservice : ApicallService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (environment.defaultauth === 'firebase') {
            const currentUser = this.authenticationService.currentUser();
            if (currentUser && currentUser.Result.access_token) {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${currentUser.Result.access_token}`
                    }
                });
            }
        } else {
            // add authorization header with jwt token if available
            const currentUser = this.apicallservice.currentUserValue;
            // if (currentUser && currentUser.token) {
            //     request = request.clone({
            //         setHeaders: {
            //             Authorization: `Bearer ${currentUser.token}`
            //         }
            //     });
            // }
            if (currentUser && currentUser.Result.access_token) {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${currentUser.Result.access_token}`
                    }
                });
            }
        }
        return next.handle(request);
    }
}
