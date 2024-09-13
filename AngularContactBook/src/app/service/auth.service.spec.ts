import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { User } from '../models/user.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { ForgetPassword } from '../models/forgetpassword.model';
import { UserDetail } from '../models/userDetails.model';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock:HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  // Register User
  it('should register user successfully',()=>{
    //arrange
    const registerUser:User={
      "userId": 1,
  "firstName": "string",
  "lastName": "string",
  "loginId": "string",
  "email": "user@example.com",
  "contactNumber": "330407 1959",
  "password": "Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s'!%U.+syh,0 P8sb-XmUqD",
  "confirmPassword": "string",
  "image":"abc.jpg",
  "imageByte":"",
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"User register successfully.",
      data:""
    }
    //act
    service.signup(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user register',()=>{
    //arrange
    const registerUser:User={
      "userId": 0,
  "firstName": "string",
  "lastName": "string",
  "loginId": "string",
  "email": "user@example.com",
  "contactNumber": "330407 1959",
  "password": "Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s'!%U.+syh,0 P8sb-XmUqD",
  "confirmPassword": "string",
  "image":"abc.jpg",
  "imageByte":"",
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.signup(registerUser).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while register user',()=>{
    //arrange
    const registerUser:User={
      "userId": 0,
  "firstName": "string",
  "lastName": "string",
  "loginId": "string",
  "email": "user@example.com",
  "contactNumber": "330407 1959",
  "password": "Di5;reP9]]A,0@c\\%V*g?Do>A/<5I?yBkWM2`dCWQ.s'!%U.+syh,0 P8sb-XmUqD",
  "confirmPassword": "string",
  "image":"abc.jpg",
  "imageByte":"",
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.signup(registerUser).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/Register');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
  //Forget Password
  it('should update password successfully',()=>{
    //arrange
    const forgetPassword:ForgetPassword={
      "userName": "string",
      "password": "string",
      "confirmPassword": "string"
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"Password successfully change.",
      data:""
    }
    //act
    service.forgetPassword(forgetPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user forget password',()=>{
    //arrange
    const forgetPassword:ForgetPassword={
      "userName": "string",
  "password": "string",
  "confirmPassword": "string"
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.forgetPassword(forgetPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while forget password',()=>{
    //arrange
    const forgetPassword:ForgetPassword={
      "userName": "string",
      "password": "string",
      "confirmPassword": "string"
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.forgetPassword(forgetPassword).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
   //Change Password
   it('should change password successfully',()=>{
    //arrange
    const forgetPassword:ForgetPassword={
      "userName": "string",
      "password": "string",
      "confirmPassword": "string"
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"Password successfully change.",
      data:""
    }
    //act
    service.changePassword(forgetPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed user change password',()=>{
    //arrange
    const forgetPassword:ForgetPassword={
      "userName": "string",
  "password": "string",
  "confirmPassword": "string"
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.changePassword(forgetPassword).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while change password',()=>{
    //arrange
    const forgetPassword:ForgetPassword={
      "userName": "string",
      "password": "string",
      "confirmPassword": "string"
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.changePassword(forgetPassword).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/ForgetPassword');
    expect(req.request.method).toBe('POST');
    req.flush({},mockHttpError);

  });
   //Update Details
   it('should update details successfully',()=>{
    //arrange
    const userdetails:UserDetail={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "image":"abc.jpg",
      "imageByte":"",
    };
    const mockSuccessResponse:ApiResponse<string>={
      success:true,
      message:"Update user successfuly.",
      data:""
    }
    //act
    service.updateDetails(userdetails).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/Edit');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed in user updation',()=>{
    //arrange
    const userdetails:UserDetail={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "image":"abc.jpg",
      "imageByte":"",
    };
    const mockErrorResponse:ApiResponse<string>={
      success:true,
      message:"User already exists.",
      data:""
    }
    //act
    service.updateDetails(userdetails).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/Edit');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while update detail',()=>{
    //arrange
    const userdetails:UserDetail={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "image":"abc.jpg",
      "imageByte":"",
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.updateDetails(userdetails).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/Edit');
    expect(req.request.method).toBe('PUT');
    req.flush({},mockHttpError);

  });
   //getUserDetailById
   it('should get user details successfully',()=>{
    //arrange
    const userdetails:UserDetail={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "image":"abc.jpg",
      "imageByte":"",
    };
    const mockSuccessResponse:ApiResponse<UserDetail>={
      success:true,
      message:"Update user successfuly.",
      data:userdetails
    }
    //act
    service.getUserDetailById(userdetails.userId).subscribe(response=>{
      //assert
      expect(response).toBe(mockSuccessResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/GetUserById/'+userdetails.userId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });
  it('should handle failed to fetch',()=>{
    //arrange
    const userdetails:UserDetail={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "image":"abc.jpg",
      "imageByte":"",
    };
    const mockErrorResponse:ApiResponse<UserDetail>={
      success:true,
      message:"",
      data:userdetails
    }
    //act
    service.getUserDetailById(userdetails.userId).subscribe(response=>{
      //assert
      expect(response).toBe(mockErrorResponse);
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/GetUserById/'+userdetails.userId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });
  it('should handle Http error while fetching detail',()=>{
    //arrange
    const userdetails:UserDetail={
      "userId": 1,
      "firstName": "string",
      "lastName": "string",
      "loginId": "string",
      "email": "user@example.com",
      "contactNumber": "330407 1959",
      "image":"abc.jpg",
      "imageByte":"",
    };
    const mockHttpError={
      statusText:"Internal Server Error",
      status:500
    }
    //act
    service.getUserDetailById(userdetails.userId).subscribe({
      next:()=>fail('should have failed with the 500 error'),
      error:(error)=>{
        //assert
      expect(error.status).toEqual(500);
      expect(error.statusText).toEqual('Internal Server Error');
      }
    });
    const req=httpMock.expectOne('http://localhost:5294/api/Auth/GetUserById/'+userdetails.userId);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);

  });
});
