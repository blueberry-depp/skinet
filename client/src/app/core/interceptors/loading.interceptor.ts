import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {delay, finalize, Observable} from 'rxjs';
import {BusyService} from "../services/busy.service";

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // we need two things, w need to sort of whitelist this specific request as go into server to prevent that from being
    // affected by a loading spinner and then we'll need to add a little individual spinner to the email address.
    // Check to see if we're making a specific request.
    // includes: search for a specific text/in this case is url.
    if (request.url.includes('emailexists')) {
      // When we're about to send request, we're going to call the following method.
      return next.handle(request)
    }

    this.busyService.busy()

    // And once the request comes back, we know it's completed, so we can turn off our busy Spinner.
    return next.handle(request).pipe(
      // Add fake delay.
     delay(1000),
      // This gives to do something when things have completed.
      finalize(() => {
        // Turn off busy spinner.
        this.busyService.idle()
      })
    );
  }
}
