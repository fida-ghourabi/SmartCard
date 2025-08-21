import { Routes } from '@angular/router';
import { LoginComponent } from './modules/auth/login/login.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
      { path: '', component: LoginComponent },

      {
    path: 'dashboard',
    loadChildren: () =>
      import('./modules/dashboard/dashboard.routes').then(m => m.dashboardRoutes),
    canActivate: [authGuard]   // ✅ Protège tout le module dashboard
  },
  { path: '**', redirectTo: '' } // fallback

];
