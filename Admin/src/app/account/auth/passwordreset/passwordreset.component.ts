import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthenticationService } from '../../../core/services/auth.service';
import { AuthfakeauthenticationService } from '../../../core/services/authfake.service';
import { first, map } from 'rxjs/operators';

import { environment } from '../../../../environments/environment';
import { ApicallService } from 'src/app/core/services/apicall.service';

@Component({
  selector: 'app-passwordreset',
  templateUrl: './passwordreset.component.html',
  styleUrls: ['./passwordreset.component.scss']
})

/**
 * Reset-password component
 */
export class PasswordresetComponent implements OnInit{

  resetForm: FormGroup;
  submitted = false;
  error = '';
  returnUrl: string;
  token :any ;
  useremail : any;
  userpassword : any
  cuserpassword : any
  loginformdata :any
  Message : any ="Enter your new password!"
  // set the currenr year
  year: number = new Date().getFullYear();

  // tslint:disable-next-line: max-line-length
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private authenticationService: AuthenticationService,private apiservice:ApicallService) { }

  ngOnInit() {
    this.route.queryParams
    .subscribe(params => {
      console.log("passwordreset param",params); // { orderby: "price" }
      this.useremail
       = params.Email;
      console.log("passwordreset param.email",params.Email); // price
    }
  );

    document.body.removeAttribute('data-layout');
    document.body.classList.add('auth-body-bg');

    this.resetForm = this.formBuilder.group({
      //useremail: ['', [Validators.required, Validators.email]],
      userpassword: ['', [Validators.required]],
      cuserpassword: ['', [Validators.required]],
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.resetForm.controls; }

  /**
   * On submit form
   */
  onSubmit() {
    
   // debugger;
    this.submitted = true;

    // stop here if form is invalid
    if (this.resetForm.invalid) {
     // debugger;
      return;
    }
    var loginformdata ={
      useremail : this.useremail.value,
       userpassword : this.f.userpassword.value
    };

    if (this.resetForm.invalid) {
      return;
    } 
    else{
      debugger;
      if(this.f.userpassword.value == this.f.cuserpassword.value)
      {
        debugger;
      this.apiservice.passwordReset(this.useremail, this.f.userpassword.value)
              .subscribe(
                data => {
                  this.router.navigate(['/']);
                },
                error => {
                  this.error = error ? error : '';
                });
              }
              else{
                debugger;
              this.Message="Password and Conform Password does not Match";
              }
            }
            

            
  }
}
