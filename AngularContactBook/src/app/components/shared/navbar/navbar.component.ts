import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { UserDetail } from 'src/app/models/userDetails.model';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent  implements OnInit {
  isAuthenticated:boolean=false;
  username:string |null|undefined;
  imageByte: string | null | undefined = null; 
  constructor(private authService:AuthService,private cdr:ChangeDetectorRef){}
  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
     this.isAuthenticated=authState;
     this.cdr.detectChanges();//manually trigger change detection
    });
    this.authService.getUsername().subscribe((username:string |null|undefined)=>{
     this.username=username;
     this.cdr.detectChanges();
    });
    this.authService.getUserId().subscribe((userId: string | null | undefined) => {
      console.log('UserId changed:', userId);
      if (userId) {
        const userIdNumber = Number(userId); // Convert userId to a number
        this.authService.getUserDetailById(userIdNumber).subscribe(
          (response: ApiResponse<UserDetail>) => {
            console.log('User detail response:', response);
            if (response && response.data) {
              const userDetail = response.data;
              this.imageByte = userDetail.imageByte; // Assuming imageByte is a base64 encoded string
              this.cdr.detectChanges();
            }
          },
          (error) => {
            console.error('Error fetching user details:', error);
          }
        );
      }
      else {
        
        this.imageByte = null; 
        this.cdr.detectChanges(); 
      }
    });
   }
   signOut(){
    this.authService.signOut();  
    this.cdr.detectChanges(); 
    
  }
 
}
