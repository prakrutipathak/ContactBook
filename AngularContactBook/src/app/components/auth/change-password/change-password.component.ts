import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit{
  userName:string |null|undefined;
  changepassword={
    userName: '',
    password: '',
    confirmPassword: ''
};
loading:boolean=false;
constructor(private authService:AuthService,private router:Router,private cdr:ChangeDetectorRef){}
  ngOnInit(): void {
    this.authService.getUsername().subscribe((userName:string |null|undefined)=>{
this.userName=userName
    });
  }
checkPasswords(form: NgForm):void {
  const password = form.controls['password'];
  const confirmPassword = form.controls['confirmPassword'];

  if (password && confirmPassword && password.value !== confirmPassword.value) {
    confirmPassword.setErrors({ passwordMismatch: true });
  } else {
    confirmPassword.setErrors(null);
  }
}

onSubmit(changePasswordForm: NgForm): void {
  if (changePasswordForm.valid) {
    if (this.userName) { // Check if userName is not null or undefined
      this.changepassword.userName = this.userName;
      this.loading = true;
      console.log(changePasswordForm.value);
      this.authService.changePassword(this.changepassword).subscribe({
        next: (response) => {
          if (response.success) {
            alert("Password Updated Successfully");
            this.authService.signOut();  
            this.router.navigate(['/signin']);
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error: (err) => {
          console.log(err.error.message);
          this.loading = false;
          alert(err.error.message);
        },
      });
    } else {
      console.error('User name is not available.');
    }
  }
}


}
