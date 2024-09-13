import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavouriteContactComponent } from './favourite-contact.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { ContactService } from 'src/app/service/contact.service';
import { Contact } from 'src/app/models/contact.model';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('FavouriteContactComponent', () => {
  let component: FavouriteContactComponent;
  let fixture: ComponentFixture<FavouriteContactComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;
  const mockContacts: Contact[] = [
    { contactId: 1, firstName: 'Test', lastName:'Test', email: 'Test@gmail.com',contactNumber:'1234567890',gender:'M',address:'pune',favourite:true,countryId:1,stateId:1,image: '',birthDate: '',imageByte:'',country:{countryId :1,countryName:'India'},state:{stateId:1,stateName:'Gujrat',countryId:1} },
    { contactId: 1, firstName: 'Test2', lastName:'Test2', email: 'Test2@gmail.com',contactNumber:'1244567890',gender:'F',address:'pune',favourite:true,countryId:1,stateId:1,image: '',birthDate:'',imageByte:'',country:{countryId :1,countryName:'India'},state:{stateId:1,stateName:'Gujrat',countryId:1} },
    
  ];


  beforeEach(() => {
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['getAllFavouriteContactsCount','getAllFavouriteContactsWithPagination','GetAllFavourite','deleteContact']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);
    TestBed.configureTestingModule({
      declarations: [FavouriteContactComponent],
      imports:[HttpClientTestingModule,RouterTestingModule],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy }
      ],
    });
    fixture = TestBed.createComponent(FavouriteContactComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should calculate total contact count without letter ',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllFavouriteContactsCount.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllFavouriteContactsWithPagination.and.returnValue(of(mockResponseall));

    //Act
    component.getAllContactsCountWithoutLetter();

    //Assert
    expect(contactServiceSpy.getAllFavouriteContactsCount).toHaveBeenCalled();

  })

  it('should handle Http error response',()=>{
    //Arrange
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllFavouriteContactsCount.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadContactsCount();

    //Assert
    expect(contactServiceSpy.getAllFavouriteContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.',mockError);


  })

  it('should calculate total contact count with letter successfully ',()=>{
    //Arrange
    const letter = 'A';
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllFavouriteContactsCount.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllFavouriteContactsWithPagination.and.returnValue(of(mockResponseall));


    //Act
    component.getAllContactsCountWithLetter(letter);

    //Assert
    expect(contactServiceSpy.getAllFavouriteContactsCount).toHaveBeenCalled();
  })

  it('should load all contacts successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.GetAllFavourite.and.returnValue(of(mockResponse));

    //Act
    component.loadAllContacts();

    //Assert
    expect(contactServiceSpy.GetAllFavourite).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should fail to load all contacts',()=>{
    //Arrange
    
    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.GetAllFavourite.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadAllContacts();
    //Assert
    expect(contactServiceSpy.GetAllFavourite).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual([]);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response 4',()=>{
    //Arrange
    
    const mockError = {message:'Network Error'};
    contactServiceSpy.GetAllFavourite.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadAllContacts();

    //Assert
    expect(contactServiceSpy.GetAllFavourite).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual([]);
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts.',mockError);
    expect(component.loading).toBe(false);
  })

  it('should call loadContactsCount and loadAllContacts on initialization', () => {
    // Mocking isAuthenticated to return true
    authServiceSpy.isAuthenticated.and.returnValue(of(true));

    // Spy on component methods
    spyOn(component, 'loadContactsCount');
    spyOn(component, 'loadAllContacts');

    // Call ngOnInit
    component.ngOnInit();   
    // Expectations
    expect(component.loadContactsCount).toHaveBeenCalled();
    expect(component.loadAllContacts).toHaveBeenCalled();
   
  });
  // delete contact
  it('should call confirmDelete and set contactId for deletion', () => {
    // Arrange
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(component, 'deleteContact');
 
    // Act
    component.confirmDelete(1);
 
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure?');
    expect(component.contactId).toBe(1);
    expect(component.deleteContact).toHaveBeenCalled();
  });
 
  it('should not call deleteContact if confirm is cancelled', () => {
    // Arrange
    spyOn(window, 'confirm').and.returnValue(false);
    spyOn(component, 'deleteContact');
 
    // Act
    component.confirmDelete(1);
 
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure?');
    expect(component.deleteContact).not.toHaveBeenCalled();
  });
 
  it('should delete contact and reload contacts', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<string> = { success: true, data: "", message: 'Contact deleted successfully' };
    contactServiceSpy.deleteContact.and.returnValue(of(mockDeleteResponse));
    spyOn(component, 'loadContacts');
 
    // Act
    component.contactId = 1;
    component.deleteContact();
 
    // Assert
    expect(contactServiceSpy.deleteContact).toHaveBeenCalledWith(1);
    expect(component.loadContacts).toHaveBeenCalled();
  });
 
  it('should alert error message if delete contact fails', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<string> = { success: false, data: "", message: 'Failed to delete contact' };
    contactServiceSpy.deleteContact.and.returnValue(of(mockDeleteResponse));
    spyOn(window, 'alert');
 
    // Act
    component.contactId = 1;
    component.deleteContact();
 
    // Assert
    expect(window.alert).toHaveBeenCalledWith('Failed to delete contact');
  });
  it('should alert error message if delete contact throws error', () => {
    // Arrange
    const mockError = { error: { message: 'Delete error' } };
    contactServiceSpy.deleteContact.and.returnValue(throwError(() => mockError));
    spyOn(window, 'alert');
 
    // Act
    component.contactId = 1;
    component.deleteContact();
 
    // Assert
    expect(window.alert).toHaveBeenCalledWith('Delete error');
  });
});
