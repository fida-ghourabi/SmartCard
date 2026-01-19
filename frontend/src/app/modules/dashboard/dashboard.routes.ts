import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { DashboardOverviewComponent } from '../dashboard-overview/dashboard-overview.component';
import { CardsComponent } from '../cards/cards.component';
import { CardDetailComponent } from '../cards/card-detail/card-detail.component';
import { HistoryComponent } from '../history/history.component';
import { SimulationWithdrawalComponent } from '../simulation/simulation-withdrawal/simulation-withdrawal.component';
import { SimulationPaymentComponent } from '../simulation/simulation-payment/simulation-payment.component';
import { SimulationTransferComponent } from '../simulation/simulation-transfer/simulation-transfer.component';
import { ChatbotComponent } from '../chatbot/chatbot.component';

export const dashboardRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    children: [
      { path: '', component: DashboardOverviewComponent },
      { path: 'cartes', component: CardsComponent },
      { path: 'carte/:id', component: CardDetailComponent },
      { path: 'historique', component: HistoryComponent },
      { path: 'simulation/retrait', component: SimulationWithdrawalComponent },
      { path: 'simulation/paiement', component: SimulationPaymentComponent },
      { path: 'simulation/transfert', component: SimulationTransferComponent },
      { path: 'chat', component: ChatbotComponent },

    ]
  }
];