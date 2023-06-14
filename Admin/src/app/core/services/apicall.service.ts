import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { map } from 'rxjs/operators';
import { User } from '../models/auth.models';
import { BehaviorSubject, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class ApicallService {
  subscribe(arg0: (data: any) => void) {
    throw new Error('Method not implemented.');
  }
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<User>;
 baseUrl = "https://outreach-api.drscloud.net/"
 // baseUrl = "http://localhost:62118/"
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }
  school = {}
  token: any;
  //loginForm: FormGroup;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }


  AdminLogin(Username: any, Password: any) {
    var loginform = {
      Username: Username,
      Password: Password
    };

    return this.http.post(this.baseUrl + "Authenticate/AdminLogin", loginform)
      .pipe(map(user => {
        // login successful if there's a jwt token in the response
        if (user) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }
        return user;
      }));

  }

  SendMail(UserEmail : any):Observable<any>{
    debugger;
    //return this.http.post('http://localhost:62118/'+"User/UpdateUserPassword/",UserEmail, this.httpOptions);
   return this.http.post(this.baseUrl+"User/UpdateUserPassword/",UserEmail, this.httpOptions);
  }

  OTPverify(Useremail:any,otp:any):Observable<any>
  {
    //debugger;
    var loginform = {
      Useremail: Useremail,
      otp: otp
    };
     debugger;
     //return this.http.post('http://localhost:62118/'+"User/OtpVerification/",loginform);
     return this.http.post(this.baseUrl+"User/OtpVerification/",loginform);

   }

   passwordReset(useremail:any,userpassword:any):Observable<any>
   {
     //debugger;
     var loginform = {
       useremail: useremail,
       userpassword: userpassword
     };
     // debugger;
      //return this.http.post('http://localhost:62118/'+"User/ResetPassword/",loginform);
      return this.http.post(this.baseUrl+"User/ResetPassword/",loginform);

    }
  

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  GetAllSchool(page: any, PerPage: any, Name: any, City: any): Observable<any> {
   // return this.http.post('http://localhost:62118/School/GetAllSchool?pagenumber=' + page + '&pagesize=' + PerPage + '&data.Name=' + Name + '&data.City=' + City, this.school, this.httpOptions);

  return this.http.post(this.baseUrl + 'School/GetAllSchool?pagenumber=' + page + '&pagesize=' + PerPage + '&data.Name=' + Name + '&data.City=' + City, this.school, this.httpOptions);

  }



 GetEventDetail(id:any) : Observable<any> {
  //return this.http.get('https://outreach-api.drscloud.net/Event/GetEventDetail?id='+id, this.httpOptions);
  return  this.http.get(this.baseUrl + 'Event/GetEventDetail?id='+id, this.httpOptions);
 debugger
 //return this.http.get('http://localhost:62118/' + 'Event/GetEventDetail?id='+id, this.httpOptions);

 }


GetAllEvent(page: any,PerPage:any,City:any,Name :any ) : Observable<any> {
  return this.http.post(this.baseUrl + 'Event/GetAllEvent?pagenumber=' + page +'&pagesize='+PerPage+'&data.City='+City+'&data.Name='+Name, this.school, this.httpOptions);
  //return this.http.post('http://localhost:62118/' + 'GetAllEvent?pagenumber=' + page +'&pagesize='+PerPage+'&data.City='+City+'&data.Name='+Name, this.school, this.httpOptions);

}

  GetAllInterest(page: any, PerPage: any, Name: any): Observable<any> {
   return this.http.post(this.baseUrl + 'Interest/GetAllInterest?pagenumber=' + page + '&pagesize=' + PerPage + '&data.Name=' + Name, this.school, this.httpOptions);
   //return this.http.post('http://localhost:62118/' + 'GetAllInterest?pagenumber=' + page + '&pagesize=' + PerPage + '&data.Name=' + Name, this.school, this.httpOptions);

  }
  GetAllUser(page: any, PerPage: any, Firstname: any,Middlename: any,Lastname: any,UserEmail: any,useractive:boolean): Observable<any> {
    return this.http.post(this.baseUrl + 'User/GetAllUser?pagenumber=' + page + '&pagesize=' + PerPage + '&data.Firstname=' + Firstname+ '&data.Middlename=' + Middlename+ '&data.Lastname=' + Lastname+ '&data.UserEmail=' + UserEmail+ '&data.IsActive=' + useractive, this.school, this.httpOptions);
    debugger;
    //return this.http.post('http://localhost:62118/' + 'User/GetAllUser?pagenumber=' + page + '&pagesize=' + PerPage + '&data.Firstname=' + Firstname+ '&data.Middlename=' + Middlename + '&data.Lastname=' + Lastname+ '&data.UserEmail=' + UserEmail+ '&data.IsActive=' + useractive, this.school, this.httpOptions);

   }
  GetUserById(id:any):Observable<any>{
    return this.http.get(this.baseUrl+"User/GetUserById/"+id)

  }

  AddSchool(newschool: any) {
    //return  this.http.post("http://localhost:62118/AddSchool", newschool, this.httpOptions);  

   return this.http.post(this.baseUrl + 'School/AddSchool', newschool, this.httpOptions);
  }

  UpdateSchool(newschool: any) {
    // return  this.http.post("http://localhost:62118/UpdateSchool", newschool, this.httpOptions);  
    return this.http.post(this.baseUrl + "School/UpdateSchool", newschool, this.httpOptions);

  }
  DeleteSchool(SchoolId: any) {
    // return  this.http.post("http://localhost:62118/DeleteSchool?id="+ SchoolId +"&status=" + false, null, this.httpOptions);  
    return this.http.post(this.baseUrl + "School/DeleteSchool?id=" + SchoolId + "&status=" + false, null, this.httpOptions);

  }
  AddInterest(newinterest: any) {
    //return this.http.post("http://localhost:62118/AddInterest", newinterest, this.httpOptions);

    return this.http.post(this.baseUrl + 'Interest/AddInterest', newinterest, this.httpOptions);
  }
  UpdateInterest(newinterest: any) {
    //return this.http.post("http://localhost:62118/UpdateInterest", newinterest, this.httpOptions);
    return this.http.post(this.baseUrl + "Interest/UpdateInterest", newinterest, this.httpOptions);
  }
  DeleteInterest(InterestId:any) {
    //return this.http.post("http://localhost:62118/DeleteInterest?id="+ InterestId +"&status=" + false, null, this.httpOptions);
    return this.http.post(this.baseUrl + "Interest/DeleteInterest?id=" + InterestId + "&status=" + false, null, this.httpOptions);
  }
  // GetEventDetail(page: any, PerPage: any, Name: any){
  //   return this.http.post("http://localhost:62118/GetEventDetail?id="  + page + '&pagesize=' + PerPage + '&data.Name=' + Name, this.school, this.httpOptions);
  //   //return this.http.post(this.baseUrl + "Event/GetEventDetail?id=" + EventId + "&status=" + false, null, this.httpOptions );
  // }
count =new BehaviorSubject(0);
GetTotalSchoolCount(count : any ){
   this.count.next(count);
}
GetTotalUserCount(count : any ){
  this.count.next(count);
}

GetTotalInterestCount(count : any ){
  this.count.next(count);

}
GetTotalEventDetailCount(count : any ){
  this.count.next(count);
}
}
