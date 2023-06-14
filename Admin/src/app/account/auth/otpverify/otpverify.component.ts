import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthenticationService } from '../../../core/services/auth.service';
import { environment } from '../../../../environments/environment';
import { ApicallService } from 'src/app/core/services/apicall.service';

@Component({
  selector: 'app-otpverify',
  templateUrl: './otpverify.component.html',
  styleUrls: ['./otpverify.component.scss']
})
export class OtpverifyComponent implements OnInit {
  OTPVerify: FormGroup;
  submitted = false;
  error = '';
  success = '';
  loading = false;
  otp:any;
  Useremail:any;
  verifydata:any;
  Message:any="Verify your otp";


  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private authenticationService: AuthenticationService,private apiservice:ApicallService) { }

  ngOnInit(): void {
    this.route.queryParams
    .subscribe(params => {
      console.log(params); // { orderby: "price" }
      this.Useremail
       = params.Email;
      console.log(params.Email); // price
    }
  );
    document.body.removeAttribute('data-layout');
    document.body.classList.add('auth-body-bg');

    this.OTPVerify = this.formBuilder.group({
      email: [ this.Useremail, [Validators.required, Validators.email]],
      otp:['', [Validators.required]],
    });
  }
  

  get f() { return this.OTPVerify.controls; }

  onSubmit() {
    //debugger;
    this.submitted = true;
    var verifydata = {
      Useremail : this.f.email.value,
      otp  :this.f.otp.value
    }  
    if (this.OTPVerify.invalid) {
      return;
    } 
    else{

      this.apiservice.OTPverify(this.f.email.value, this.f.otp.value)
              .subscribe(
                data => {
                  console.log(data);
                  if(data.Result!=0)
                  {
                    debugger;
                    this.Message="Otp Verified!";
                    this.router.navigate(['/account/reset-password'],{queryParams:{Email:this.f.email.value}});
                  }
                  else{
                    debugger;
                    this.Message="Wrong OTP";
                    console.log('Wrong OTP');
                  }
                },
                error => {
                  this.error = error ? error : '';
                });
              }
              
            }
          }
