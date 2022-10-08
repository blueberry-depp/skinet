import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {ToastrService} from "ngx-toastr";
import {NavigationExtras, Router} from "@angular/router";

@Injectable()
// Because this is an interceptor, we need to provide this in our app module.
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    // Router gives us access to navigation functionality, so we'll be able to redirect the user to somewhere else.
    private router: Router,
    private toastr: ToastrService
  ) {
  }

  // We want to catch any errors from this
  // The request, what we get back from this is unobservable, so in order to do something with this, like
  // any other observable, we're going to need to use the pipe method to do whatever functionality we want inside here,
  // we're going to use Rxjs feature called Catch Error.
  // request: this is going to be outgoing request so we can do something on the way out.
  // next: this is http response which is coming back, and we want to catch any errors inside the response coming back from API. And that
  // will give us an opportunity to do something with the particular errors.
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // pipe: which is going to allow us to chain rxjs operators.
    return next.handle(request).pipe(
      // Because http response is coming back as an observable then we need to use the rxjs catch error method in
      // order to catch the error out of the observable itself. And that's why we're using the pipe here to do so.
      catchError(error => {
        // Make sure we've got an error object, and if we do we can start to check fully error status whether it's a 400 or for a for that kind of thing.
        if (error) {
          // For 400 error this is typically generated when a user submits something to the server and it's just a bad request for whatever reason.
          if (error.status === 400) {
            // 400 validation error is because the user sent up some form data and we would want to display the results
            // of what they've sent up to the server and why the servers rejected it the list of errors on the form itself or inside the components itself.
            if (error.error.errors) {
              throw error.error
            } else {
              this.toastr.error(error.error.message, error.error.statusCode)
            }
          }

          if (error.status === 401) {
            this.toastr.error(error.error.message, error.error.statusCode)
          }


          if (error.status === 404) {
            this.router.navigateByUrl('/not-found')
          }

          if (error.status === 500) {
            // And if we've got an error status of 500 then we pass the exception information to server error component. And since
            // Angular 7.2 we've been able to pass state to the routes that we're about to navigate to and in order to do this we're
            // going to pass it through something called navigation extras.
            // error.error: exception that we get back from our API
            const navigationExtras: NavigationExtras = {state: {error: error.error}}
            // Pass in the navigationExtras as the states
            this.router.navigateByUrl('/server-error', navigationExtras)
          }
        }
        // Throw the error of interceptor
        return throwError(error)
      })
    )
  }
}
