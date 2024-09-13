import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent {
  forgetpassword={
    userName:'',
    password: '',
    confirmPassword: ''
};
loading:boolean=false;
constructor(private authService:AuthService,private router:Router){}
checkPasswords(form: NgForm):void {
  const password = form.controls['password'];
  const confirmPassword = form.controls['confirmPassword'];

  if (password && confirmPassword && password.value !== confirmPassword.value) {
    confirmPassword.setErrors({ passwordMismatch: true });
  } else {
    confirmPassword.setErrors(null);
  }
}
onSubmit(forgetPasswordForm:NgForm):void{
  if(forgetPasswordForm.valid){
    this.loading=true;
    console.log(forgetPasswordForm.value);
    this.authService.forgetPassword(this.forgetpassword).subscribe({
      next:(response)=>{
        if(response.success){
          this.router.navigate(['/forgetpasswordsuccess']);
        }else{
          alert(response.message);
        }
        this.loading=false;
      },
      error:(err)=>{
        console.log(err.error.message);
        this.loading=false;
        alert(err.error.message);
      },

    });
    
  }
}

}
