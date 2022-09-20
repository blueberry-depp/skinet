import { Component, OnInit } from '@angular/core';
import {BreadcrumbService} from "xng-breadcrumb";
import {Observable} from "rxjs";

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit {
  // We populating it inside here so that we could use it in template.
  breadcrumb$: Observable<any[]>
  constructor(
    private breadcrumbService: BreadcrumbService
  ) { }

  ngOnInit(): void {
    // In order to get access to this observable, our options are we can even subscribe to this
    // observable we can make this observable a promise or we can pipe it and projects it into something
    // else but then we would still need to even subscribe to it or send it to a promise,
    // now there is a general rule of thumb that if you subscribe to something then you should
    // always unsubscribe. In terms of http requests in angular and http request is considered finite. It has a start and an end,
    // and when he http request response has been completed then angular itself caused the complete on the subscribe so we don't need to unsubscribe
    // and we can use complete method too(next, error, complete), angular also has a lifecycle method call onDestroy
    // and what we could do is implement the on destroying interface. We don't use in component yet because is going to destroy itself when the
    // component is no longer in use.
    this.breadcrumb$ = this.breadcrumbService.breadcrumbs$
  }

}
