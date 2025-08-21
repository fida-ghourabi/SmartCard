import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CarteService } from '../../core/services/carte.service';
import { CardDto } from '../../models/card.model';

@Component({
  selector: 'app-cards',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cards.component.html',
  styleUrl: './cards.component.css'
})
export class CardsComponent implements OnInit {

  
  cards: CardDto[] = [];
  loading = false;
  error = '';

  constructor(private carteService: CarteService, private router: Router) {}

  ngOnInit(): void {
     this.loadCartes();
  }

  loadCartes(): void {
    this.loading = true;
    this.carteService.getMesCartes().subscribe({
      next: (cards) => {
        this.cards = cards;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Erreur lors du chargement des cartes';
        this.loading = false;
      }
    });

  }

  selectCard(card:  CardDto): void {
    this.router.navigate(['/dashboard/carte', card.id]);
  }

  getStatusClass(etat: CardDto['etatCarte']): string {
    switch (etat) {
      case 'Active': return 'status-active';
      case 'Bloque': return 'status-blocked';
      case 'Desactive': return 'status-disabled';
      default: return '';
    }
  }
  
  maskCardNumber(numeroCarte: string): string {
    const last4 = numeroCarte.slice(-4);
    return 'XXXX-XXXX-' + last4;
  }
}
