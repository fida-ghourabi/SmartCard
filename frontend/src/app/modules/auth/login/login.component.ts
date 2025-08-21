import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
 loginData = {
    email: '',
    password: ''
  };
  
  isLoading = false;
  showPassword = false;
  errorMessage = '';
  successMessage = '';
  rememberMe = false;

  constructor(private router: Router, private authService: AuthService) {}

  onSubmit(): void {
    if (!this.loginData.email || !this.loginData.password) {
      this.errorMessage = 'Veuillez remplir tous les champs';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
     this.successMessage = '';

    this.authService.login(this.loginData.email, this.loginData.password).subscribe({
      next: () => {
        this.successMessage = 'Connexion réussie !';
        console.log('Connexion réussie pour', this.loginData.email);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Email ou mot de passe incorrect';
         console.error('Erreur login :', err);
      },
      complete: () => this.isLoading = false
    });
  }


  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onForgotPassword(): void {
    alert('Fonctionnalité de récupération de mot de passe à venir');
  }

  onCreateAccount(): void {
    alert('Contactez votre agence STB pour ouvrir un compte');
  }
}
