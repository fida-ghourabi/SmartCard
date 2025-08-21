import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CardDto } from '../../../models/card.model';
import { SimulationService } from '../../../core/services/simulation.service';
import { CarteService } from '../../../core/services/carte.service';

import { SimulationDto } from '../../../models/simulation.model';
import { OtpVerificationComponent } from '../otp-verification/otp-verification.component';
@Component({
  selector: 'app-simulation-transfer',
  standalone: true,
  imports: [CommonModule, FormsModule, OtpVerificationComponent],
  templateUrl: './simulation-transfer.component.html',
  styleUrl: './simulation-transfer.component.css'
})
export class SimulationTransferComponent implements OnInit {
cards: CardDto[] = [];
  sourceCardId = '';
  destinationCompte = '';
amount: number | null = null;
  description = '';
  sourceCard: CardDto | null = null;
  //destinationCard: Card | null = null;
  simulationResult: any = null;
  isSimulating = false;
 //
 showOtpVerification = false;
  transactionData: any = null;
  transactionId: string | null = null;
 phoneNumber: string = localStorage.getItem('clientPhone') || '';  //

  constructor(private simulationService: SimulationService, private carteService: CarteService) {}

  

  ngOnInit(): void {
     this.carteService.getMesCartes().subscribe(cards => {
      this.cards = cards.filter(card => card.etatCarte === 'Active');
    });
  }

  onSourceCardChange(): void {
    this.sourceCard = this.cards.find(card => card.id === this.sourceCardId) || null;
    this.simulationResult = null;
    
  
  }

 
  simulateTransfer(): void {
    if (!this.sourceCard || !this.destinationCompte || !this.amount || this.amount <= 0) return;

    

  // Vérifier le solde disponible
  if (this.amount > this.sourceCard.solde) {
    this.simulationResult = {
      success: false,
      amount: this.amount,
      typeCarte : this.sourceCard.typeCarte,
      numCarte : this.sourceCard.numeroCarte,
      compteDestinataire: this.destinationCompte,
      //paymentLimit: this.selectedCard.plafondPaiement,
      currentBalance: this.sourceCard.solde,
      newBalance: this.sourceCard!.solde - this.amount,
      message: `Solde insuffisant. Solde actuel: ${this.sourceCard.solde.toFixed(2)} TND`
    };
    return;
  }

  // Si tout est OK, on continue la simulation

    this.isSimulating = true;
    
    



     const dto: SimulationDto = {
      carteId: this.sourceCard.id,
      montant: this.amount,
      type: 'Transfert',
      compteDestinataire: this.destinationCompte
    };
     
      this.simulationService.createTransaction(dto).subscribe({
      next: res => {
        this.transactionId = res.transactionId;
        this.simulationResult = {
          success: true,
          numeroCarte: this.sourceCard!.numeroCarte,

          amount: this.amount,
          typeCarte : this.sourceCard!.typeCarte,
          compteDestinataire: this.destinationCompte,
          //paymentLimit: this.sourceCard!.plafondPaiement,
          currentBalance: this.sourceCard!.solde,
          newBalance: this.sourceCard!.solde - (this.amount ?? 0),
          message: 'Transfert autorisé, veuillez saisir le code OTP'
        };
        this.showOtpVerification = true;
        this.isSimulating = false;
      },
      error: err => {
        this.simulationResult = {
          success: false,
          numeroCarte: this.sourceCard!.numeroCarte,
          amount: this.amount,
          typeCarte : this.sourceCard!.typeCarte,
          compteDestinataire: this.destinationCompte,
          //paymentLimit: this.sourceCard!.plafondPaiement,
          currentBalance: this.sourceCard!.solde,
          newBalance: this.sourceCard!.solde - (this.amount ?? 0),
          message: err.error?.message || 'Erreur lors de la simulation'
        };
        this.isSimulating = false;
      }
    });

  }



 


  onOtpVerified(otpCode: string): void {
    if (!this.transactionId) return;

    this.simulationService.validateOtp(this.transactionId, otpCode).subscribe({
      next: res => {
        if (this.simulationResult) {
          this.simulationResult.message = 'Transfert confirmé avec succès !';
          this.simulationResult.success = true;
        }
        this.showOtpVerification = false;
      },
      error: err => {
        if (this.simulationResult) {
          this.simulationResult.message = err.error?.message || 'Code OTP incorrect';
        }
      }
    });
  }

  onOtpCancelled(): void {
    this.showOtpVerification = false;
    if (this.simulationResult) {
      this.simulationResult.message = 'Transaction annulée par l\'utilisateur';
      this.simulationResult.success = false;
    }
  }

   resetSimulation(): void {
    this.amount = 0;
    this.destinationCompte = '';
    this.sourceCardId = '';
    this.sourceCard = null;
    this.simulationResult = null;
    this.transactionId = null;
    this.showOtpVerification = false;
  }

}
