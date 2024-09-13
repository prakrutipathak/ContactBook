import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LocalStorageKeys } from './helpers/localstoragekeys';
import { ApiResponse } from '../models/ApiResponse{T}';
import { LocalstorageService } from './helpers/localstorage.service';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { ForgetPassword } from '../models/forgetpassword.model';
import { UserDetail } from '../models/userDetails.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl='http://localhost:5294/api/Auth/';
  private authState= new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
  private userIdSubject=new BehaviorSubject<string | null |undefined>(this.localStorageHelper.getItem(LocalStorageKeys.UserId));
  private usernameSubject=new BehaviorSubject<string | null |undefined>(this.localStorageHelper.getItem(LocalStorageKeys.LoginId));
  constructor(private localStorageHelper:LocalstorageService, private http:HttpClient) { }
  signup(user:User):Observable<ApiResponse<string>>{
    const body=user;
    return this.http.post<ApiResponse<string>>(this.apiUrl+'Register',body);
  }
  forgetPassword(user:ForgetPassword):Observable<ApiResponse<string>>{
    const body=user;
    return this.http.post<ApiResponse<string>>(this.apiUrl+'ForgetPassword',body);
  }
  getUserDetailById(userId: number | undefined): Observable<ApiResponse<UserDetail>> {
    return this.http.get<ApiResponse<UserDetail>>(this.apiUrl + 'GetUserById/' + userId);
  }
  updateDetails(user:UserDetail):Observable<ApiResponse<string>>{
    const body=user;
    return this.http.put<ApiResponse<string>>(this.apiUrl+'Edit',body);
  }

  changePassword(user:ForgetPassword):Observable<ApiResponse<string>>{
    const body=user;
    return this.http.post<ApiResponse<string>>(this.apiUrl+'ForgetPassword',body);
  }
  signIn(username:string,password:string):Observable<ApiResponse<string>>{
    const body={username,password};
     return this.http.post<ApiResponse<string>>(this.apiUrl+'Login',body).pipe(
      tap(response=>{
        if(response.success){
          const token=response.data;
          const payload = token.split('.')[1];
          const decodedPayload = JSON.parse(atob(payload));
          const userId = decodedPayload.UserId;
          this.localStorageHelper.setItem(LocalStorageKeys.TokenName,token);
          this.localStorageHelper.setItem(LocalStorageKeys.UserId,userId);
          this.localStorageHelper.setItem(LocalStorageKeys.LoginId,username);
          this.authState.next(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
          this.usernameSubject.next(username);
        }
      })
    );
  }
  signOut(){
    this.localStorageHelper.removeItem(LocalStorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalStorageKeys.UserId);
    this.localStorageHelper.removeItem(LocalStorageKeys.LoginId);
    this.authState.next(false);
    this.usernameSubject.next(null);
  }
  isAuthenticated(){
    return this.authState.asObservable();
  }
  getUsername():Observable<string |null|undefined>{
    return this.usernameSubject.asObservable();
  }
  getUserId():Observable<string |null|undefined>{
    return this.userIdSubject.asObservable();
  }
}
