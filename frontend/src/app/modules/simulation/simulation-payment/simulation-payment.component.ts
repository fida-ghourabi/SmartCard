import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CardDto } from '../../../models/card.model';
import { SimulationService } from '../../../core/services/simulation.service';
import { CarteService } from '../../../core/services/carte.service';

import { SimulationDto } from '../../../models/simulation.model';
import { OtpVerificationComponent } from '../otp-verification/otp-verification.component';
@Component({
  selector: 'app-simulation-payment',
  standalone: true,
  imports: [CommonModule, FormsModule, OtpVerificationComponent],
  templateUrl: './simulation-payment.component.html',
  styleUrl: './simulation-payment.component.css'
})
export class SimulationPaymentComponent implements OnInit {
cards: CardDto[] = [];
  selectedCardId = '';
amount: number | null = null;
  merchant = '';
  selectedCard: CardDto | null = null;
  simulationResult: any = null;
  isSimulating = false;
  //
 showOtpVerification = false;
  transactionData: any = null;
  transactionId: string | null = null;
phoneNumber: string = localStorage.getItem('clientPhone') || '';  //
  merchantCategories = [
    'Restaurant',
    'Supermarché',
    'Station-service',
    'Pharmacie',
    'Vêtements',
    'Électronique',
    'Voyage',
    'Autre'
  ];

  constructor(private simulationService: SimulationService, private carteService: CarteService) {}

  ngOnInit(): void {
     this.carteService.getMesCartes().subscribe(cards => {
      this.cards = cards.filter(card => card.etatCarte === 'Active');
    });
  }

  onCardChange(): void {
    this.selectedCard = this.cards.find(card => card.id === this.selectedCardId) || null;
    this.simulationResult = null;
  }

  simulatePayment(): void {
    if (!this.selectedCard || !this.amount || this.amount <= 0 || !this.merchant.trim()) return;
    


      // Vérifier le plafond de paiement
  if (this.amount > this.selectedCard.plafondPaiement) {
    this.simulationResult = {
      success: false,
      amount: this.amount,
      merchant: this.merchant,
      paymentLimit: this.selectedCard.plafondPaiement,
      currentBalance: this.selectedCard.solde,
      newBalance: this.selectedCard.solde,
      message: `Le montant dépasse le plafond de paiement autorisé (${this.selectedCard.plafondPaiement.toFixed(2)} TND)`
    };
    return;
  }

  // Vérifier le solde disponible
  if (this.amount > this.selectedCard.solde) {
    this.simulationResult = {
      success: false,
      amount: this.amount,
      merchant: this.merchant,
      paymentLimit: this.selectedCard.plafondPaiement,
      currentBalance: this.selectedCard.solde,
      newBalance: this.selectedCard.solde,
      message: `Solde insuffisant. Solde actuel: ${this.selectedCard.solde.toFixed(2)} TND`
    };
    return;
  }

  // Si tout est OK, on continue la simulation

    this.isSimulating = true;
    
    
    const dto: SimulationDto = {
      carteId: this.selectedCard.id,
      montant: this.amount,
      type: 'Paiement',
      sourcePaiement: this.merchant
    };
     
      this.simulationService.createTransaction(dto).subscribe({
      next: res => {
        this.transactionId = res.transactionId;
        this.simulationResult = {
          success: true,
          amount: this.amount,
          merchant: this.merchant,
          paymentLimit: this.selectedCard!.plafondPaiement,
          currentBalance: this.selectedCard!.solde,
          newBalance: this.selectedCard!.solde - (this.amount ?? 0),
          message: 'Paiement autorisé, veuillez saisir le code OTP'
        };
        this.showOtpVerification = true;
        this.isSimulating = false;
      },
      error: err => {
        this.simulationResult = {
          success: false,
          amount: this.amount,
          merchant: this.merchant,
          paymentLimit: this.selectedCard!.plafondPaiement,
          currentBalance: this.selectedCard!.solde,
          newBalance: this.selectedCard!.solde,
          message: err.error?.message || 'Erreur lors de la simulation'
        };
        this.isSimulating = false;
      }
    });
  }

////


   onOtpVerified(otpCode: string): void {
    if (!this.transactionId) return;

    this.simulationService.validateOtp(this.transactionId, otpCode).subscribe({
      next: res => {
        if (this.simulationResult) {
          this.simulationResult.message = 'Paiement confirmé avec succès !';
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
    this.merchant = '';
    this.selectedCardId = '';
    this.selectedCard = null;
    this.simulationResult = null;
    this.transactionId = null;
    this.showOtpVerification = false;
  }
}
