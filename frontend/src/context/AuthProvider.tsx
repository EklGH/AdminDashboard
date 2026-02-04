// Import de ReactNode
import type { ReactNode } from "react";
// Import React Hooks
import { useState } from "react";
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
  // Pour stocker l'utilisateur connecté
  const [user, setUser] = useState<User | null>(null);

  // Login mock : update les identifiants
  const login = async (email: string, password: string) => {
    // Vérifie si les identifiants sont valides
    if (email && password) {
      setUser({ email });
    } else {
      throw new Error("Identifiants invalides");
    }
  };

  // Logout : réinitialise l'utilisateur
  const logout = () => {
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
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
