import { Injectable } from '@angular/core';
import { HttpClient , HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SimulationDto } from '../../models/simulation.model';
@Injectable({
  providedIn: 'root'
})
export class SimulationService {

  private apiUrl = 'https://localhost:7137/api'; 
  constructor(private http: HttpClient) { }


  

  // Créer une transaction de retrait en attente (simulation)
  createTransaction(dto: SimulationDto): Observable<{ transactionId: string }> {
    return this.http.post<{ transactionId: string }>(`${this.apiUrl}/TransactionsOtp/create-pending`, dto);
  }


  // Valider le code OTP pour confirmer la transaction
  validateOtp(transactionPendingId: string, otpCode: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiUrl}/TransactionsOtp/validate-otp`,
      { TransactionPendingId: transactionPendingId, OtpCode: otpCode }
    );
  }
}


