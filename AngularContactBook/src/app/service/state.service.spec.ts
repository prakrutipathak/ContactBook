import { TestBed } from '@angular/core/testing';

import { StateService } from './state.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CountryState } from '../models/country.state.model';
import { ApiResponse } from '../models/ApiResponse{T}';

describe('StateService', () => {
  let service: StateService;
  let httpMock:HttpTestingController;
  const mockApiResponse:ApiResponse<CountryState[]>={
    success:true,
    data:[
      {
        "stateId": 1,
        "stateName": "Gujarat",
        "countryId": 1,
        "country": {
          "countryId": 1,
          "countryName": "India",
        }
      },
      {
        "stateId": 2,
        "stateName": "Uttar Pradesh",
        "countryId": 1,
        "country": {
          "countryId": 1,
          "countryName": "India",
        }
      },
      {
        "stateId": 3,
        "stateName": "Karnataka",
        "countryId": 1,
        "country": {
          "countryId": 1,
          "countryName": "India",
        }
      },
    ],
    message:''
  }


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[StateService]
    });

    service = TestBed.inject(StateService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  // Get state by countryid
  it('should fetch all state by country id successfully',()=>{
    //Arrange
    const countryId=1;
    const apiUrl='http://localhost:5294/api/State/GetStatesByCountryId/'+countryId;
    
    //Act
    service.GetStatesByCountryId(countryId).subscribe((response)=>{
      expect(response.data.length).toBe(3);
      expect(response.data).toEqual(mockApiResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });
  it('should handle an empty state list',()=>{
    const countryId=1;
    const apiUrl='http://localhost:5294/api/State/GetStatesByCountryId/'+countryId;
    const emptyResponse:ApiResponse<CountryState[]>={
      success:true,
      data:[],
      message:''
    }
    //Act
    service.GetStatesByCountryId(countryId).subscribe((response)=>{
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual(emptyResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);


  });
  it('should handle Http error gracefully for fetching all state by countryid',()=>{
    const countryId=1;
    const apiUrl='http://localhost:5294/api/State/GetStatesByCountryId/'+countryId;
    const errorMessage='Failed to load categories';
    
    //Act
    service.GetStatesByCountryId(countryId).subscribe( 
      ()=>fail('expected an error, not country'),
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
