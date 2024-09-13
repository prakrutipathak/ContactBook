import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContactListPaginationComponent } from './contact-list-pagination.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ContactService } from 'src/app/service/contact.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { ChangeDetectorRef } from '@angular/core';
import { Contact } from 'src/app/models/contact.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';

describe('ContactListPaginationComponent', () => {
  let component: ContactListPaginationComponent;
  let fixture: ComponentFixture<ContactListPaginationComponent>;
  let contactServiceSpy: jasmine.SpyObj<ContactService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;
  const mockContacts: Contact[] = [
    { contactId: 1, firstName: 'Test', lastName:'Test', email: 'Test@gmail.com',contactNumber:'1234567890',gender:'M',address:'pune',favourite:true,countryId:1,stateId:1,image: '',birthDate: '',imageByte:'',country:{countryId :1,countryName:'India'},state:{stateId:1,stateName:'Gujrat',countryId:1} },
    { contactId: 1, firstName: 'Test2', lastName:'Test2', email: 'Test2@gmail.com',contactNumber:'1244567890',gender:'F',address:'pune',favourite:true,countryId:1,stateId:1,image: '',birthDate:'',imageByte:'',country:{countryId :1,countryName:'India'},state:{stateId:1,stateName:'Gujrat',countryId:1} },
    
  ];
  beforeEach(() => {
    
    contactServiceSpy = jasmine.createSpyObj('ContactService', ['getAllContactsCount','getAllContactsWithPagination','getAllContacts','deleteContact']);
    authServiceSpy = jasmine.createSpyObj('AuthService', ['isAuthenticated']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);
    TestBed.configureTestingModule({
      declarations: [ContactListPaginationComponent],
      imports:[HttpClientTestingModule,RouterTestingModule,FormsModule],
      providers: [
        { provide: ContactService, useValue: contactServiceSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy }
      ],
    });
    fixture = TestBed.createComponent(ContactListPaginationComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should calculate total contact count without letter without search successfully',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponseall));

    //Act
    component.getAllContactsCountWithLetter("","");

    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();

  })
  it('should calculate total contact count with out letter successfully with search',()=>{
    //Arrange
    const search='e'
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponseall));

    //Act
    component.getAllContactsCountWithLetter('',search);

    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();

  })

  it('should fail to calculate total count without letter without search',()=>{
    //Arrange
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsCountWithoutLetter();
    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count','Failed to fetch contacts count');

  })
  it('should fail to calculate total count without letter with search',()=>{
    //Arrange
    const search='e'
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsCountWithLetter('',search);
    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count','Failed to fetch contacts count');

  })

  it('should handle Http error response',()=>{
    //Arrange
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllContactsCount.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadContactsCount();

    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.',mockError);


  })

  it('should calculate total contact count with letter successfully without search',()=>{
    //Arrange
    const letter = 'A';
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponseall));


    //Act
    component.getAllContactsCountWithLetter('',letter);

    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
  })
  it('should calculate total contact count with letter successfully with letter with search',()=>{
    //Arrange
    const letter = 'A';
    const search='e'
    const mockResponse :ApiResponse<number> ={success:true,data:2,message:''};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    const mockResponseall :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponseall));

    //Act
    component.getAllContactsCountWithLetter(letter,search);
    //fixture.detectChanges();

    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
  })

  it('should fail to calculate total count with letter without search ',()=>{
    //Arrange
    const letter = 'A';
    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsCountWithLetter(letter,'');
    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count','Failed to fetch contacts count');

  })
  it('should fail to calculate total count with letter with search ',()=>{
    //Arrange
    const letter = 'A';
    const search='e'

    const mockResponse :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsCountWithLetter(letter, search);
    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts count','Failed to fetch contacts count');

  })

  it('should handle Http error response 1',()=>{
    //Arrange
    const letter = 'A';
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllContactsCount.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.getAllContactsCountWithLetter(letter,'');

    //Assert
    expect(contactServiceSpy.getAllContactsCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts count.',mockError);
    expect(component.loading).toBe(false);


  })

  it('should load contacts without letter without search successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));
    const mockResponsecount :ApiResponse<number> ={success:false,data:0,message:'Failed to fetch contacts count'};
    contactServiceSpy.getAllContactsCount.and.returnValue(of(mockResponsecount));
    const mockResponseload :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContacts.and.returnValue(of(mockResponseload));

    //Act
    component.totalPages=100;
    //component.totalPages=Math.ceil(component.totalItems / component.pageSize);
    component.getAllContactsWithPagination();
    //fixture.detectChanges();

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalledWith(component.pageNumber,component.pageSize,' ',component.sort,'');
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })
  it('should load contacts without letter with search successfully',()=>{
    //Arrange
    const search='e'

    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));

    //Act
    component.getAllContactsWithLetter('',search);

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })


  it('should fail to load contacts  without letter with search',()=>{
    //Arrange
    const search='e'

    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsWithLetter('',search);
    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })


  it('should load contacts with letter successfully without search',()=>{
    //Arrange
   const letter = 'R';
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));

    //Act
    component.getAllContactsWithLetter(letter,'');

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })
  it('should load contacts with letter successfully with search',()=>{
    //Arrange
    const search='e'

   const letter = 'R';
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));

    //Act
    component.getAllContactsWithLetter(letter,search);

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should fail to load contacts  with letter without search',()=>{
    //Arrange
    const letter = 'R';
    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsWithLetter(letter,'');
    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should fail to load contacts  with letter with search',()=>{
    //Arrange
    const letter = 'R';
    const search='e'

    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.getAllContactsWithLetter(letter,search);
    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response 3',()=>{
    //Arrange
    const letter = 'R';
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllContactsWithPagination.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.getAllContactsWithLetter(letter,'');

    //Assert
    expect(contactServiceSpy.getAllContactsWithPagination).toHaveBeenCalled();
    expect(component.contacts).toEqual(undefined);
    expect(console.error).toHaveBeenCalledWith('Error fetching contacts.',mockError);
    expect(component.loading).toBe(false);
  })


  it('should load all contacts successfully',()=>{
    //Arrange
   
    const mockResponse :ApiResponse<Contact[]> ={success:true,data:mockContacts,message:''};
    contactServiceSpy.getAllContacts.and.returnValue(of(mockResponse));

    //Act
    component.loadAllContacts();

    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual(mockContacts);
    expect(component.loading).toBe(false);
  })

  it('should fail to load all contacts',()=>{
    //Arrange
    
    const mockResponse :ApiResponse<Contact[]> ={success:false,data:[],message:'Failed to fetch contacts'};
    contactServiceSpy.getAllContacts.and.returnValue(of(mockResponse));
    spyOn(console,'error')
    //Act
    component.loadAllContacts();
    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
    expect(component.contactsForInitial).toEqual([]);
    expect(component.loading).toBe(false);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch contacts','Failed to fetch contacts');

  })

  it('should handle Http error response 4',()=>{
    //Arrange
    
    const mockError = {message:'Network Error'};
    contactServiceSpy.getAllContacts.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.loadAllContacts();

    //Assert
    expect(contactServiceSpy.getAllContacts).toHaveBeenCalled();
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
  //delete contact
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
