import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Contact } from '../models/contact.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { AddContact } from '../models/addcontact.model';
import { UpdateContact } from '../models/update.contact.model';
import { ContactSP } from '../models/spcontact.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl='http://localhost:5294/api/Contact/';
  constructor(private http:HttpClient) { }
  getAllContacts():Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+'GetAllContacts');
  }
  GetAllFavourite():Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+'GetAllFavourite');
  }
  getContactById(contactId:number |undefined):Observable<ApiResponse<Contact>>{
    return this.http.get<ApiResponse<Contact>>(this.apiUrl+'GetContactById/'+contactId);
  }
  getAllContactsCount(letter:string,search:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'GetContactsCount/?letter='+letter+'&search='+search);
  }
  getAllContactsWithPagination(pageNumber: number,pageSize:number,letter:string,sort:string,search:string) : Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+'GetAllContactsByPagination?letter='+letter+'&search='+search+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort);
  }
  getAllFavouriteContactsCount(letter:string) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'TotalContactFavourite/?letter='+letter);
  }
  getAllFavouriteContactsWithPagination(pageNumber: number,pageSize:number,letter:string,sort:string) : Observable<ApiResponse<Contact[]>>{
    return this.http.get<ApiResponse<Contact[]>>(this.apiUrl+'GetPaginatedFavouriteContacts?letter='+letter+'&page='+pageNumber+'&pageSize='+pageSize+'&sortOrder='+sort);
  }
  createContact(contact: AddContact): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl+'Create', contact);
  }
  modifyContact(updateContact:UpdateContact):Observable<ApiResponse<string>>{
    return this.http.put<ApiResponse<string>>(this.apiUrl+"Edit",updateContact);
  }
  deleteContact(contactId:number |undefined):Observable<ApiResponse<string>>{
    return this.http.delete<ApiResponse<string>>(this.apiUrl+'Delete/'+contactId);
  }
  CountContactBasedOnCountry(countryId:number|undefined) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'CountContactBasedOnCountry/'+countryId);
  }
  CountContactBasedOnGender(gender:string|undefined) : Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+'CountContactBasedOnGender/'+gender);
  }
  GetDetailByBirthMonth(month: number|undefined) : Observable<ApiResponse<ContactSP[]>>{
    return this.http.get<ApiResponse<ContactSP[]>>(this.apiUrl+'GetDetailByBirthMonth/'+month);
  }
  GetDetailByStateId(stateId: number|undefined) : Observable<ApiResponse<ContactSP[]>>{
    return this.http.get<ApiResponse<ContactSP[]>>(this.apiUrl+'GetDetailByStateId/'+stateId);
  }
}
