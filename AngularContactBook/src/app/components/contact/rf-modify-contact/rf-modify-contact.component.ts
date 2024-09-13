import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactCountry } from 'src/app/models/country.model';
import { CountryState } from 'src/app/models/country.state.model';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';

@Component({
  selector: 'app-rf-modify-contact',
  templateUrl: './rf-modify-contact.component.html',
  styleUrls: ['./rf-modify-contact.component.css']
})
export class RfModifyContactComponent implements OnInit{
  imageUrl: string | ArrayBuffer | null = null;
  loading: boolean = false;
  @ViewChild('imageInput') imageInput!: ElementRef;
fileSizeExceeded =  false;
fileFormatInvalid =  false;
  countries:ContactCountry []=[];
  states:CountryState[]=[];
  contactForm!:FormGroup;
  constructor(private contactService:ContactService,  private route: ActivatedRoute, private datePipe: DatePipe ,private stateService:StateService,private fb:FormBuilder,private router: Router,private countryService:CountryService){}
  ngOnInit(): void {
    this.contactForm=this.fb.group({
      contactId: [0],
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
    this.getContact();
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
      imageByte: '',
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
getContact():void{
  const contactId = Number(this.route.snapshot.paramMap.get('contactId'));
  this.contactService.getContactById(contactId).subscribe({
    next: (response) => {

      if (response.success) {
        const formattedBirthDate = this.datePipe.transform(response.data.birthDate, 'yyyy-MM-dd');
        this.contactForm.patchValue({
          contactId: response.data.contactId,
          countryId: response.data.countryId,
          stateId: response.data.stateId,
          firstName : response.data.firstName,
          lastName : response.data.lastName,
          address : response.data.address,
          contactNumber : response.data.contactNumber,
          email : response.data.email,
          gender : response.data.gender,
          favourite : response.data.favourite,
          image: response.data.image,
          imageByte: response.data.imageByte,
          birthDate: formattedBirthDate      
        });
        console.log(formattedBirthDate)
        console.log(response.data)
         // Check if the response contains imageByte data
         if (response.data.imageByte) {
          // Set imageUrl to display the image
          this.imageUrl = 'data:image/jpeg;base64,' + response.data.imageByte;
      }
      } else {
        console.error('Failed to fetch contacts', response.message);
      }
    },
    error: (error) => {
      alert(error.error.message);
      this.loading = false;
    },
    complete: () => {
      this.loading = false;
    },
  });
  }

  onSubmit(){
    this.loading = true;
    if(this.contactForm.valid){
      console.log(this.contactForm.value);
      this.contactService.modifyContact(this.contactForm.value)
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
