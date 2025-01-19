import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';  // Import Router

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router  // Inject Router
  ) {}

  onLogin(): void {
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        // Handle successful login, redirect or show success message
        console.log(response);
        console.log('Login successful');
        this.router.navigate(['/chat']);  // Navigate after successful login
      },
      error: (err) => {
        console.log(err);
        this.errorMessage = 'Invalid credentials. Please try again.';
      }
    });
  }
}
