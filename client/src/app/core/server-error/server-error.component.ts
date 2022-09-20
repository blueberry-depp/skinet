import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {
  error: any

  // These navigation extras are only available in the constructor, if we try and do this inside the ngOnInit then we're
  // going to get an undefined for this navigation extras.
  constructor(private router: Router) {
    // We want to do is get all of this or this error objects and pass it to  internal server error component now
    // because we're redirecting the user that we can't just throw the error to this component because the user or the component isn't going to
    // have access to the error at that point. So what we'll need to do is use a feature of the router that's been available since angular 7.2
    // and that's the ability to pass some state via the router to the component with redirecting to.
    const navigation = this.router.getCurrentNavigation()
    // We'll be super defensive here. We'll make sure we've got something in navigation and then we'll make sure we've got something in
    // navigation.extras and we'll make sure we've got something in navigation.extras.state because what we don't want
    // to do is have error component having an error that's not related to error. Otherwise we end up with an error within an error within an error.
    // And one thing to point out with what we're doing here is refreshing the page is going to make our state disappear. We only get one time access
    // to this when we're navigating via the routes and whilst there's many options to persist this this is not something that we're going to
    // attempt to persist.
    this.error = navigation && navigation.extras && navigation.extras.state && navigation.extras.state.error
  }

  ngOnInit(): void {
  }

}
