import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import signUp from '../../models/signupModel';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  form: FormGroup;
  registerValid: boolean = true;
  message: string | undefined;

  constructor(
    private authService: AuthService,
    private router: Router,
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      email: ['', Validators.required],
      username: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  async register() {
    let userCredentials: signUp = {
      email: this.form.value.email,
      username: this.form.value.username,
      password: this.form.value.password,
      confirmPassword: this.form.value.confirmPassword,
    };

    let registered = (
      await this.authService.register(userCredentials)
    ).subscribe({
      next: async () => {
        let loggedIn = await this.authService.login(
          userCredentials.email,
          userCredentials.password
        );

        if (loggedIn) {
          this.router.navigate(['/']);
        } else {
          this.message = 'smth went wrong';
          this.registerValid = false;
        }
      },
      error: response => {
        console.log(response);
        
      }
    }
    
  );
  }
}
