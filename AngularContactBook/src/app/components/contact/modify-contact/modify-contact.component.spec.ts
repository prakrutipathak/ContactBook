import { ComponentFixture, TestBed, tick } from '@angular/core/testing';

import { ModifyContactComponent } from './modify-contact.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ContactService } from 'src/app/service/contact.service';
import { CountryService } from 'src/app/service/country.service';
import { StateService } from 'src/app/service/state.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { CountryState } from 'src/app/models/country.state.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { ContactCountry } from 'src/app/models/country.model';
import { Contact } from 'src/app/models/contact.model';

describe('ModifyContactComponent', () => {
  let component: ModifyContactComponent;
  let fixture: ComponentFixture<ModifyContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let countryServiceSpy: jasmine.SpyObj<CountryService>;
  let stateServiceSpy: jasmine.SpyObj<StateService>;
  let routerSpy: Router;
  let route: ActivatedRoute;
  const mockContact: Contact= {
    contactId: 1,
    firstName: 'Test',
    lastName: 'Test',
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
  };
  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['modifyContact','getContactById']);
    countryServiceSpy = jasmine.createSpyObj('CountryService', ['getAllCountries']);
    stateServiceSpy = jasmine.createSpyObj('StateService', ['GetStatesByCountryId']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      declarations: [ModifyContactComponent],
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: CountryService, useValue: countryServiceSpy },
        { provide: StateService, useValue: stateServiceSpy },
        {provide: ActivatedRoute,
          useValue:{
            params:of({contactId:1})
          }
        }
        
      ]
    });
    fixture = TestBed.createComponent(ModifyContactComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
    routerSpy = TestBed.inject(Router);
    //route = TestBed.inject(ActivatedRoute);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  //country load
  it('should load countries on init', () => {
    // Arrange
    
  const mockCountries: ContactCountry[] = [
    { countryId: 1, countryName: 'Category 1'},
    { countryId: 2, countryName: 'Category 2'},
  ];
    const mockResponse: ApiResponse<ContactCountry[]> = { success: true, data: mockCountries, message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockResponse));
 
    // Act
     fixture.detectChanges();

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
    fixture.detectChanges();
 
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
    fixture.detectChanges();
 
    // Assert
    expect(countryServiceSpy.getAllCountries).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching country', mockError);
  });
//state load
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
  //load contact(get)
  it('should initialize contactId from route params and load contact details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    const mockCountryResponse: ApiResponse<ContactCountry[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
    
    fixture.detectChanges(); // ngOnInit is called here
    
 
    // Assert
    expect(component.contactId).toBe(1);
    expect(contactServiceSpy.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });
 
  it('should log error message if contact loading fails', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: false, data: mockContact, message: 'Failed to fetch contact: ' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    const mockCountryResponse: ApiResponse<ContactCountry[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
    spyOn(console, 'error');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contact: ', 'Failed to fetch contact: ');
  });
 
  it('should alert error message on HTTP error', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    contactServiceSpy.getContactById.and.returnValue(throwError(mockError));
    const mockCountryResponse: ApiResponse<ContactCountry[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(window.alert).toHaveBeenCalledWith('HTTP error');
  });
 
  it('should log "Completed" when contact loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactServiceSpy.getContactById.and.returnValue(of(mockResponse));
    const mockCountryResponse: ApiResponse<ContactCountry[]> = { success: true, data: [],message: '' };
    countryServiceSpy.getAllCountries.and.returnValue(of(mockCountryResponse));
    spyOn(console, 'log');
 
    // Act
    fixture.detectChanges();
 
    // Assert
    expect(console.log).toHaveBeenCalledWith('Completed');
  });


  //onsubmit
  it('should navigate to /contact on successful contact updation', () => {
    spyOn(routerSpy,'navigate');
    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    contactServiceSpy.modifyContact.and.returnValue(of(mockResponse));
 
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
 
    expect(contactServiceSpy.modifyContact).toHaveBeenCalledWith(component.contact); // Verify addContact was called with component.contact
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/contactpagination']);
    expect(component.loading).toBe(false);
  });

  it('should alert error message on unsuccessful contact addition', () => {
    spyOn(window, 'alert');
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error adding category' };
    contactServiceSpy.modifyContact.and.returnValue(of(mockResponse));
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
    contactServiceSpy.modifyContact.and.returnValue(throwError(mockError));
 
    const form = <NgForm><unknown>{
      valid: true,
      value: {
        contactId:1,
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
 
  it('should not call contactService.modifyContact on invalid form submission', () => {
    const form = <NgForm>{ valid: false };
 
    component.onSubmit(form);
 
    expect(contactServiceSpy.modifyContact).not.toHaveBeenCalled();
    expect(component.loading).toBe(false);
  });
});
