import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactCountry } from 'src/app/models/country.model';
import { CountryState } from 'src/app/models/country.state.model';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';

@Component({
  selector: 'app-rf-add-contact',
  templateUrl: './rf-add-contact.component.html',
  styleUrls: ['./rf-add-contact.component.css']
})
export class RfAddContactComponent implements OnInit {
  imageUrl: string | ArrayBuffer | null = null;
  loading: boolean = false;
  @ViewChild('imageInput') imageInput!: ElementRef;
fileSizeExceeded =  false;
fileFormatInvalid =  false;
  countries:ContactCountry []=[];
  states:CountryState[]=[];
  contactForm!:FormGroup;
  constructor(private contactService:ContactService,private stateService:StateService,private fb:FormBuilder,private router: Router,private countryService:CountryService){}
  ngOnInit(): void {
    this.contactForm=this.fb.group({
      firstName: ['',[Validators.required, Validators.minLength(2)]],
      lastName: ['',[Validators.required, Validators.minLength(2)]],
      address: ['', Validators.required],
      contactNumber: ['',[Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      countryId : [0, [Validators.required, this.contactValidator]],
      stateId : [0, [Validators.required, this.contactValidator]],
      email: ['',[Validators.required, Validators.email]],
      gender: [, Validators.required],
      favourite: [false],
      imageByte: [''],
      image: [null],
      birthDate: [,[this.validateBirthdate]]
    });
   this.loadCountries();
   this.fetchStateByCountry();
  }
  get formControl(){
    return this.contactForm.controls;
  }
  contactValidator(controls:any){
    return controls.value=='' ? {invalidCountry:true}:null;
  }
  validateBirthdate(control: AbstractControl): ValidationErrors | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();
  
    // Set hours, minutes, seconds, and milliseconds to 0 to compare only the date part
    selectedDate.setHours(0, 0, 0, 0);
    currentDate.setHours(0, 0, 0, 0);
  
    if (selectedDate > currentDate) {
      return { invalidBirthDate: true };
    }
    return null;
   }
  
  loadCountries():void{
    this.loading = true; 
    this.countryService.getAllCountries().subscribe({
      next:(response: ApiResponse<ContactCountry[]>)=>{
        if(response.success){
          this.countries = response.data;
        }else{
          console.error('Failed to fetch countries ',response.message);
        }
        this.loading = false;
      },
      error:(error)=>{
        console.error('Error fetching countries: ',error);
        this.loading = false;
      }
    });
  }
  fetchStateByCountry(): void {
    this.contactForm.get('countryId')?.valueChanges.subscribe((countryId: number) => {
      if (countryId !== 0) {
      this.states = [];
      this.contactForm.get('stateId')?.setValue(null); // Reset the state control's value to null

        this.loading = true;
        this.stateService.GetStatesByCountryId(countryId).subscribe({
          next: (response: ApiResponse<CountryState[]>) => {
            if (response.success) {
              this.states = response.data;
            } else {
              console.error('Failed to fetch states', response.message);
            }
            this.loading = false;
          },
          error: (error) => {
            console.error('Failed to fetch states', error);
            this.loading = false;
          }
        });
      }
    });
  }
  removeFile() {
    this.contactForm.patchValue({
      imageUrl: null,
      image: null
    });
    this.imageUrl = null; // Also reset imageUrl to remove the displayed image
    const inputElement = document.getElementById('imageByte') as HTMLInputElement;
    inputElement.value = '';
     
  }
  
  onFileChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const fileType = file.type; // Get the MIME type of the file
      if (fileType === 'image/jpeg' || fileType === 'image/png' || fileType === 'image/jpg')
         {
          if(file.size > 50 * 1024)
            {
              this.fileSizeExceeded = true; // Set flag to true if file size exceeds the limit
              const inputElement = document.getElementById('imageByte') as HTMLInputElement;
              inputElement.value = '';
              return;
            }
            this.fileFormatInvalid = false;
            this.fileSizeExceeded = false; 

      const reader = new FileReader();
      reader.onload = () => {
        this.contactForm.patchValue({
          imageByte: (reader.result as string).split(',')[1],
          image: file.name
        });
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(file);
    }
    else {
      this.fileFormatInvalid = true;
      const inputElement = document.getElementById('imageByte') as HTMLInputElement;
      inputElement.value = '';

       
    }

   
  }
}
  onSubmit(){
    this.loading = true;
    if(this.contactForm.valid){
      if(this.imageUrl === null)
        {
          this.contactForm.patchValue({
              imageByte : '',
              image: null
          })
        }
      console.log(this.contactForm.value);
      this.contactService.createContact(this.contactForm.value)
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
          this.loading = false;
          alert(err.error.message);
        },
        complete: () => {
          this.loading = false;
          console.log('Completed');
        }
  });
}
  }


}
