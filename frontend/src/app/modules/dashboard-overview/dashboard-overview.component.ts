import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardDto} from '../../models/card.model';
import { DashboardService } from '../../core/services/dashboard.service';
import { CarteService } from '../../core/services/carte.service';
import { TransactionDto } from '../../models/transaction.model';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard-overview',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard-overview.component.html',
  styleUrl: './dashboard-overview.component.css'
})
export class DashboardOverviewComponent implements OnInit {
 client = localStorage.getItem('clientPrenom')+ ' '+ localStorage.getItem('clientNom');
  cards: CardDto[] = [];
  totalBalance = 0;
  activeCards = 0;
  
  lastTransaction: TransactionDto | null = null;

  constructor(private carteService: CarteService,private dashboardService: DashboardService) {}

  ngOnInit(): void {
   

    this.carteService.getMesCartes().subscribe(cards => {
      this.cards = cards;
      
    });

    

   this.dashboardService.getCarteStats().subscribe(stats => {
      this.activeCards = stats.cartesActives;
    });

      this.dashboardService.getSoldeTotal().subscribe(solde => {
    this.totalBalance = solde;
  });

   // Dernière transaction
    this.dashboardService.getLastTransaction().subscribe(tx => {
      this.lastTransaction = tx;
    });
  }

  getGreeting(): string {
    const hour = new Date().getHours();
    if (hour < 12) return 'Bonjour';
    if (hour < 18) return 'Bon après-midi';
    return 'Bonsoir';
  }

  getCurrentDate(): string {
    return new Date().toLocaleDateString('fr-FR', {
      weekday: 'long',
      day: '2-digit',
      month: 'long',
      year: 'numeric'
    });
  }

  

  getStatusClass(status: string): string {
    return status.toLowerCase().replace(' ', '-');
  }
}
