import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../account.service";

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService
  ) {
  }

  ngOnInit(): void {
    this.createCheckoutForm()
    this.getAddressFormValues()

  }

  createCheckoutForm() {
    // We can validate each of these forms individually and we can also validate the form as a whole.
    this.checkoutForm = this.fb.group({
      // We can have form groups inside form group.
      addressForm: this.fb.group({
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        street: [null, Validators.required],
        city: [null, Validators.required],
        state: [null, Validators.required],
        zipcode: [null, Validators.required],
      }),
      deliveryForm: this.fb.group({
        deliveryMethod: [null, Validators.required],
      }),
      paymentForm: this.fb.group({
        nameOnCard: [null, Validators.required],
      })
    })
  }

  // Get address values from API and use in checkout component.
  getAddressFormValues() {
    this.accountService.getUserAddress().subscribe({
      next: address => {
        if (address) {
          // Populate form with the address values.
          this.checkoutForm.get('addressForm').patchValue(address)
        }
      },
      error: error => console.log(error)
    })
  }


}
