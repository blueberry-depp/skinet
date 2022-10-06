import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {map, Observable} from 'rxjs';
import {AccountService} from "../../account.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    // Check the current user observable if there's authenticated user/user logged in or not,
    // when in the context of a router which we are going to be because this is router functionality. Then when we
    // activate the routes and we want to observe something or we would check what's inside that observable we don't
    // actually need to subscribe because the router is going to subscribe for us and therefore unsubscribe from this as well.
    // We want auth guard to wait until currentUser$ has something/value.
    return this.accountService.currentUser$.pipe(
      map(auth => {
        if (auth) return true
        // If they not log in.
        this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}})
      })

    )
  }

}
