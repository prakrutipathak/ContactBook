import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddContactComponent } from './add-contact.component';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';
import { Router } from '@angular/router';
import { ContactListPaginationComponent } from '../contact-list-pagination/contact-list-pagination.component';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { ContactCountry } from 'src/app/models/country.model';
import { ContactState } from 'src/app/models/state.model';
import { CountryState } from 'src/app/models/country.state.model';

describe('AddContactComponent', () => {
  let component: AddContactComponent;
  let fixture: ComponentFixture<AddContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let countryServiceSpy: jasmine.SpyObj<CountryService>;
  let stateServiceSpy: jasmine.SpyObj<StateService>;
  let routerSpy: Router

  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['createContact']);
    countryServiceSpy = jasmine.createSpyObj('CountryService', ['getAllCountries']);
    stateServiceSpy = jasmine.createSpyObj('StateService', ['GetStatesByCountryId']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      declarations: [AddContactComponent],
      imports:[HttpClientTestingModule,RouterTestingModule.withRoutes([{path:'contactpagination', component: ContactListPaginationComponent}]),FormsModule],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: CountryService, useValue: countryServiceSpy },
        { provide: StateService, useValue: stateServiceSpy },
      ]
    });
    fixture = TestBed.createComponent(AddContactComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
    routerSpy = TestBed.inject(Router)
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should navigate to /contact on successful contact addition', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    contactServiceSpy.createContact.and.returnValue(of(mockResponse));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
       firstName: 'Test name',
        lastName: 'last name',
        countryId: 2,
          stateId: 2,
          email: "Test@gmail.com",
          contactNumber: "1234567891",
          image: '',
          imageByte: "",
          gender: "F",
          favourites: true,
          birthdate: "09-08-2008"
      },
      controls: {
       
        contactId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.contact.stateId = 2; // Ensure this.contact.stateId is set to match form.value.stateId
    component.onSubmit(form);
 
    expect(contactServiceSpy.createContact).toHaveBeenCalledWith(component.contact); // Verify addContact was called with component.contact
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/contactpagination']);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on unsuccessful contact addition', () => {
    spyOn(window, 'alert');
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error adding category' };
    contactServiceSpy.createContact.and.returnValue(of(mockResponse));
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        firstName: 'Test name',
         lastName: 'last name',
         countryId: 2,
           stateId: 2,
           email: "Test@gmail.com",
           contactNumber: "1234567891",
           image: '',
           imageByte: "",
           gender: "F",
           favourites: true,
           birthdate: "09-08-2008"
       },
       controls: {
       
        contactId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.contact.stateId = 2;
    component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on HTTP error', () => {
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.createContact.and.returnValue(throwError(mockError));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        firstName: 'Test name',
         lastName: 'last name',
         countryId: 2,
           stateId: 2,
           email: "Test@gmail.com",
           contactNumber: "1234567891",
           image: '',
           imageByte: "",
           gender: "F",
           favourites: true,
           birthdate: "09-08-2008"
       },
       controls: {
       
        contactId: {value:1}, firstName: {value:'Test name'},
        lastName: {value:'last name'},
        countryId: {value:2},
          stateId:{value: 2},
          email: {value:"Test@gmail.com"},
          contactNumber: {value:"1234567891"},
          image: {value:''},
          imageByte: {value:""},
          gender:{value: "F"},
          favourites: {value:true},
          birthdate:{value: "09-08-2008"}
      }
    };
 
    component.contact.stateId = 2;
    component.onSubmit(form);
 
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
    expect(component.loading).toBe(false);
  });
 
  it('should not call contactService.createContact on invalid form submission', () => {
    const form = <NgForm>{ valid: false };
 
    component.onSubmit(form);
 
    expect(contactServiceSpy.createContact).not.toHaveBeenCalled();
    expect(component.loading).toBe(false);
  });

  it('should load countries on init', () => {
    // Arrange
    
  const mockCountries: ContactCountry[] = [
    { countryId: 1, countryName: 'Category 1'},
    { countryId: 2, countryName: 'Category 2'},
  ];
    const mockResponse: ApiResponse<ContactCountry[]> = { success: true, data: mockCountries, message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
 
    // Act
    component.ngOnInit();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(component.countries).toEqual(mockCountries);
  });
 
  it('should handle failed country loading', () => {
    // Arrange
    const mockResponse: ApiResponse<ContactCountry[]> = { success: false, data: [], message: 'Failed to fetch countries' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
 
    // Act
    component.ngOnInit();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch countries', 'Failed to fetch countries');
  });
 
  it('should handle error during country loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    countryServiceSpy.getAllCountries.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');
 
    // Act
    component.ngOnInit();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching country', mockError);
  });

  it('should load state from country Id', () => {
    // Arrange
    const mockStates: CountryState[] = [
      { countryId: 1, stateName: 'Category 1', stateId: 2,country: {
        countryId: 1,
        countryName: "country 1"
      },},
      { countryId: 2, stateName: 'Category 2', stateId: 1,country: {
        countryId: 2,
        countryName: "country 1"
      },},
    ];
    const mockResponse: ApiResponse<CountryState[]> = { success: true, data: mockStates, message: '' };
    stateServiceSpy.GetStatesByCountryId.and.returnValue(of(mockResponse));
  const countryId = 1;
    // Act
    component.loadStates(countryId) // ngOnInit is called here

    // Assert
    expect(stateServiceSpy.GetStatesByCountryId).toHaveBeenCalledWith(countryId);
    expect(component.states).toEqual(mockStates);
  });

  it('should not load state when response is false', () => {
    // Arrange

    const mockResponse: ApiResponse<CountryState[]> = { success: false, data: [], message: 'Failed to fetch states' };
    stateServiceSpy.GetStatesByCountryId.and.returnValue(of(mockResponse));
    spyOn(console, 'error');

     const countryId = 1;
    // Act
    component.loadStates(countryId) // ngOnInit is called here

    // Assert
    expect(stateServiceSpy.GetStatesByCountryId).toHaveBeenCalledWith(countryId);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch states', 'Failed to fetch states');
  });

  it('should handle error during state loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    stateServiceSpy.GetStatesByCountryId.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');
    const countryId = 1;

    // Act
    component.loadStates(countryId) // ngOnInit is called here
 
    // Assert
    expect(stateServiceSpy.GetStatesByCountryId).toHaveBeenCalledWith(countryId);
    expect(console.error).toHaveBeenCalledWith('Error fetching state', mockError);
  });



});
