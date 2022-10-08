import {Component, Input, OnInit} from '@angular/core';
import {CdkStepper} from "@angular/cdk/stepper";

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss'],
  providers: [{provide: CdkStepper, useExisting: StepperComponent}]
})
export class StepperComponent extends CdkStepper implements OnInit {
  // We'll use this to let the client tell us all the child's component, whether or not the linear mode is selected,
  // this is the only property that we'll need to add that we want to receive from clients.
  @Input() linearModeSelected: boolean

  ngOnInit(): void {
    this.linear = this.linearModeSelected
  }

  onClick(index: number) {
    // We keep track of which step we're currently on.
    this.selectedIndex = index
    //console.log(this.selectedIndex)
  }

}
