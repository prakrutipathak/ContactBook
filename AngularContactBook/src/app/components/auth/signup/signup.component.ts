import { Component, ElementRef, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  user={
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    email: '',
    contactNumber: '',
    password: '',
    confirmPassword: '',
    image:'',
    imageByte:''
};
imageUrl: string | ArrayBuffer | null = null;
loading:boolean=false;
@ViewChild('imageInput') imageInput!: ElementRef;
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
onSubmit(signUpForm:NgForm):void{
  if(signUpForm.valid){
    this.loading=true;
    if (this.imageUrl === null) {
      this.user.imageByte = '';
      this.user.image = '';
    }
    console.log(signUpForm.value);
    this.authService.signup(this.user).subscribe({
      next:(response)=>{
        if(response.success){
          this.router.navigate(['/signupsuccess']);
        }else{
          this.imageUrl = null;
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
onFileChange(event: any): void {
  const file = event.target.files[0];
  if (file) {
    const fileType = file.type;
    if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg') {
      const reader = new FileReader();
      reader.onload = () => {
        this.user.imageByte = (reader.result as string).split(',')[1];
        this.user.image = file.name;
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(file);
    } else {
    this.imageInput.nativeElement.value = '';
      alert('Invalid file format! Please upload an image in JPG, JPEG, or PNG format.');
       
    }
  }}
removeFile() {
  this.imageUrl = null; 
  this.user.imageByte='';
  this.user.image = '';
  this.imageInput.nativeElement.value = '';
}

}
