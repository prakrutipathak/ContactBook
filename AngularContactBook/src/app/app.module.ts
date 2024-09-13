import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SignupComponent } from './components/auth/signup/signup.component';

import { PrivacyComponent } from './components/privacy/privacy.component';
import { SigninComponent } from './components/auth/signin/signin.component';

import {HttpClientModule} from "@angular/common/http";
import { HomeComponent } from './components/home/home.component';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { ModifyContactComponent } from './components/contact/modify-contact/modify-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SignupSuccessComponent } from './components/auth/signup-success/signup-success.component';
import { ContactListPaginationComponent } from './components/contact/contact-list-pagination/contact-list-pagination.component';
import { FavouriteContactComponent } from './components/contact/favourite-contact/favourite-contact.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { ForgetPasswordComponent } from './components/auth/forget-password/forget-password.component';
import { ForgetPasswordSuccessComponent } from './components/auth/forget-password-success/forget-password-success.component';
import { ChangePasswordComponent } from './components/auth/change-password/change-password.component';
import { UpdateDetailsComponent } from './components/auth/update-details/update-details.component';
import { RfAddContactComponent } from './components/contact/rf-add-contact/rf-add-contact.component';
import { RfModifyContactComponent } from './components/contact/rf-modify-contact/rf-modify-contact.component';
import { DatePipe } from '@angular/common';
import { ReportComponent } from './components/contact/report/report.component';
@NgModule({
  declarations: [
    AppComponent,
    SignupComponent,
    HomeComponent,
    PrivacyComponent,
    SigninComponent,
    ContactListComponent,
    AddContactComponent,
    ModifyContactComponent,
    ContactDetailsComponent,
    SignupSuccessComponent,
    ContactListPaginationComponent,
    FavouriteContactComponent,
    NavbarComponent,
    FooterComponent,
    ForgetPasswordComponent,
    ForgetPasswordSuccessComponent,
    ChangePasswordComponent,
    UpdateDetailsComponent,
    RfAddContactComponent,
    RfModifyContactComponent,
    ReportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
