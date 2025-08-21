import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
//import { Transaction, Card } from '../../models/card.model';
import { TransactionDto, CardLight } from '../../models/transaction.model';
import { TransactionService } from '../../core/services/transaction.service';
// import pdfmake
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';
// assignation correcte
(pdfMake as any).vfs = (pdfFonts as any).vfs;@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './history.component.html',
  styleUrl: './history.component.css'
})
export class HistoryComponent implements OnInit {

  // Données
  transactions = signal<TransactionDto[]>([]);
  filtered = signal<TransactionDto[]>([]);
  cards = signal<CardLight[]>([]);
   // Filtres / tri
  searchTerm = signal<string>('');
  selectedCardFilter = signal<string>('');     // on stocke l'ID carte (si dispo)
  selectedTypeFilter = signal<string>('');     // 'Retrait' | 'Paiement' | 'Transfert' ou ''
  sortBy = signal<'date' | 'montant' | 'type'>('date');
  sortOrder = signal<'asc' | 'desc'>('desc');

// Types disponibles
  transactionTypes: Array<'Retrait' | 'Paiement' | 'Transfert'> = ['Retrait', 'Paiement', 'Transfert'];
  constructor(private transactionService: TransactionService) {}
 totalMontant = computed(() =>
    this.filtered().reduce((acc, t) => acc + (t.montant ?? 0), 0)
  );

  ngOnInit(): void {
        this.loadData();

  }

  private loadData(): void {
    this.transactionService.getClientTransactions().subscribe({
      next: (tx) => {
        this.transactions.set(tx);
        this.filtered.set([...tx]);
        this.sortNow();
      },
      error: (err) => {
        console.error('Erreur transactions client:', err);
        this.transactions.set([]);
        this.filtered.set([]);
      }
    });

    this.transactionService.getClientCards().subscribe({
      next: (cards) => this.cards.set(cards),
      error: (err) => {
        console.warn('Impossible de récupérer les cartes client (optionnel):', err);
        this.cards.set([]);
      }
    });
  }

  // Recherche + filtres
  onSearch(): void {
    this.applyFilters();
  }

  onFilterChange(): void {
    this.applyFilters();
  }

  private applyFilters(): void {
    const term = (this.searchTerm() || '').toLowerCase().trim();
    const cardId = this.selectedCardFilter();
    const type = this.selectedTypeFilter();

    let list = [...this.transactions()];

    if (term) {
      list = list.filter(t =>
        (t.description ? t.description.toLowerCase().includes(term) : false) ||
        (t.type ?? '').toLowerCase().includes(term) ||
        (t.montant ?? 0).toString().includes(term) ||
        (t.lieu ?? '').toLowerCase().includes(term) ||
        (t.nomBanque ?? '').toLowerCase().includes(term) ||
        (t.typeRetrait ?? '').toLowerCase().includes(term) ||
        (t.sourcePaiement ?? '').toLowerCase().includes(term) ||
        (t.compteDestinataire ?? '').toLowerCase().includes(term) ||
        (t.numeroCarte ?? '').toLowerCase().includes(term)
      );
    }

    // Filtre carte (si tu veux filtrer par numéro de carte)
    if (cardId) {
      // Ici on suppose que cardId === id carte. Si l’API ne renvoie pas l’id, on peut filtrer par numeroCarte.
      // Adapte si besoin (ex: filtrer sur numeroCarte).
      const card = this.cards().find(c => c.id === cardId);
      if (card?.numeroCarte) {
        list = list.filter(t => t.numeroCarte === card.numeroCarte);
      } else {
        // fallback: aucun match si pas de numeroCarte
        list = [];
      }
    }

    if (type) {
      list = list.filter(t => t.type === type);
    }

    this.filtered.set(list);
    this.sortNow();
  }

  // Tri
  onSortChange(): void {
    this.sortNow();
  }

  private sortNow(): void {
    const by = this.sortBy();
    const order = this.sortOrder();
    const arr = [...this.filtered()];

    arr.sort((a, b) => {
      let comp = 0;
      switch (by) {
        case 'date':
          comp = new Date(a.date).getTime() - new Date(b.date).getTime();
          break;
        case 'montant':
          comp = (a.montant ?? 0) - (b.montant ?? 0);
          break;
        case 'type':
          comp = (a.type ?? '').localeCompare(b.type ?? '');
          break;
      }
      return order === 'desc' ? -comp : comp;
    });

    this.filtered.set(arr);
  }

  clearFilters(): void {
    this.searchTerm.set('');
    this.selectedCardFilter.set('');
    this.selectedTypeFilter.set('');
    this.filtered.set([...this.transactions()]);
    this.sortNow();
  }

  // Utilitaire pour afficher un libellé lisible de la carte sélectionnée
  getCardLabel(c: CardLight): string {
    // ex: "Visa - 123456789 - **** 1234"
    const suffix = c.numeroCarte ? c.numeroCarte.slice(-4) : '';
    return `${c.typeCarte ?? 'Carte'}  - xxxx xxxx xxxx ${suffix}`;
  }

 
    // ===== EXPORT PDF =====
  exportTransactionsPDF(): void {
  const rows = this.filtered().map(t => [
    t.type,
    new Date(t.date).toLocaleString(),
    (t.montant ?? 0).toFixed(2) + ' TND',
    t.lieu ?? '',
    t.nomBanque ?? '',
    t.typeRetrait ?? '',
    t.sourcePaiement ?? '',
    t.compteDestinataire ?? '',
    t.numeroCarte ?? '',
    t.typeCarte ?? ''
  ]);

  const docDefinition: any = {
    pageOrientation: 'landscape',
    content: [
      { text: 'Historique des Transactions', style: 'header' },
      { text: `Total : ${this.totalMontant().toFixed(2)} TND`, style: 'subheader' },
      {
        table: {
          headerRows: 1,
          widths: ['auto','auto','auto','*','*','*','*','*','*','*'],
          body: [
            [
              'Type', 'Date', 'Montant',
              'Lieu', 'Banque', 'Type Retrait',
              'Source Paiement', 'Compte Destinataire',
              'Numéro Carte', 'Type Carte'
            ],
            ...rows
          ]
        }
      }
    ],
    styles: {
      header: { fontSize: 18, bold: true, margin: [0,0,0,10] },
      subheader: { fontSize: 14, italics: true, margin: [0,0,0,10] }
    },
    defaultStyle: {
      fontSize: 9
    }
  };

  pdfMake.createPdf(docDefinition).download(
    `transactions_${new Date().toISOString().slice(0,10)}.pdf`
  );
}


}
