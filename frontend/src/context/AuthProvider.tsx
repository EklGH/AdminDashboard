// Import de ReactNode
import type { ReactNode } from "react";
// Imports React Hooks
import { useState } from "react";
import { useEffect } from "react";
// Import du Context
import { AuthContext } from "./AuthContext";
// Import du type User
import type { User } from "../types";

// ======== Interface du Provider
interface AuthProviderProps {
  children: ReactNode;
}

// ======== Composant Provider
export function AuthProvider({ children }: AuthProviderProps) {
  // Stocke l'utilisateur connecté
  const [user, setUser] = useState<User | null>(null);
  // Indique si la restauration auth est en cours
  const [loading, setLoading] = useState(true);

  // ======== Restaure l'authentification au chargement
  useEffect(() => {
    const restoreAuth = () => {
      // Récupère le user sauvegardé
      const storedUser = localStorage.getItem("user");
      // Récupère le token mock
      const token = localStorage.getItem("token");

      // Si un utilisateur et un token existent, les restaure.
      if (storedUser && token) {
        setUser(JSON.parse(storedUser));
      }

      setLoading(false);
    };
    Promise.resolve().then(restoreAuth);
  }, []);

  // ======== Login mock : connecte l'utilisateur et sauvegarde ses infos
  const login = async (email: string, password: string) => {
    // Vérifie si email et mdp sont saisis
    if (!email || !password) {
      throw new Error("Identifiants invalides");
    }

    // Crée un utilisateur avec email
    const user: User = { email };

    // Update l'utilisateur connecté et sauvegarde ses infos
    setUser(user);
    localStorage.setItem("user", JSON.stringify(user));
    localStorage.setItem("token", "fake-jwt-token");
  };

  // ======== Logout et supprime les infos stockées
  const logout = () => {
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    setUser(null);
  };

  // ======== Rendu JSX du Provider
  return (
    <AuthContext.Provider
      value={{
        user,
        login,
        logout,
        isAuthenticated: !!user,
        loading,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
