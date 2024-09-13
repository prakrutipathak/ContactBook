import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/ApiResponse{T}';
import { ContactCountry } from '../models/country.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CountryService {
  private apiUrl='http://localhost:5294/api/Country/';
  constructor(private http:HttpClient) { }
  getAllCountries():Observable<ApiResponse<ContactCountry[]>>{
    return this.http.get<ApiResponse<ContactCountry[]>>(this.apiUrl+'GetAllCountries');
  }

}
