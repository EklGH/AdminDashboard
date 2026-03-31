// ======== Interface Product
export interface Product {
  id: string; // Id du produit
  name: string; // Nom du produit
  category: string; // Catégorie
  price: number; // Prix
  stock: number; // Quantité disponible
}

// ======== Interface Paginated Products (GraphQL)
export interface PaginatedProducts {
  items: Product[];
  page: number;
  pageSize: number;
  totalItems: number;
}

// ======== Interface Reservation
export interface Reservation {
  id: string; // Id de la réservation
  customer: string; // Nom du client
  date: string; // Date de la réservation (YYYY-MM-DD)
  status: "Confirmed" | "Pending" | "Cancelled"; // Statut actuel
  productId: string; // UUID du produit réservé
  userId?: string | null; // UUID de l’utilisateur, nullable
}

// ======== Interface Paginated Reservations
export interface PaginatedReservations {
  items: Reservation[];
  page: number;
  pageSize: number;
  totalItems: number;
}

// ======== Interface User
export interface User {
  email: string; // Adresse email de l’utilisateur (identifiant)
}

// ======== Interface AuthContextType
export interface AuthContextType {
  user: User | null; // Utilisateur connecté, ou null si déconnecté
  login: (dto: LoginRequest) => Promise<void>; // Fonction pour connecter un utilisateur
  register: (dto: RegisterRequest) => Promise<void>; // Fonction register
  logout: () => void; // Fonction pour déconnecter l’utilisateur
  refreshToken: (dto: RefreshTokenRequest) => Promise<void>; // Rafraîchissement token
  isAuthenticated: boolean; // Indique si l’utilisateur est connecté
  loading: boolean; // Indique si la restauration auth est en cours
}

// ======== Interface LoginResponse (réponse login simulé)
export interface LoginResponse {
  user: User; // Info utilisateur
  token: string; // JWT
}

// ======== Interface RegisterRequest
export interface RegisterRequest {
  email: string;
  password: string;
}

// ======== Interface LoginRequest
export interface LoginRequest {
  email: string;
  password: string;
}

// ======== Interface RefreshTokenRequest
export interface RefreshTokenRequest {
  refreshToken: string;
}

// ======== Interface ProductCreateDto
export interface ProductCreateDto {
  name: string;
  category: string;
  price: number;
  stock: number;
}

// ======== Interface ProductUpdateDto
export interface ProductUpdateDto {
  name: string;
  category: string;
  price: number;
  stock: number;
}

// ======== Interface ReservationCreateDto
export interface ReservationCreateDto {
  customer: string;
  date: string; // YYYY-MM-DD
  status: "Confirmed" | "Pending" | "Cancelled";
  productId: string;
  userId?: string | null;
}

// ======== Interface ReservationUpdateDto
export interface ReservationUpdateDto {
  customer: string;
  date: string;
  status: "Confirmed" | "Pending" | "Cancelled";
  productId: string;
  userId?: string | null;
}

// ======== Interface ProductFilter (GraphQL)
export interface ProductFilter {
  id?: { eq?: string; neq?: string };
  name?: { contains?: string; eq?: string };
  category?: { eq?: string; contains?: string };
  price?: { eq?: number; gt?: number; lt?: number };
  stock?: { eq?: number; gt?: number; lt?: number };
  and?: ProductFilter[];
  or?: ProductFilter[];
}
