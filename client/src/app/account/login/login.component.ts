import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../../account.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  // Create a reactive angular FormGroup.
  loginForm: FormGroup
  returnUrl: string


  constructor(
    private accountService: AccountService,
    private router: Router,
    // We want to get query parameters from our route and what we need to do is actually bring in the activated route as well so that we can
    // get access to the query parameters, we can't get them from the router.
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    // Set the return url if we haven't.
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop'
    // Initialize form when login component is created.
    this.createLoginForm()

  }

  // Create FormControl and then we're going to apply those to login form inputs in template.
  createLoginForm() {
    this.loginForm = new FormGroup({
      // '': initial state.
      email: new FormControl('', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      password: new FormControl('', Validators.required)
    })
  }

  // When user submit the form and get back information from the server.
  onSubmit() {
    // console.log(this.loginForm.value)
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => this.router.navigateByUrl(this.returnUrl),
      error: error => console.log(error)
    })
  }
}
