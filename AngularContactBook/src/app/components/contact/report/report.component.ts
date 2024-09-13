import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactCountry } from 'src/app/models/country.model';
import { CountryState } from 'src/app/models/country.state.model';
import { ContactSP } from 'src/app/models/spcontact.model';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  loading:boolean=false;
  month:number |undefined;
  selectedMonth: number = 0;
  selectedstate: number = 0;
  selectedcountry: number = 0;
  selectedgender: string = "";
  countries:ContactCountry[]=[];
  states:CountryState[]=[];
  contactCount: number | null=null;  

  months: { value: number, name: string }[] = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];

  constructor(private contactservice:ContactService,private router: Router,private countryService:CountryService,private stateService:StateService){}
  ngOnInit(): void {
    this.loadCountries();
    this.loadStates();
    
  }
  private fetchData(): void {
    this.loading = true;
    if (this.selectedMonth > 0) {
      this.GetDetailByBirthMonth(this.selectedMonth);
    } else if (this.selectedstate > 0) {
      this.GetDetailByStateId(this.selectedstate);
    } else if (this.selectedcountry > 0) {
      this.CountContactBasedOnCountry(this.selectedcountry);
    } else if (this.selectedgender) {
      this.CountContactBasedOnGender(this.selectedgender);
    } else {
      this.loading = false;
      this.contacts = null;
      this.contactCount = null;
    }
  }
  contacts:ContactSP[] | null=null;
  // month dropdown
  onMonthChange(event: any): void {
    this.selectedcountry = 0;
    this.selectedstate = 0;
    this.selectedgender = "";
    this.contactCount=null;
    this.selectedMonth = +event.target.value; // Convert the string to number
    console.log('Selected Month Number:', this.selectedMonth);
    this.fetchData();
    
  }
  onStateChange(event: any): void {
    this.selectedcountry = 0;
    this.selectedMonth = 0;
    this.selectedgender = "";
    this.selectedstate = +event.target.value; // Convert the string to number
   
    console.log('Selected state Number:', this.selectedstate);
    this.fetchData();
  }
  onCountryChange(event: any): void {
    this.selectedMonth = 0;
    this.selectedstate = 0;
    this.selectedgender = "";
    this.selectedcountry = +event.target.value; // Convert the string to number
    console.log('Selected country Number:', this.selectedcountry);
   
    this.fetchData();
  }
  onGenderChange(event: any): void {
    this.selectedcountry = 0;
    this.selectedstate = 0;
    this.selectedMonth = 0;
    this.selectedgender = event.target.value; // Convert the string to number
    console.log('Selected country Number:', this.selectedgender);
    
    this.fetchData();
  }
 
 
  //load data on selection of month
  GetDetailByBirthMonth(month:number|undefined):void{
   
    this.loading=true;
    this.contactservice.GetDetailByBirthMonth(month).subscribe({
      next:(response: ApiResponse<ContactSP[]>)=>{
      if(response.success){
        this.contacts=response.data;
        this.contactCount = null;
      }
      else{
        console.error('Failed to fetch contacts',response.message)
        this.contacts = null;
      }
      this.loading=false;
    },error:(error)=>{
console.error('Error fetching contact',error)
this.contacts = null;
this.loading=false;
    }
    });

  }
  //load data on selection of state
  GetDetailByStateId(stateId:number|undefined):void{
    if (stateId === undefined) {
      return; // Do nothing if month is undefined
    }
    this.loading=true;
    this.contactservice.GetDetailByStateId(stateId).subscribe({
      next:(response: ApiResponse<ContactSP[]>)=>{
      if(response.success){
        this.contacts=response.data;
        this.contactCount = null;
      }
      else{
        this.contacts = null;
        console.error('Failed to fetch contacts',response.message)
      }
      this.loading=false;
    },error:(error)=>{
console.error('Error fetching contact',error)
this.contacts = null;
this.loading=false;
    }
    });

  }
  //count on selection of countryid
  CountContactBasedOnCountry(countryId:number):void{
    this.contactservice.CountContactBasedOnCountry(countryId).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          this.contactCount = response.data;
          console.log('Contact Count:', this.contactCount);
          this.contacts = null;
        } else {
          console.error('Failed to fetch contacts count', response.message);
          this.contactCount = 0;
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.contactCount = 0; 
        this.loading = false;
      }
    });

  }
  //count on selection of gender
  CountContactBasedOnGender(gender:string):void{
    this.contactservice.CountContactBasedOnGender(gender).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          this.contactCount = response.data;
          this.contacts = null;
          console.log('Contact Count:', this.contactCount);
        } else {
          console.error('Failed to fetch contacts count', response.message);
          this.contactCount = 0;
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching contacts count.', error);
        this.contactCount = 0;
        this.loading = false;
      }
    });

  }
  //load country and state
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
  loadStates():void{
    this.loading=true;
    this.stateService.getAllStates().subscribe({
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
 
  

}
