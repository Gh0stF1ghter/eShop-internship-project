import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { userRoles } from '../../constants/userRoles';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  form: FormGroup;
  loginValid: boolean = true;

  constructor(
    private authService: AuthService,
    private router: Router,
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      email: ['', Validators.required],
      password: ['', [Validators.required]],
    });
  }

  async login() {
    let loggedIn = await this.authService.login(
      this.form.value.email,
      this.form.value.password
    );

    if (loggedIn) {
      if (this.authService.getUserRole() === userRoles.admin) {
        this.router.navigate(['/admin']);
      } else {
        this.router.navigate(['/']);
      }
    } else {
      this.loginValid = false;
    }
  }
}
