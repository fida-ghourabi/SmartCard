export type TransactionType = 'Retrait' | 'Paiement' | 'Transfert';

export interface TransactionDto {
  id: string;
  type: TransactionType;        // 'Retrait' | 'Paiement' | 'Transfert'
  date: string;                 // ISO string
  montant: number;              // montant de la transaction

  // Champs selon le type
  lieu?: string;
  nomBanque?: string;
  typeRetrait?: string;
  sourcePaiement?: string;
  compteDestinataire?: string;

  // Infos de la carte liée (optionnel)
  numeroCarte?: string;
  typeCarte?: string;
  soldeCompte?: number;
  description?: string;         
}

/** Interface légère pour les cartes dans le filtre */
export interface CardLight {
  id: string;
  typeCarte: string;
  numCompte: string;
  numeroCarte: string;
}