import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PrivacyComponent } from './components/privacy/privacy.component';

import { SigninComponent } from './components/auth/signin/signin.component';
import { SignupComponent } from './components/auth/signup/signup.component';
import { ContactListComponent } from './components/contact/contact-list/contact-list.component';
import { HomeComponent } from './components/home/home.component';
import { AddContactComponent } from './components/contact/add-contact/add-contact.component';
import { ModifyContactComponent } from './components/contact/modify-contact/modify-contact.component';
import { ContactDetailsComponent } from './components/contact/contact-details/contact-details.component';
import { SignupSuccessComponent } from './components/auth/signup-success/signup-success.component';
import { authGuard } from './guards/auth.guard';
import { ContactListPaginationComponent } from './components/contact/contact-list-pagination/contact-list-pagination.component';
import { FavouriteContactComponent } from './components/contact/favourite-contact/favourite-contact.component';
import { ForgetPasswordComponent } from './components/auth/forget-password/forget-password.component';
import { ForgetPasswordSuccessComponent } from './components/auth/forget-password-success/forget-password-success.component';
import { ChangePasswordComponent } from './components/auth/change-password/change-password.component';
import { UpdateDetailsComponent } from './components/auth/update-details/update-details.component';
import { RfAddContactComponent } from './components/contact/rf-add-contact/rf-add-contact.component';
import { RfModifyContactComponent } from './components/contact/rf-modify-contact/rf-modify-contact.component';
import { ReportComponent } from './components/contact/report/report.component';

const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home',component:HomeComponent},
  {path:'privacy',component:PrivacyComponent},
  {path:'contacts',component:ContactListComponent},
  {path:'contactpagination',component:ContactListPaginationComponent},
  {path:'favouritecontact',component:FavouriteContactComponent},
  {path:'signin',component:SigninComponent},
  {path:'signup',component:SignupComponent},
  {path:'signupsuccess',component:SignupSuccessComponent},
  {path:'addcontact',component:AddContactComponent,canActivate:[authGuard]},
  {path:'modifycontact/:contactId',component:ModifyContactComponent,canActivate:[authGuard]},
  {path:'rfaddcontact',component:RfAddContactComponent,canActivate:[authGuard]},
  {path:'rfmodifycontact/:contactId',component:RfModifyContactComponent,canActivate:[authGuard]},
  {path:'contactdetails/:contactId',component:ContactDetailsComponent},
  {path:'forgetpassword',component:ForgetPasswordComponent},
  {path:'forgetpasswordsuccess',component:ForgetPasswordSuccessComponent},
  {path:'changepassword',component:ChangePasswordComponent,canActivate:[authGuard]},
  {path:'userupdatedetails',component:UpdateDetailsComponent,canActivate:[authGuard]},
  {path:'report',component:ReportComponent},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
