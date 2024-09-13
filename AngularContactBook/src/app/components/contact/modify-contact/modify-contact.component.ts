import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactCountry } from 'src/app/models/country.model';
import { CountryState } from 'src/app/models/country.state.model';
import { UpdateContact } from 'src/app/models/update.contact.model';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';

@Component({
  selector: 'app-modify-contact',
  templateUrl: './modify-contact.component.html',
  styleUrls: ['./modify-contact.component.css']
})
export class ModifyContactComponent implements OnInit {
  loading:boolean=false;
  @ViewChild('imageInput') imageInput!: ElementRef;
  contactId:number|undefined;
  imageUrl: string | ArrayBuffer | null = null;
  countries:ContactCountry[]=[];
  states:CountryState[]=[];
  imageByte2:string='';
  constructor(private contactservice:ContactService,private route:ActivatedRoute,private countryService:CountryService,private stateService:StateService,private router:Router){}
  ngOnInit(): void {
    this.route.params.subscribe((params)=>{
      this.contactId = params['contactId'];
      this.loadContactDetails(this.contactId);
    });
    this.loadCountries();
  }
  contact:UpdateContact={
    contactId:0,
    firstName: '',
    lastName: '',
    image: '',
    email: '',
    contactNumber: '',
    imageByte:'',
    birthDate:'',
    address: '',
    gender: '',
    favourite: false,
    countryId: 0,
    stateId: null,
    
  }
  loadCountries():void{
    this.loading=true;
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<ContactCountry[]>)=>{
      if(response.success){
        this.countries=response.data;
      }
      else{
        console.error('Failed to fetch countries',response.message)
      }
      this.loading=false;
    },error:(error)=>{
console.error('Error fetching country',error)
this.loading=false;
    }
    });

  }
  loadStates(countryId:number):void{
    this.loading=true;
    this.stateService.GetStatesByCountryId(countryId).subscribe({
      next:(response: ApiResponse<CountryState[]>)=>{
      if(response.success){
        this.states=response.data;
      }
      else{
        console.error('Failed to fetch states',response.message)
      }
      this.loading=false;
    },error:(error)=>{
console.error('Error fetching state',error)
this.loading=false;
    }
    });

  }
  onCountryChange() {
    if (this.contact.countryId) {
      this.contact.stateId = null; // Reset the state selection
      this.states = [];
      this.loadStates(this.contact.countryId);
    } 
  }
  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const fileType = file.type;
      if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg') {
        const reader = new FileReader();
        reader.onload = () => {
          this.contact.imageByte = (reader.result as string).split(',')[1];
          this.contact.image = file.name;
          this.imageUrl = reader.result;
        };
        reader.readAsDataURL(file);
      } else {
        this.imageUrl=null;
        this.contact.imageByte='';
        this.contact.image='';
        this.imageInput.nativeElement.value = '';
        alert('Invalid file format! Please upload an image in JPG, JPEG, or PNG format.');
         
      }
    }}

  
    removeFile() {
      this.imageUrl = null; 
      this.contact.imageByte='';
      this.contact.image = '';
      this.imageInput.nativeElement.value = '';
  }
  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }
  loadContactDetails(contactId:number | undefined):void{
    this.contactservice.getContactById(contactId).subscribe({
      next:(response)=>{
        if(response.success){
          this.contact = response.data;
          this.contact.birthDate = this.formatDate(new Date(this.contact.birthDate)); 
          this.loadStates(this.contact.countryId);
          this.imageUrl='data:image/jpeg;base64,' + this.contact.imageByte;
          this.imageByte2=this.contact.imageByte;
          this.imageInput.nativeElement.value = this.contact.image;
        }else{
          console.error('Failed to fetch contact: ',response.message);
        }
      },
      error: (err) => {
        console.log(err);
        this.loading = false;
        alert(err.error.message);
      },
      complete: () => {
        this.loading = false;
        console.log('Completed');
      }
    })
  }
  onSubmit(myform:NgForm){
    if (myform.valid) {
      this.loading=true;
      console.log(myform.value);
      this.contactservice.modifyContact(this.contact)
        .subscribe({
          next:(response)=> {
            if (response.success) {
            console.log("Contact updated successfully:", response);
            this.router.navigate(['/contactpagination']);
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

  }

}
