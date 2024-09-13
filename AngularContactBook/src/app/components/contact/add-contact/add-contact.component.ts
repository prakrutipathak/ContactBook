import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddContact } from 'src/app/models/addcontact.model';
import { ContactCountry } from 'src/app/models/country.model';
import { CountryState } from 'src/app/models/country.state.model';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})
export class AddContactComponent implements OnInit{
  
  imageUrl: string | ArrayBuffer | null = null;
  loading:boolean=false;
  @ViewChild('imageInput') imageInput!: ElementRef;
  countries:ContactCountry[]=[];
  states:CountryState[]=[];
  constructor(private contactservice:ContactService,private countryService:CountryService,private stateService:StateService,private router:Router){}
  ngOnInit(): void {
    this.loadCountries();
  }
  contact:AddContact={
    firstName: '',
    lastName: '',
    image: '',
    email: '',
    contactNumber: '',
    address: '',
    gender: '',
    favourite: false,
    countryId: null,
    stateId: null,
    imageByte:'',
    birthDate:'' ,
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
      this.loadStates(this.contact.countryId);
    } else {
      this.states = []; 
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
  onSubmit(myform:NgForm){
    if (myform.valid) {
      this.loading=true;
      if (this.imageUrl === null) {
        this.contact.imageByte = '';
        this.contact.image = '';
      }
      console.log(myform.value);
      this.contactservice.createContact(this.contact)
        .subscribe({
          next:(response)=> {
            if (response.success) {
            console.log("Contact created successfully:", response);
            this.router.navigate(['/contactpagination']);
            }
            else {
              this.imageUrl = null;
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
