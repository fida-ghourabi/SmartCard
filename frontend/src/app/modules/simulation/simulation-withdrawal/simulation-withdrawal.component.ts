import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimulationService } from '../../../core/services/simulation.service';
import { CardDto } from '../../../models/card.model';
import { SimulationDto, SimulationResult } from '../../../models/simulation.model';
import { CarteService } from '../../../core/services/carte.service';

@Component({
  selector: 'app-simulation-withdrawal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './simulation-withdrawal.component.html',
  styleUrl: './simulation-withdrawal.component.css'
})
export class SimulationWithdrawalComponent implements OnInit {
cards: CardDto[] = [];
  selectedCardId = '';
amount: number | null = null;
  selectedCard: CardDto | null = null;
  simulationResult: any = null;
  isSimulating = false;
  withdrawalType = '';
location = '';
bankName = '';

withdrawalTypes = [
  { value: 'DAB', label: 'DAB (Distributeur STB)' },
  { value: 'GAB', label: 'GAB (Autre Banque)' }
];

locations = [
  'Tunis Centre Ville', 'Lafayette', 'El Menzah 6', 'Ennasr', 'Lac 1', 'Lac 2',
  'Manar', 'La Marsa', 'Ariana', 'Ben Arous', 'Bizerte', 'Nabeul',
  'Sousse', 'Monastir', 'Mahdia', 'Sfax', 'Gabès', 'Médenine',
  'Gafsa', 'Kairouan'
];

banks = [
  'BIAT', 'UIB', 'Attijari', 'Zitouna', 'BNA', 'Amen Bank',
  'STB', 'Al Baraka', 'Banque de Tunisie (BT)', 'UBCI', 'QNB Tunisie', 'La Poste Tunisienne'
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

  simulateWithdrawal(): void {
    if (!this.selectedCard || !this.amount|| this.amount <= 0) return;
    
    // Vérification du plafond et du solde
  if (this.amount > this.selectedCard.plafondRetrait) {
    this.simulationResult = {
      success: false,
      amount: this.amount,
      currentBalance: this.selectedCard.solde,
      newBalance: this.selectedCard.solde,
      withdrawalLimit: this.selectedCard.plafondRetrait,
      withdrawalType: this.withdrawalType,
      location: this.location,
      bankName: this.withdrawalType === 'GAB' ? this.bankName : 'STB',
      message: 'Montant supérieur au plafond de retrait'
    };
    return;
  }

  if (this.amount > this.selectedCard.solde) {
    this.simulationResult = {
      success: false,
      amount: this.amount,
      currentBalance: this.selectedCard.solde,
      newBalance: this.selectedCard.solde,
      withdrawalLimit: this.selectedCard.plafondRetrait,
      withdrawalType: this.withdrawalType,
      location: this.location,
      bankName: this.withdrawalType === 'GAB' ? this.bankName : 'STB',
      message: 'Solde insuffisant'
    };
    return;
  }

  // Si tout est OK, on continue la simulation avec l'API
    this.isSimulating = true;
    
       const dto: SimulationDto = {
      carteId: this.selectedCard.id,
      montant: this.amount,
      type: 'Retrait',
      typeRetrait: this.withdrawalType,
      lieu: this.location,
      nomBanque: this.withdrawalType === 'GAB' ? this.bankName : 'STB'
    }; 


 

  
this.simulationService.createTransaction(dto).subscribe({
      next: (res) => {
        this.simulationResult = {
          success: true,
          amount: this.amount,
          currentBalance: this.selectedCard!.solde,
          newBalance: this.selectedCard!.solde - (this.amount ?? 0),
          withdrawalLimit: this.selectedCard!.plafondRetrait,
          withdrawalType: this.withdrawalType,
          location: this.location,
          bankName: this.withdrawalType === 'GAB' ? this.bankName : 'STB',
          message: 'Retrait autorisé. Transaction ID: ' + res.transactionId
        };
        this.isSimulating = false;
      },
      error: (err) => {
        this.simulationResult = {
          success: false,
          amount: this.amount,
          currentBalance: this.selectedCard!.solde,
          newBalance: this.selectedCard!.solde,
          withdrawalLimit: this.selectedCard!.plafondRetrait,
          withdrawalType: this.withdrawalType,
          location: this.location,
          bankName: this.withdrawalType === 'GAB' ? this.bankName : 'STB',
          message: err.error?.message || 'Erreur lors de la simulation du retrait'
        };
        this.isSimulating = false;
      }
    });
  }

resetSimulation(): void {
  this.amount = 0;
  this.selectedCardId = '';
  this.selectedCard = null;
  this.withdrawalType = '';
  this.location = '';
  this.bankName = '';
  this.simulationResult = null;
}
}
