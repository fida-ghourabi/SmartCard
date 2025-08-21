import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CarteService } from '../../../core/services/carte.service';
import { CardDto,  CardUpdateEtatDto, CardPlafondUpdateDto } from '../../../models/card.model';
import { TransactionDto } from '../../../models/transaction.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-card-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './card-detail.component.html',
  styleUrl: './card-detail.component.css'
})
export class CardDetailComponent implements OnInit {
  card?: CardDto;
  transactions: TransactionDto[] = [];
 

  editMode = false;
  tempWithdrawalLimit = 0;
  tempPaymentLimit = 0;

  constructor(private route: ActivatedRoute, private carteService: CarteService, private router: Router) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    // Version simple : on recharge la liste et on trouve la carte (sinon crée un /{id} côté API)
    this.carteService.getMesCartes().subscribe(cards => {
      this.card = cards.find(c => c.id === id);
      if (this.card) {
        this.tempWithdrawalLimit = Number(this.card.plafondRetrait);
        this.tempPaymentLimit = Number(this.card.plafondPaiement);

        // Transactions (si endpoint dispo)
        this.carteService.getTransactions(this.card.id).subscribe({
           next: (transactions) => this.transactions = transactions.slice(0, 5),
           error: () => this.transactions = []
        });
      }
    });
  }

  goBack(): void {
    window.history.back();
  }

  viewAllTransactions(): void {
    if (!this.card) return;
  this.router.navigate(['/dashboard/historique'], {
    queryParams: { carteId: this.card.id }   // tu passes l'id de la carte en paramètre
  });
  }

  toggleEditMode(): void {
    if (!this.card) return;
    
    this.editMode = !this.editMode;
    if (!this.editMode) {
      this.tempWithdrawalLimit = Number(this.card.plafondRetrait);
      this.tempPaymentLimit = Number(this.card.plafondPaiement);
    }
  }

  saveLimits(): void {
    if (!this.card) return;
    const payload: CardPlafondUpdateDto = {
      carteId: this.card.id,
      nouveauPlafondRetrait: this.tempWithdrawalLimit,
      nouveauPlafondPaiement: this.tempPaymentLimit
    };

    this.carteService.updatePlafonds(payload).subscribe({
      next: () => {
        this.card!.plafondRetrait = this.tempWithdrawalLimit;
        this.card!.plafondPaiement = this.tempPaymentLimit;
        this.editMode = false;
      },
      error: (err) => console.error('Erreur update plafonds:', err)
    });
  }

  toggleCardStatus(): void {
    if (!this.card) return;
    
    const newStatus = this.card.etatCarte === 'Active' ? 'Desactive' : 'Active';
     const payload2: CardUpdateEtatDto = {
      carteId: this.card.id,
      nouvelEtat: newStatus
     
    };
    this.carteService.modifierEtatCarte(payload2)
      .subscribe({
        next: () => this.card!.etatCarte = newStatus,
        error: (err) => console.error('Erreur update status:', err)
      });
  }

  blockCard(): void {
    if (!this.card) return;
    
    this.carteService.modifierEtatCarte({ carteId: this.card.id, nouvelEtat: 'Bloque' })
      .subscribe({
        next: () => this.card!.etatCarte = 'Bloque',
        error: (err) => console.error('Erreur block card:', err)
      });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Active': return 'status-active';
      case 'Bloque': return 'status-blocked';
      case 'Desactive': return 'status-disabled';
      default: return '';
    }
  }
   maskCardNumber(fullNumber: string): string {
    const last4 = fullNumber.slice(-4);
    return 'XXXX-XXXX-' + last4;
  }

    // Retourne la description selon le type de transaction
  getTransactionDesc(transaction: TransactionDto): string {
  switch (transaction.type) {
    case 'Paiement':
      return transaction.sourcePaiement ?? '';
    case 'Transfert':
      return transaction.compteDestinataire ?? '';
    case 'Retrait':
      return `${transaction.typeRetrait ?? ''} - ${transaction.nomBanque ?? ''} - ${transaction.lieu ?? ''}`;
    default:
      return '';
  }
}

}
