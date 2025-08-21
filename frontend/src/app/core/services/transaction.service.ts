import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { TransactionDto, CardLight } from '../../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private readonly apiBase = 'https://localhost:7137/api';

  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }


  /** 🔹 Récupérer toutes les transactions du client connecté */
  getClientTransactions(): Observable<TransactionDto[]> {
    return this.http.get<TransactionDto[]>(
      `${this.apiBase}/Transactions/client`,
      { headers: this.getAuthHeaders() }
    );
  }

  /** 🔹 Récupérer la liste des cartes du client (pour filtrer/étiqueter) */
  getClientCards(): Observable<CardLight[]> {
    return this.http.get<any[]>(
      `${this.apiBase}/Cartes/mescartes`,
      { headers: this.getAuthHeaders() }
    ).pipe(
      map(cards => cards.map(c => ({
        id: c.id,
        typeCarte: c.typeCarte,
        numCompte: c.numCompte,
        numeroCarte: c.numeroCarte
      } as CardLight)))
    );
  }

}

