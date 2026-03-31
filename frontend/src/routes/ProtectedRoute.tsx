// Import de Navigate pour rediriger l'utilisateur
import { Navigate, useLocation } from "react-router-dom";
// Import du hook useAuth pour gérer l’authentification
import { useAuth } from "../context/useAuth";
// Import du type ReactNode
import type { ReactNode } from "react";

// ======== Composant de Protection des routes privées
export default function ProtectedRoute({ children }: { children: ReactNode }) {
  // Récupère l'état d'authentification global et l'état de chargement
  const { isAuthenticated, loading } = useAuth();
  const location = useLocation();

  // Affiche un loader si l'état d'auth non déterminé
  if (loading) {
    return (
      <div className="h-screen flex items-center justify-center">
        <span className="text-gray-500">Loading...</span>
      </div>
    );
  }

  // Si l'utilisateur n'est pas authentifié
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }
  return children;
}
