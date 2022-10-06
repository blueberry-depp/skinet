import {Component, ElementRef, Input, OnInit, Self, ViewChild} from '@angular/core';
import {ControlValueAccessor, NgControl} from "@angular/forms";

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})

// we're going to be working with these form controls and a form control in Angular is an entity that tracks the value and the
// validation status of an individual form control. And we want that information to still come back inside login form component,
// ControlValueAccessor: because we want to get access to the the form control value, then we're going to implement what's referred
// to as the control value accessor, this defines an interface that acts as a bridge between the angular forms API and the native element in the DOM,
export class TextInputComponent implements OnInit, ControlValueAccessor {
  // We need to get access to that native element(eg. input field) by using the view child.
  // static: true: because this is an input, it's always going to be on e-mail template. We're not going to surround it with an ngIf or anything like that.
  @ViewChild('input', {static: true}) input: ElementRef
  // We also take two input properties so that we can override whether or not this is a type of text inputs.
  @Input() type ='text'
  // Set this to call this label passing the label input from at the moment are login components where we're going to be using this text input.
  @Input() label:string

  // We also need to take care of in this particular component is validation that we're also getting from ControlValueAccessor. But in order
  // to access the validation, we need to get access to the control itself. And the way that we can do this is to inject it into
  // constructor here. And that means we'll be able to access its properties and validated inside these components.
  // public: we're going to need to access this inside HTML templates as well, so we'll say public.
  // NgControl: is what our form controls derive from.
  // @Self(): This is for angular dependency injection and angular is going to look for where to locate, what it's going to inject into itself,
  // and if we already have a service activated somewhere in our application, it's going to walk up the tree of dependency hierarchy looking
  // for something that matches what we're injecting here. But if we use the self decorator here, it's only going to use this inside itself
  // and not look for any other share dependency that's already in use. So this guarantees that we're working with the very specific control
  // that we're injecting in controlDir: NgControl.
  constructor(@Self() public controlDir: NgControl) {
    // Binds this to our class. And now we've got access to controlDir/control directive inside our component and will have access to it inside
    // our template as well.
    controlDir.valueAccessor = this
  }

  ngOnInit(): void {
    const control = this.controlDir.control
    // We'll get access to what validators have been set on this particular control(const control).
    // Check inside here to see if we have any validators or just going to set it to an empty array.
    // Synchronous validators is when we check in the validity of if something's required or if it's an email address.
    const validators = control.validator ? [control.validator] : []
    // Async validators are applied after synchronous validators, for if we want to validate its going to an API and check something on there.
    const asyncValidators = control.asyncValidator ? [control.asyncValidator] : []

    // The control that we passed from, let's say login form is going to pass across it validators to this inputs and it's going to set them at the same time.
    control.setValidators(validators)
    control.setAsyncValidators(asyncValidators)
    // This is going to try and validate form on initialization, exactly the same thing that's going on locally inside our login component
    // with that form at the moment.
    control.updateValueAndValidity()

  }

  onChange(event) {}

  onTouched() {}

  registerOnChange(fn: any): void {
    // Set this equal to the ControlValueAccessor function.
    this.onChange = fn
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn
  }

  writeValue(obj: any): void {
    // We need to get the value of inputs and write it into this. And this gives ControlValueAccessor access
    // to the values that are inputs into our input field.
    // Get the value of this elements and set the value.
    this.input.nativeElement.value = obj || ''
  }

}
