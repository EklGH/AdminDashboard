// Import de Navigate pour rediriger l'utilisateur
import { Navigate } from "react-router-dom";
// Import du hook useAuth pour gérer l’authentification
import { useAuth } from "../context/useAuth";
// Import du type ReactNode
import type { ReactNode } from "react";

// ======== Composant de Protection des routes privées
export default function ProtectedRoute({ children }: { children: ReactNode }) {
  // Récupère l'état d'authentification global
  const { isAuthenticated } = useAuth();

  // Si l'utilisateur n'est pas authentifié -> page Login
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  return children;
}
