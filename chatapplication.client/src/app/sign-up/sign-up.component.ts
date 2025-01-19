import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';  // Import Router

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  username: string = '';
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSignUp(): void {
    this.authService.signUp(this.username, this.email, this.password).subscribe({
      next: (response) => {
        console.log('Sign-up successful');
        this.router.navigate(['/sign-in']);
        // Optionally, redirect to login or home page
      },
      error: (err) => {
        this.errorMessage = 'Sign-up failed. Please try again.';
        console.error('Sign-up error', err);
      }
    });
  }
}
