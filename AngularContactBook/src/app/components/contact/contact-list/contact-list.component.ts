import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Contact } from 'src/app/models/contact.model';
import { AuthService } from 'src/app/service/auth.service';
import { ContactService } from 'src/app/service/contact.service';


@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent {
  contacts:Contact[] | undefined;
  contactId:number |undefined;
  isAuthenticated:boolean=false;
  username:string |null|undefined;

  loading:boolean=false;
  constructor(private contactservice:ContactService,private router: Router,private authService:AuthService,private cdr:ChangeDetectorRef){}
  ngOnInit(): void {
    this.loadContacts();
    this.authService.isAuthenticated().subscribe((authState:boolean)=>{
      this.isAuthenticated=authState;
      this.cdr.detectChanges();
     });
     this.authService.getUsername().subscribe((username:string |null|undefined)=>{
      this.username=username;
      this.cdr.detectChanges();
     });
      }
      loadContacts():void{
        this.loading=true;
        this.contactservice.getAllContacts().subscribe({
          next:(response: ApiResponse<Contact[]>)=>{
          if(response.success){
            this.contacts=response.data;
          }
          else{
            console.error('Failed to fetch contacts',response.message)
          }
          this.loading=false;
        },error:(error)=>{
    console.error('Error fetching contact',error)
    this.loading=false;
        }
        });

      }
      confirmDelete(id:number):void{
        if(confirm('Are you sure?')){
          this.contactId = id;
          this.deleteContact();
        }
      }
      
      deleteContact():void{
        this.contactservice.deleteContact(this.contactId).subscribe({
          next:(response)=>{
            if(response.success){
              
              this.loadContacts();
            }else{
              alert(response.message);
            }
          },
          error:(err)=>{
            alert(err.error.message);
          },
          complete:()=>{
            console.log('completed');
          }
        })
        this.router.navigate(['/contacts']);
      }

}
