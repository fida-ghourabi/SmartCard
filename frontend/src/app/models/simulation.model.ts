// src/app/models/simulation-withdrawal.model.ts

export interface SimulationDto {
  carteId: string;
  montant: number;
  type: string; // "Retrait"
  typeRetrait?: string; // "DAB" ou "GAB"
  lieu?: string;
  nomBanque?: string;
  compteDestinataire?: string;
  sourcePaiement?: string;
}

export interface SimulationResult {
  success: boolean;
  amount: number;
  currentBalance: number;
  newBalance: number;
  withdrawalLimit: number;
  withdrawalType: string;
  location?: string;
  bankName?: string;
  message: string;
}


