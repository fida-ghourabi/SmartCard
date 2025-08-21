import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-otp-verification',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './otp-verification.component.html',
  styleUrl: './otp-verification.component.css'
})
export class OtpVerificationComponent implements OnInit, OnDestroy {
 @Input() transactionData: any = null;
  @Input() phoneNumber: string = '';
  @Output() otpVerified = new EventEmitter<string>();
  @Output() cancel = new EventEmitter<void>();

  otpCode = ['', '', '', '', '', ''];
  isVerifying = false;
  errorMessage = '';
  successMessage = '';
  timeLeft = 300; // 5 minutes
  timerInterval: any;
  canResend = false;
  resendCooldown = 30;
  resendInterval: any;

  ngOnInit(): void {
    this.startTimer();
    this.startResendCooldown();
  }

  ngOnDestroy(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
    if (this.resendInterval) {
      clearInterval(this.resendInterval);
    }
  }

  startTimer(): void {
    this.timerInterval = setInterval(() => {
      this.timeLeft--;
      if (this.timeLeft <= 0) {
        clearInterval(this.timerInterval);
        this.errorMessage = 'Le code OTP a expiré. Veuillez recommencer la transaction.';
      }
    }, 1000);
  }

  startResendCooldown(): void {
    this.canResend = false;
    this.resendInterval = setInterval(() => {
      this.resendCooldown--;
      if (this.resendCooldown <= 0) {
        this.canResend = true;
        clearInterval(this.resendInterval);
      }
    }, 1000);
  }

onOtpInput(event: any, index: number): void {
  const value = event.target.value;

  // Accepter seulement un chiffre
  if (/^\d$/.test(value)) {
    this.otpCode[index] = value; // Mettre la valeur dans le champ exact
    this.errorMessage = '';
  } else if (value === '') {
    this.otpCode[index] = '';
  } else {
    // Saisie invalide
    this.otpCode[index] = '';
    event.target.value = '';
  }
}trackByIndex(index: number, item: string): number {
  return index; // Angular utilise l’index pour suivre chaque input
}


  onKeyDown(event: KeyboardEvent, index: number): void {
    if (event.key === 'Backspace' && this.otpCode[index] === '' && index > 0) {
      const prevInput = document.getElementById(`otp-${index - 1}`) as HTMLInputElement;
      if (prevInput) {
        prevInput.focus();
      }
    }
  }

  onPaste(event: ClipboardEvent): void {
    event.preventDefault();
    const pastedData = event.clipboardData?.getData('text') || '';
    const digits = pastedData.replace(/\D/g, '').slice(0, 6);
    
    for (let i = 0; i < 6; i++) {
      this.otpCode[i] = digits[i] || '';
      const input = document.getElementById(`otp-${i}`) as HTMLInputElement;
      if (input) {
        input.value = this.otpCode[i];
      }
    }
    
    
  }

  
   
   /** Bouton de validation */
  onVerifyClick(): void {
    const enteredCode = this.otpCode.join('');
    if (enteredCode.length !== 6) {
      this.errorMessage = 'Veuillez saisir le code complet à 6 chiffres';
      return;
    }

    this.isVerifying = true;
    this.errorMessage = '';
    this.otpVerified.emit(enteredCode);
  }

   resendOtp(): void {
    if(!this.canResend) return;
    this.resendCooldown=30;
    this.startResendCooldown();
    alert('Un nouveau code OTP a été envoyé !');
  }



    // Masquer une partie du numéro de téléphone (ex: 123****789)
getMaskedPhone(): string {
  if (!this.phoneNumber || this.phoneNumber.length < 4) return this.phoneNumber || '';
  const start = this.phoneNumber.slice(0, 3);
  const end = this.phoneNumber.slice(-3);
  return `${start}****${end}`;
}

isVerifyDisabled(): boolean {
  return this.otpCode.includes('') || this.isVerifying;
}

// Convertir les secondes en mm:ss
formatTime(seconds: number): string {
  const min = Math.floor(seconds / 60).toString().padStart(2, '0');
  const sec = (seconds % 60).toString().padStart(2, '0');
  return `${min}:${sec}`;
}
  onCancel(): void {
    this.cancel.emit();
  }
}
