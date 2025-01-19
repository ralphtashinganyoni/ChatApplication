import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/Auth/sign-in`, { username, password }).pipe(
      tap(response => {
        console.log(response);
        // Save the JWT token in localStorage upon successful login
        localStorage.setItem('access_token', response.token);
        localStorage.setItem('user_id', response.applicationUser.id);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('access_token');
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  signUp(username: string, email: string, password: string): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/Auth/sign-up`, { username, email, password });
}
getCurrentUser(): string {
  return localStorage.getItem('user_id') || '';
}
}
