import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

import { UiModule } from '../../shared/ui/ui.module';
import { AuthRoutingModule } from './auth-routing';

import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { PasswordresetComponent } from './passwordreset/passwordreset.component';
import { OtpverifyComponent } from './otpverify/otpverify.component';
import { SendmailComponent } from './sendmail/sendmail.component';

@NgModule({
  declarations: [LoginComponent, SignupComponent, PasswordresetComponent, OtpverifyComponent, SendmailComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NgbAlertModule,
    UiModule,
    AuthRoutingModule
  ]
})
export class AuthModule { }
