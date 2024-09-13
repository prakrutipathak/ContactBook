import { TestBed } from '@angular/core/testing';

import { CountryService } from './country.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ContactCountry } from '../models/country.model';
import { ApiResponse } from '../models/ApiResponse{T}';

describe('CountryService', () => {
  let service: CountryService;
  let httpMock:HttpTestingController;
  const mockApiResponse:ApiResponse<ContactCountry[]>={
    success:true,
    data:[
      {
        "countryId": 1,
        "countryName": "India"
      },
      {
        "countryId": 2,
        "countryName": "Germany"
      },
      {
        "countryId": 3,
        "countryName": "America"
      },
    ],
    message:''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers:[CountryService]
    });
    service = TestBed.inject(CountryService);
    httpMock=TestBed.inject(HttpTestingController);
  });
  afterEach(()=>{
    httpMock.verify();
    
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  // get all country
  it('should fetch all country successfully',()=>{
    //Arrange
    const apiUrl='http://localhost:5294/api/Country/GetAllCountries';
    //Act
    service.getAllCountries().subscribe((response)=>{
      expect(response.data.length).toBe(3);
      expect(response.data).toEqual(mockApiResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });
  it('should handle an empty country list',()=>{
    const apiUrl='http://localhost:5294/api/Country/GetAllCountries';
    const emptyResponse:ApiResponse<ContactCountry[]>={
      success:true,
      data:[],
      message:''
    }
    //Act
    service.getAllCountries().subscribe((response)=>{
      expect(response.data.length).toBe(0);
      expect(response.data).toEqual(emptyResponse.data);


    });
    const req= httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);


  });
  it('should handle Http error gracefully for fetching all countries',()=>{
    const apiUrl='http://localhost:5294/api/Country/GetAllCountries';
    const errorMessage='Failed to load categories';
    //Act
    service.getAllCountries().subscribe( 
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
