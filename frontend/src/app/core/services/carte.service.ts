import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { CardDto,CardUpdateEtatDto, CardPlafondUpdateDto } from '../../models/card.model';
import { TransactionDto } from '../../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class CarteService {
 private readonly apiUrl = 'https://localhost:7137/api';
  constructor(private http: HttpClient) { }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

    /** Liste des cartes du client connecté */
  getMesCartes(): Observable<CardDto[]> {
    return this.http.get<CardDto[]>(`${this.apiUrl}/Cartes/mescartes`, {
      headers: this.getAuthHeaders()
    });
  }

  /** Récup transactions d’une carte (si endpoint dispo) */
  getTransactions(cardId: string): Observable<TransactionDto[]> {
    return this.http.get<TransactionDto[]>(`${this.apiUrl}/Transactions/carte/${cardId}`,{
    headers: this.getAuthHeaders()
  });
  }

 // 📌 Modifier l’état d’une carte
  modifierEtatCarte(dto: CardUpdateEtatDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cartes/etat`, dto, {
      headers: this.getAuthHeaders()
    });
  }

  // 📌 Modifier les plafonds d’une carte
  updatePlafonds(dto: CardPlafondUpdateDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/Cartes/plafonds`, dto, {
      headers: this.getAuthHeaders()
    });

  }

}
