import {Injectable} from '@angular/core';
import {environment} from "../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {BehaviorSubject, map, of, ReplaySubject} from "rxjs";
import {IUser} from "./shared/models/user";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;

  // Now our user object we're going to go to API of course and we're going to get back a user object and this is going to
  // contain email, display name and token, the token we're going to want to persist in local storage so that when a user
  // closes their browser and then comes back to our application, we can check to see if they have a token and then
  // we can automatically retrieve their user information from the API and effectively log them into application. And what
  // we're also want to access the user information in other places in application and what we want to do is replace
  // the log in and sign up buttons and just say welcome Bob or whoever the user is. So we need an observable for our current user.
  // ReplaySubject is not emit initial value.
  private currentUserSource = new ReplaySubject<IUser>(1)
  currentUser$ = this.currentUserSource.asObservable()

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
  }

  // We need to make a call to API. If we have a token at local storage, so we'll take the token as a string as a parameter and
  // then pass this token to API because getting the current user is a method that we need to be authorized for before we can
  // return the current user. So we'll need to send up the token, and we've seen from what we did in postman that we need to pass this
  // as a header in our request with a key of authorization and then we pass the token as a bearer token.
  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null)
      // We still need to return something an observable to the method that's calling this loadCurrentUser.
      // of: is an observable.
      return of(null)
    }

    let headers = new HttpHeaders()
    headers = headers.set('Authorization', `Bearer ${token}`)

    return this.http.get(`${this.baseUrl}account`, {headers}).pipe(
      // Mapping and projecting the user object we receive into currentUserSource observable.
      map((user: IUser) => {
        // Check to see that we've got something in the user object.
        if (user) {
          // Token which we get from user token.
          localStorage.setItem('token', user.token)
          // Update the observable.
          this.currentUserSource.next(user)
        }
      })
    )
  }


  // This is going to take some values that we're going to receive from a form that the user is going to complete with
  // that email and password and just set the values to any.
  login(values: any) {
    return this.http.post(`${this.baseUrl}account/login`, values).pipe(
      // Mapping and projecting the response into currentUserSource observable.
      map((user: IUser) => {
        // Check to see that we've got something in the user object.
        if (user) {
          // Token which we get from user token.
          localStorage.setItem('token', user.token)
          // We're storing user object in our service.
          this.currentUserSource.next(user)
        }
      })
    )
  }

  register(values: any) {
    return this.http.post(`${this.baseUrl}account/register`, values).pipe(
      map((user: IUser) => {
        if (user) {
          // Token which we get from user token.
          localStorage.setItem('token', user.token)
          // We're storing user object in our service.
          this.currentUserSource.next(user)
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('token')
    this.currentUserSource.next(null)
    this.router.navigateByUrl('/')
  }

  checkEmailExists(email: string) {
    return this.http.get(`${this.baseUrl}account/emailexists?email=${email}`)
  }

  getUserAddress() {
    return this.http.get(`${this.baseUrl}account/address`)
  }

}
