import { Injectable } from '@angular/core';
import { CountryState } from '../models/country.state.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private apiUrl='http://localhost:5294/api/State/';
  constructor(private http:HttpClient) { }
  GetStatesByCountryId(countryId:number):Observable<ApiResponse<CountryState[]>>{
    return this.http.get<ApiResponse<CountryState[]>>(this.apiUrl+'GetStatesByCountryId/'+countryId);
  }
  getAllStates():Observable<ApiResponse<CountryState[]>>{
    return this.http.get<ApiResponse<CountryState[]>>(this.apiUrl+'GetAllStates');
  }
}
