import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthenticationService } from '../../../core/services/auth.service';
import { AuthfakeauthenticationService } from '../../../core/services/authfake.service';

import { ActivatedRoute, Router } from '@angular/router';
import { first, map } from 'rxjs/operators';
import {ApicallService} from '../../../core/services/apicall.service';
import { catchError } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from 'src/app/core/models/auth.models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  submitted = false;
  error = '';
  returnUrl: string;
  token :any ;
  Username : any;
  Password : any
  loginformdata :any
  // set the currenr year
  year: number = new Date().getFullYear();
  // private currentUserSubject: BehaviorSubject<User>;
  // public currentUser: Observable<any>;

  // tslint:disable-next-line: max-line-length
   constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, public authenticationService: AuthenticationService, public authFackservice: AuthfakeauthenticationService,public apicallservice:ApicallService) { }

  ngOnInit() {
    document.body.removeAttribute('data-layout');
    document.body.classList.add('auth-body-bg');

    this.loginForm = this.formBuilder.group({
      Username: [this.Username, [Validators.required, Validators.email]],
      Password: [this.Password, [Validators.required]],
    });

  //   // reset login status
  //    this.apicallservice.logout();
  //   // get return url from route parameters or default to '/'
  //   // tslint:disable-next-line: no-string-literal

     this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
   }
  // constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, public authenticationService: AuthenticationService, public authFackservice: AuthfakeauthenticationService) { }

  // ngOnInit() {
  //   document.body.removeAttribute('data-layout');
  //   document.body.classList.add('auth-body-bg');

  //   this.loginForm = this.formBuilder.group({
  //     email: ['admin@themesdesign.in', [Validators.required, Validators.email]],
  //     password: ['123456', [Validators.required]],
  //   });

  //   this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

  // }
  // convenience getter for easy access to form fields

  //   get f() { return this.loginForm.controls; }
  
  // onSubmit() {
  //   this.submitted = true;

  //   // stop here if form is invalid
  //   if (this.loginForm.invalid) {
  //     return;
  //   } else 
  //   {
  //     if (environment.defaultauth === 'firebase') {
  //       this.authenticationService.login(this.f.email.value, this.f.password.value).then((res: any) => {
  //         this.router.navigate(['/']);
  //       })
  //         .catch(error => {
  //           this.error = error ? error : '';
  //         });
  //     } else
  //      {
  //       this.authFackservice.login(this.f.email.value, this.f.password.value)
  //         .pipe(first())
  //         .subscribe(
  //           data => {
  //             this.router.navigate(['/']);
  //           },
  //           error => {
  //             this.error = error ? error : '';
  //           });
  //     }
  //   }
  // }
  get f() 
  { return this.loginForm.controls; }

  /**
   * Form submit
   */
  onSubmit() {
    this.submitted = true;
var loginformdata ={
  Username : this.loginForm.controls['Username'].value,
   Password : this.loginForm.controls['Password'].value
};

   

    if (this.loginForm.invalid) {
      return;
    } 
    else{

      this.apicallservice.AdminLogin(this.f.Username.value, this.f.Password.value)
              .pipe(first())
              .subscribe(
                data => {
                  this.router.navigate(['/']);
                },
                error => {
                  this.error = error ? error : '';
                });

      // this.apicallservice.AdminLogin(this.f.Username.value, this.f.Password.value).pipe(map(user => {
      //         // login successful if there's a jwt token in the response
      //         if (user) {
      //             // store user details and jwt token in local storage to keep user logged in between page refreshes
      //             localStorage.setItem('currentUser', JSON.stringify(user));
      //            // this.currentUserSubject.next(user);
      //                this.router.navigate(['/']);


      //         }
      //         return user;
      //    });
      
            //  })
               
      // this.apicallservice.AdminLogin(this.f.Username.value, this.f.Password.value).pipe(first()).subscribe(res => {

      //           this.router.navigate(['/']);
      //         })
                // .catchError(error => {
                //   this.error = error ? error : '';
                // });

// this.apicallservice.AdminLogin(loginformdata).subscribe((res)=>{

// console.log(res);

// this.token = res;
// console.log(  this.token);

// if(this.token.Result.access_token != null)
// {
//   console.log(this.token.Result.access_token )
//   localStorage.setItem("token",this.token.token);
//   // localStorage.setItem("IsLoggedIn");
 
//  if(this.token.Result.access_token) {
//   alert("Login successfully");
//   this.router.navigate(['/']);
//  } 
// }


// else {
// alert(this.token.message)
// }

// },
//           error => {
//               this.error = error ? error : '';
//           });  
    }
  }


  //       this.http.post(environment.API_URL_LogIn+'login',val)
  //       .subscribe(res=>{
    
  //         this.token = res;
  //         console.log(  this.token);
    
  //         if(this.token.token != null)
  //         {
  //           console.log(this.token.token)
  //           localStorage.setItem("token",this.token.token);
  //           // localStorage.setItem("IsLoggedIn");
           
  //          if(this.token.token) {
  //           alert("Login successfully");
  //           this.router.navigate(['/']);

  //          // this.router.navigate(['dashboard']);
  //          } 
  //         }
         
    
  //        else {
  //         alert(this.token.message)
  //       }
        
  //       })  
  //     }

  //   // stop here if form is invalid
  //   if (this.loginForm.invalid) {
  //     return;
  //   } 
  //   else {
  //     if (environment.defaultauth === 'firebase') {
  //       this.authenticationService.login(this.f.email.value, this.f.password.value).then((res: any) => {
  //         this.router.navigate(['/']);
  //       })
  //         .catch(error => {
  //           this.error = error ? error : '';
  //         });
  //     } else {
  //       this.authFackservice.login(this.f.email.value, this.f.password.value)
  //         .pipe(first())
  //         .subscribe(
  //           data => {
  //             this.router.navigate(['/']);
  //           },
  //           error => {
  //             this.error = error ? error : '';
  //           });
  //     }
  //   }
  // }

}
