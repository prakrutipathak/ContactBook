import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { ContactService } from 'src/app/service/contact.service';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.css']
})
export class ContactDetailsComponent implements OnInit{
  contactId:number|undefined;
  contact:Contact={
    contactId: 0,
    firstName: '',
    lastName: '',
    image: '',
    email: '',
    contactNumber: '',
    address: '',
    gender: '',
    favourite: false,
    imageByte:'',
    birthDate:'',
    countryId: 0,
    country: {
      countryId: 0,
      countryName: '',
    },
    stateId: 0,
    state: {
      stateId: 0,
      stateName: '',
      countryId: 0,
    },
  }
  
  constructor(private contactService:ContactService,private route:ActivatedRoute){}
  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.contactId=params['contactId'];
      this.loadContactsDetails(this.contactId);
     });
  }
  formatDate(date: Date): string {
    if (!date) {
      return ''; // or any default value that makes sense in your application
    }
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }
  loadContactsDetails(contactId:number |undefined):void{
    this.contactService.getContactById(contactId).subscribe({
      next:(response)=>{
        if(response.success){
          this.contact=response.data;
          if (this.contact.birthDate) {
            this.contact.birthDate = this.formatDate(new Date(this.contact.birthDate));
          }
          
        }else{
          console.error('Failed to fetch',response.message);
        }
        
      },
      error:(err)=>{
        alert(err.error.message);
      },
      complete:()=>{
        console.log('Completed');
      }

    });
  }
}
