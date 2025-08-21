import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7137/api/Auth';
  
  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<any> {
    const body = { email, password };
    return this.http.post<any>(`${this.apiUrl}/login`, body).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem('authToken', response.token);
          localStorage.setItem('clientNom', response.nom);
          localStorage.setItem('clientPrenom', response.prenom); 
          localStorage.setItem('clientPhone', response.phone);
          console.log('Login réussi :', response.nom, response.prenom);
          console.log('Login réussi, token stocké :', response.token);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('clientNom');
    localStorage.removeItem('clientPrenom');
    localStorage.removeItem('clientPhone');

  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }






}
