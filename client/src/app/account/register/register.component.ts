import {Component, OnInit} from '@angular/core';
import {AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, ValidatorFn, Validators} from "@angular/forms";
import {AccountService} from "../../account.service";
import {Router} from "@angular/router";
import {map, of, switchMap, timer} from "rxjs";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup
  errors: string[]

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.createRegisterForm()
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      displayName: [null, Validators.required],
      email: ['',
        // This is synchronous validators these ones are going to happen instantly.
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
        // The asynchronous validators are only call if our synchronous validators have passed validation so we're not
        // going to be calling this as we're typing we're only going to call this when both of these validators
        // have passed and we've got a valid email address at that point we call async validator and it's gonna make that network request to API,
        // however it's going to wait until 500 milliseconds after we've type to character before doing so.
        [this.validateEmailNotTaken()]
      ],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]] // specify password as the field that we want to match this against.
    })
    // If we change the password after validate against confirm password, it will be valid, because the confirmed password validator is only applied to the confirmed password,
    // we need also check the password field again and update it is valid the T against the confirmed password.
    // Get the password control, check the value changes of this control then we subscribe to the value changes.
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      // So what we're going to be testing for is when our password changes, then we're going to update the validity of password against confirmPassword as well,
      // and if either one of them changes and does not match the other one, then we validate form once again.
      this.registerForm.controls.confirmPassword.updateValueAndValidity()
    })
  }

  // Custom validator.
  // matchTo: string: because our field names are strings.
  // : -> specify a type of what we want to return and return ValidatorFn.
  matchValues(matchTo: string): ValidatorFn {
    // All the FormControl derive from an AbstractControl.
    return (control: AbstractControl) => {
      // control?.value: to confirm password control and compare this to whatever we pass into the matchTo and we're going to pass in the password,
      // that we want to compare this value to and if these passwords match, then we return null and that means validation has passed,
      // if the passwords don't match, then we attach a validator error called isMatching to the control, and then this will fail our form validation.
      // controls: gives us access to all of the controls in the form.
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching: true}
    }
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: response => this.router.navigateByUrl('/shop'),
      error: error => {
        console.log(error)
        this.errors = error.errors
      }
    })
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        // switchMap: special operator that return the inner observable and return it to control which is the outer observable
        switchMap(() => {
          // Check the control have a value or not.
          if (!control.value) {
            // Returning an observable of something and we return null.
            return of(null)
          }
          // If we do have something inside control and there's been a delay of 500 milliseconds.
          // (control.value): is email address
          return this.accountService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null
            })
          )
        })
      )
    }
  }

}
