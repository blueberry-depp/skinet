import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  // @Input() property: something we receive from a parent component
  // and @Output() is a way that a child component pagination component or pager component is going to be a
  // child component on the shop components page and  we want to emit an output from child component to parent component,
  // and shop component has the method onPageChanged to change the page and we still want to call this method but from child component.
  @Input() totalCount: number
  @Input() pageSize: number
  // We need an output property this time because we're emitting information out of the components,
  // this is going to need to emit an event, and we specify the type of thing that we're going to emit from this particular event(<>).
  @Output() pageChanged = new EventEmitter<number>()

  constructor() { }

  ngOnInit(): void {
  }

  onPagerChange(event: any) {
    // We're going to be emitting this number(event.page) from the child component which is our pagination component,
    // and it's going to be received by the parent component.
    this.pageChanged.emit(event.page)
  }

}
