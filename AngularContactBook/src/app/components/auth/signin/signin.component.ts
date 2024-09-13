import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { LocalstorageService } from 'src/app/service/helpers/localstorage.service';
import { LocalStorageKeys } from 'src/app/service/helpers/localstoragekeys';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {
  loading:boolean=false;
  username:string='';
  password:string=''
  constructor(private authService:AuthService,private cdr:ChangeDetectorRef,private localStorageHelper:LocalstorageService,private router:Router){}
  login(){
    this.loading=true;
    this.authService.signIn(this.username,this.password).subscribe({
      next:(response)=>{
        if(response.success){
          // this.localStorageHelper.setItem(LocalStorageKeys.TokenName,response.data);
          // this.localStorageHelper.setItem(LocalStorageKeys.LoginId,this.username);
          this.cdr.detectChanges(); //manually trigger change detection
          this.router.navigate(['/contactpagination']);
        }else{
          alert(response.message);
        }
        this.loading=false;
      },
      error:(err)=>{
        this.loading=false;
        alert(err.error.message);
      }
      
    });
  }

}



