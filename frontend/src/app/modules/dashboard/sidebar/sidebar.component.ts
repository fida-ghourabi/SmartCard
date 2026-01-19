import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  
  activeItem = 'dashboard';
  simulationExpanded = false;
  
 constructor(private authService: AuthService, private router: Router) {}  
  menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: '📊', route: '/dashboard' },
    { id: 'cards', label: 'Mes Cartes', icon: '💳', route: '/dashboard/cartes' },
    { 
      id: 'simulation', 
      label: 'Transaction', 
      icon: '🔄',
      hasSubmenu: true,
      submenu: [
        { id: 'simulation-withdrawal', label: 'Retrait', icon: '💰', route: '/dashboard/simulation/retrait' },
        { id: 'simulation-payment', label: 'Paiement', icon: '💳', route: '/dashboard/simulation/paiement' },
        { id: 'simulation-transfer', label: 'Transfert', icon: '🔄', route: '/dashboard/simulation/transfert' }
      ]
    },
    { id: 'history', label: 'Historique', icon: '📈', route: '/dashboard/historique' },
    { id: 'logout', label: 'Déconnexion', icon: '🚪', route: '/' }
  ];

  setActiveItem(itemId: string): void {
     if (itemId === 'logout') {
      this.logout();
      return;
    }

    this.activeItem = itemId;
    if (itemId === 'simulation') {
      this.simulationExpanded = !this.simulationExpanded;
    } else if (!itemId.startsWith('simulation-')) {
      this.simulationExpanded = false;
    }
  }

  logout(): void {
    this.authService.logout();  
    this.router.navigate(['/']); // redirection vers la page login
  }
}
