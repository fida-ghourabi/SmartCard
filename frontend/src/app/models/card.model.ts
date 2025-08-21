export interface CardDto {
  id: string;
  numeroCarte: string;
  nomPorteur: string;
  typeCarte: string;
  etatCarte: 'Active' | 'Desactive' | 'Bloque';
  dateCreation: string;   // ISO
  dateExpiration: string; // ISO
  plafondRetrait: number;
  plafondRetraitMax: number;
  plafondPaiement: number;
  plafondPaiementMax: number;
  imageUrl: string;
  numCompte: string;
  solde: number;

  clientNom: string;
  clientPrenom: string;
  clientEmail: string;
  clientTelephone: string;
}
export interface TransactionDto {
  id: string;
  date: string;
  type: string;
  description: string;
  amount: number;
}

export interface CardUpdateEtatDto {
  carteId: string;
  nouvelEtat: string;
}

export interface CardPlafondUpdateDto {
  carteId: string;
  nouveauPlafondRetrait: number;
  nouveauPlafondPaiement: number;
}
