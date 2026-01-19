import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TransactionDto  } from '../../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'https://localhost:7137/api'; 
  constructor(private http: HttpClient) { }



  // 🔹 Récup solde total
  getSoldeTotal(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/comptes/solde-total`);
  }

  getLastTransaction(): Observable<TransactionDto> {
    return this.http.get<TransactionDto>(`${this.apiUrl}/transactions/dernier`);
  }



  getCarteStats(): Observable<{ clientId: string, totalCartes: number, cartesActives: number }> {
    return this.http.get<{ clientId: string, totalCartes: number, cartesActives: number }>(
      `${this.apiUrl}/cartes/stats`
   );
  }

}
