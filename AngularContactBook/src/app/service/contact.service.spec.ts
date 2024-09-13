import { TestBed } from '@angular/core/testing';

import { ContactService } from './contact.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Contact } from '../models/contact.model';
import { AddContact } from '../models/addcontact.model';
import { UpdateContact } from '../models/update.contact.model';
describe('ContactService', () => {
  let service: ContactService;
  let httpMock:HttpTestingController;
  const mockApiResponse:ApiResponse<Contact[]>={
    success:true,
    data:[
      {
      contactId: 1,
      firstName: "Prakruti",
      lastName: "Pathak",
      image: "Screenshot 2024-05-17 172755.png",
      email: "string@email.com",
      contactNumber: "1234567890",
      address: "Vadodaraa",
      gender: "F",
      favourite: true,
      birthDate:"",
      imageByte:"",
      countryId: 1,
      stateId:1,
      country: {
        countryId: 1,
        countryName: "India",  
      },
      state: {
        stateId: 1,
        stateName: "Gujarat",
        countryId: 1,
      }
    },
    {
      contactId: 1,
      firstName: "Prakruti",
      lastName: "Pathak",
      image: "Screenshot 2024-05-17 172755.png",
      email: "string@email.com",
      contactNumber: "1234567890",
      address: "Vadodaraa",
      gender: "F",
      favourite: true,
      countryId: 1,
      birthDate:"",
      imageByte:"",
      stateId:1,
      country: {
        countryId: 1,
        countryName: "India",  
      },
      state: {
        stateId: 1,
        stateName: "Gujarat",
        countryId: 1,
      }
    }

    ],
    message:''
  }
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[ContactService]
    });
    service = TestBed.inject(ContactService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });
  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  // get all contact
  it('should fetch all contacts successfully',()=>{
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetAllContacts';
    //Act
    service.getAllContacts().subscribe((response)=>{
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });
  it('should handle an empty contacts list',()=>{
    const apiUrl='http://localhost:5294/api/Contact/GetAllContacts';
    const emptyResponse:ApiResponse<Contact[]>={
      success:true,
      data:[],
      message:''
    }
    //Act
    service.getAllContacts().subscribe((response)=>{
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual(emptyResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);


  });
  it('should handle Http error gracefully',()=>{
    const apiUrl='http://localhost:5294/api/Contact/GetAllContacts';
    const errorMessage='Failed to load categories';
    //Act
    service.getAllContacts().subscribe( 
      ()=>fail('expected an error, not contact'),
      (error)=>{
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }    
    );
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
   // Respond with error
   req.flush(errorMessage,{status:500,statusText:'Internal Server Error'})
  });
  // Add contact
  it('should add a contacts successfully',()=>{
    //arrange
    const addContact:AddContact={
      "firstName": "string",
      "lastName": "string",
      "image": "string",
      "email": "string",
      "contactNumber": "string",
      "address": "string",
      "gender": "string",
      "favourite": true,
      "countryId": 0,
      "stateId": 0,
      "imageByte": "string",
      "birthDate": "2024-06-09T17:05:35.619Z"
    }
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"Contact saved successfully.",
      data:""
    }
    //act
    service.createContact(addContact).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed contact addition',()=>{
    //arrange
    const addContact:AddContact={
      "firstName": "string",
      "lastName": "string",
      "image": "string",
      "email": "string",
      "contactNumber": "string",
      "address": "string",
      "gender": "string",
      "favourite": true,
      "countryId": 0,
      "stateId": 0,
      "imageByte": "string",
      "birthDate": "2024-06-09T17:05:35.619Z"
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"Category already exists.",
      data:""
    }
    //act
    service.createContact(addContact).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while adding contact',()=>{
    //arrange
    const addContact:AddContact={
      "firstName": "string",
      "lastName": "string",
      "image": "string",
      "email": "string",
      "contactNumber": "string",
      "address": "string",
      "gender": "string",
      "favourite": true,
      "countryId": 0,
      "stateId": 0,
      "imageByte": "string",
      "birthDate": "2024-06-09T17:05:35.619Z"
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.createContact(addContact).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Create');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
  // update contact
  it('should update a contacts successfully',()=>{
    //arrange
    const updateContact:UpdateContact={
      "contactId":1,
      "firstName": "string",
      "lastName": "string",
      "image": "string",
      "email": "string",
      "contactNumber": "string",
      "address": "string",
      "gender": "string",
      "favourite": true,
      "countryId": 0,
      "stateId": 0,
      "imageByte": "string",
      "birthDate": "2024-06-09T17:05:35.619Z"
    }
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"Contact update successfully.",
      data:""
    }
    //act
    service.modifyContact(updateContact).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Edit');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed contact update',()=>{
    //arrange
    const updateContact:UpdateContact={
      "contactId":1,
      "firstName": "string",
      "lastName": "string",
      "image": "string",
      "email": "string",
      "contactNumber": "string",
      "address": "string",
      "gender": "string",
      "favourite": true,
      "countryId": 0,
      "stateId": 0,
      "imageByte": "string",
      "birthDate": "2024-06-09T17:05:35.619Z"
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"Category already exists.",
      data:""
    }
    //act
    service.modifyContact(updateContact).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Edit');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while adding contact',()=>{
    //arrange
    const updateContact:UpdateContact={
      "contactId":1,
      "firstName": "string",
      "lastName": "string",
      "image": "string",
      "email": "string",
      "contactNumber": "string",
      "address": "string",
      "gender": "string",
      "favourite": true,
      "countryId": 0,
      "stateId": 0,
      "imageByte": "string",
      "birthDate": "2024-06-09T17:05:35.619Z"
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.modifyContact(updateContact).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Edit');
    expect(req.request.method).toBe('PUT');
    req.flush({},mockHttpError);

  });
  // Fetch Contact Details bu Id
  it('should fetch a contact by id successfully',()=>{
    const contactId=1;
    const mockSuccessResponse:ApiResponse<Contact>={
      success:true,
      data:{
        "contactId": 1,
    "firstName": "Prakruti",
    "lastName": "Pathak",
    "image": "Screenshot 2024-05-17 172755.png",
    "email": "prakruti@gmail.com",
    "contactNumber": "1234567891",
    "address": "Vadodara",
    "gender": "F",
    "favourite": true,
    "countryId": 2,
    "country": {
      "countryId": 2,
      "countryName": "Germany",
    },
    "stateId": 6,
    "state": {
      "stateId": 6,
      "stateName": "Berlin",
      "countryId": 0,
    },
    "imageByte":'0x',
    "birthDate": "2003/03/12"

      },
      message:''

    };
    //Act
    service.getContactById(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
      expect(response.data.contactId).toEqual(contactId);

    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });
  it('should handle failed contact retrival',()=>{
    const contactId=1;
    const mockErrorResponse:ApiResponse<Contact>={
      success:false,
      data:{} as Contact,
      message:'No record Found',
    };
    //Act
    service.getContactById(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toEqual('No record Found');
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);
  });
  it('should handle Http error while contact retrival',()=>{
    const contactId=1;
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //Act
    service.getContactById(contactId).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/GetContactById/'+contactId);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
  });
  // Contact Delete bu Id
  it('should delete a contact by id successfully',()=>{
    const contactId=1;
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      data:'',
      message:'contact deleted successfully.'

    };
    //Act
    service.deleteContact(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('contact deleted successfully.');
      expect(response.data).toEqual(mockSuccessResponse.data);
      

    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Delete/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);
  });
  it('should handle failed contact deletion',()=>{
    const contactId=1;
    const mockErrorResponse:ApiResponse<string>={
      success:false,
      data:'',
      message:'No record Found',
    };
    //Act
    service.deleteContact(contactId).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toEqual('No record Found');
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Delete/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockErrorResponse);
  });
  it('should handle Http error while contact deletion',()=>{
    const contactId=1;
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //Act
    service.deleteContact(contactId).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/Delete/'+contactId);
    expect(req.request.method).toBe('DELETE');
    req.flush({},mockHttpError);
  });
  // Favourite count
  it('should fetch a favourite contact count successfully',()=>{
    const letter='A';
    const mockSuccessResponse:ApiResponse<number>={
      success:true,
      data:0,
      message:''

    };
    //Act
    service.getAllFavouriteContactsCount(letter).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);

    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/TotalContactFavourite/?letter='+letter);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });
  it('should handle failed favourite contact count retrival',()=>{
    const letter='A';
    const mockErrorResponse:ApiResponse<number>={
      success:false,
      data:2,
      message:'No record Found',
    };
    //Act
    service.getAllFavouriteContactsCount(letter).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toEqual('No record Found');
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/TotalContactFavourite/?letter='+letter);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);
  });
  it('should handle Http error gracefully for favourite count',()=>{
    const letter='A';
    const apiUrl='http://localhost:5294/api/Contact/TotalContactFavourite/?letter='+letter;
    const errorMessage='Failed to load contacts';
    //Act
    service.getAllFavouriteContactsCount(letter).subscribe( 
      ()=>fail('expected an error, not contact'),
      (error)=>{
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }    
    );
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
   // Respond with error
   req.flush(errorMessage,{status:500,statusText:'Internal Server Error'})
  });
  //Total Count
  it('should fetch a total contact count successfully',()=>{
    const letter='A';
    const search='Abc'
    const mockSuccessResponse:ApiResponse<number>={
      success:true,
      data:0,
      message:''

    };
    //Act
    service.getAllContactsCount(letter,search).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);

    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/GetContactsCount/?letter='+letter+'&search='+search);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });
  it('should handle failed total contact count retrival',()=>{
    const letter='A';
    const search='Abc'
    const mockErrorResponse:ApiResponse<number>={
      success:false,
      data:2,
      message:'No record Found',
    };
    //Act
    service.getAllContactsCount(letter,search).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toEqual('No record Found');
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Contact/GetContactsCount/?letter='+letter+'&search='+search);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);
  });
  it('should handle Http error gracefully for total count',()=>{
    const letter='A';
    const search='Abc'
    const apiUrl='http://localhost:5294/api/Contact/GetContactsCount/?letter='+letter+'&search='+search;
    const errorMessage='Failed to load contacts';
    //Act
    service.getAllContactsCount(letter,search).subscribe( 
      ()=>fail('expected an error, not contact'),
      (error)=>{
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }    
    );
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
   // Respond with error
   req.flush(errorMessage,{status:500,statusText:'Internal Server Error'})
  });
  //Get all pagination contact
  it('should fetch all paginated contacts successfully',()=>{
    const letter='A';
    const search='abc';
    const pageNumber=1;
    const pageSize=2;
    const sort='asc';
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetAllContactsByPagination?letter='+letter+'&search='+search+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort;
    //Act
    service.getAllContactsWithPagination(pageNumber,pageSize,letter,sort,search).subscribe((response)=>{
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });
  it('should handle an empty contacts list for paginated contact',()=>{
    const letter='A';
    const search='abc';
    const pageNumber=1;
    const pageSize=2;
    const sort='asc';
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetAllContactsByPagination?letter='+letter+'&search='+search+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort;
    const emptyResponse:ApiResponse<Contact[]>={
      success:true,
      data:[],
      message:''
    }
    //Act
    service.getAllContactsWithPagination(pageNumber,pageSize,letter,sort,search).subscribe((response)=>{
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual(emptyResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);


  });
  it('should handle Http error gracefully for paginated contact',()=>{
    const letter='A';
    const search='abc';
    const pageNumber=1;
    const pageSize=2;
    const sort='asc';
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetAllContactsByPagination?letter='+letter+'&search='+search+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort;
    const errorMessage='Failed to load categories';
    //Act
    service.getAllContactsWithPagination(pageNumber,pageSize,letter,sort,search).subscribe( 
      ()=>fail('expected an error, not contact'),
      (error)=>{
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }    
    );
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
   // Respond with error
   req.flush(errorMessage,{status:500,statusText:'Internal Server Error'})
  });
  // Get all favourite contact paginated
  it('should fetch all paginated favourite contacts successfully',()=>{
    const letter='A';
    const pageNumber=1;
    const pageSize=2;
    const sort='asc';
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetPaginatedFavouriteContacts?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort;
    //Act
    service.getAllFavouriteContactsWithPagination(pageNumber,pageSize,letter,sort).subscribe((response)=>{
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });
  it('should handle an empty contacts list for paginated favourite contact',()=>{
    const letter='A';
    const pageNumber=1;
    const pageSize=2;
    const sort='asc';
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetPaginatedFavouriteContacts?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort;
    const emptyResponse:ApiResponse<Contact[]>={
      success:true,
      data:[],
      message:''
    }
    //Act
    service.getAllFavouriteContactsWithPagination(pageNumber,pageSize,letter,sort).subscribe((response)=>{
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual(emptyResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);


  });
  it('should handle Http error gracefully for paginated favourite contact',()=>{
    const letter='A';
    const pageNumber=1;
    const pageSize=2;
    const sort='asc';
    //Arrange
    const apiUrl='http://localhost:5294/api/Contact/GetPaginatedFavouriteContacts?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort;
    const errorMessage='Failed to load categories';
    //Act
    service.getAllFavouriteContactsWithPagination(pageNumber,pageSize,letter,sort).subscribe( 
      ()=>fail('expected an error, not contact'),
      (error)=>{
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }    
    );
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
   // Respond with error
   req.flush(errorMessage,{status:500,statusText:'Internal Server Error'})
  });
});
