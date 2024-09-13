import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-update-details',
  templateUrl: './update-details.component.html',
  styleUrls: ['./update-details.component.css']
})
export class UpdateDetailsComponent implements OnInit{
  user={
    userId: 0,
    firstName: '',
    lastName: '',
    loginId: '',
    email: '',
    contactNumber: '',
    image:'',
    imageByte:''
};
loading:boolean=false;
@ViewChild('imageInput') imageInput!: ElementRef;
imageUrl: string | ArrayBuffer | null = null;
imageByte2:string='';
constructor(private authService:AuthService,private cdr:ChangeDetectorRef,private router:Router){}
  ngOnInit(): void {
    this.authService.getUserId().subscribe((userId: string | null | undefined) => {
      if (userId) {
      const userIdNumber = Number(userId);
      this.loadUserDetails(userIdNumber); 
      }
      this.cdr.detectChanges(); // Ensure change detection is triggered
    });
     // Assuming you have a method in AuthService to get userId
    
    
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
        this.imageUrl=null;
        this.user.imageByte='';
        this.user.image='';
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
  loadUserDetails(userId:number | undefined):void{
    this.authService.getUserDetailById(userId).subscribe({
      next:(response)=>{
        if(response.success){
          this.user = response.data;
          this.imageUrl='data:image/jpeg;base64,' + this.user.imageByte;
          this.imageByte2=this.user.imageByte;
          this.imageInput.nativeElement.value = this.user.image;
        }else{
          console.error('Failed to fetch user: ',response.message);
        }
      },
      error:(error)=>{
        console.error('Error fetching users: ',error);
      }
    })
  }
  onSubmit(myform:NgForm){
    if (myform.valid) {
      this.loading=true;
      console.log(myform.value);
      this.authService.updateDetails(this.user)
        .subscribe({
          next:(response)=> {
            if (response.success) {
            alert("User Details Updated Successfully");
            this.router.navigate(['/contactpagination']);
            //this.authService.signOut();
            }
            else {
              alert(response.message);
            } 
            this.loading = false;
          },
          error: (err) => {
            console.log(err);
            this.loading = false;
            alert(err.error.message);
          },
          complete: () => {
            this.loading = false;
            console.log('completed');
          }
    });
    }   
    else{
      console.log("model not valid");
    }  

  }

}
