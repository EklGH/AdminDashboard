// ======== Interface Product
export interface Product {
  id: number; // Id du produit
  name: string; // Nom du produit
  category: string; // Catégorie
  price: number; // Prix
  stock: number; // Quantité disponible
}

// ======== Interface Reservation
export interface Reservation {
  id: number; // Id de la réservation
  customer: string; // Nom du client
  date: string; // Date de la réservation (YYYY-MM-DD)
  status: "Confirmed" | "Pending" | "Cancelled"; // Statut actuel
}

// ======== Interface User
export interface User {
  email: string; // Adresse email de l’utilisateur (identifiant)
}

// ======== Interface AuthContextType
export interface AuthContextType {
  user: User | null; // Utilisateur connecté, ou null si déconnecté
  login: (email: string, password: string) => Promise<void>; // Fonction pour connecter un utilisateur
  logout: () => void; // Fonction pour déconnecter l’utilisateur
  isAuthenticated: boolean; // Indique si l’utilisateur est connecté
}

// ======== Interface LoginResponse (réponse login simulé)
export interface LoginResponse {
  user: { email: string }; // Info utilisateur
  token: string; // Token fictif d'authentification (fake JWT)
}
