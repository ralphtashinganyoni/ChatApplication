import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {


  constructor(private http: HttpClient) {}

  getUsers(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/users`);
}

getRecentMessages(): Observable<any> {
  return this.http.get<any>(`${environment.apiUrl}/messages`);
}
}
