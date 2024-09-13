import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactDetailsComponent } from './contact-details.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ContactService } from 'src/app/service/contact.service';
import { ActivatedRoute } from '@angular/router';
import { Contact } from 'src/app/models/contact.model';
import { of } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('ContactDetailsComponent', () => {
  let component: ContactDetailsComponent;
  let fixture: ComponentFixture<ContactDetailsComponent>;
  let contactService: jasmine.SpyObj<ContactService>;
  let route: ActivatedRoute;
  const mockContact: Contact= {
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
  };


  beforeEach(() => {
    const contactServiceSpy = jasmine.createSpyObj('ContactService', ['getContactById']);
    TestBed.configureTestingModule({
      declarations: [ContactDetailsComponent],
      imports:[HttpClientTestingModule,RouterTestingModule],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ contactId: 1 })
          }
        }
      ]
    });
    fixture = TestBed.createComponent(ContactDetailsComponent);
    component = fixture.componentInstance;
    contactService = TestBed.inject(ContactService) as jasmine.SpyObj<ContactService>;
    route = TestBed.inject(ActivatedRoute);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should initialize contactId from route params and load contact details', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactService.getContactById.and.returnValue(of(mockResponse));
 
    // Act
    fixture.detectChanges(); // ngOnInit is called here
 
    // Assert
    expect(component.contactId).toBe(1);
    expect(contactService.getContactById).toHaveBeenCalledWith(1);
    expect(component.contact).toEqual(mockContact);
  });
 
  it('should log "Completed" when contact loading completes', () => {
    // Arrange
    const mockResponse: ApiResponse<Contact> = { success: true, data: mockContact, message: '' };
    contactService.getContactById.and.returnValue(of(mockResponse));
    spyOn(console, 'log');
 
    // Act
    component.loadContactsDetails(1); 
    fixture.detectChanges();
 
    // Assert
    expect(console.log).toHaveBeenCalledWith('Completed');
  });

});
