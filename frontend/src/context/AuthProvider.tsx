// Imports des hooks React
import { useEffect, useState } from "react";
// Import du Context
import { AuthContext } from "./AuthContext";
// Imports des types et DTOs
import type {
  AuthContextType,
  User,
  LoginRequest,
  RegisterRequest,
  RefreshTokenRequest,
} from "../types";
// Import du vrai login
import { authApi } from "../services/api/auth.api";

// ======== Interface du Provider
interface Props {
  children: React.ReactNode;
}

// ======== Provider d'authentification
export const AuthProvider = ({ children }: Props) => {
  // State utilisateur connecté
  const [user, setUser] = useState<User | null>(null);
  // State de chargement (restauration de session)
  const [loading, setLoading] = useState(true);

  // Login
  const login = async (dto: LoginRequest): Promise<void> => {
    const data = await authApi.login(dto);

    setUser(data.user);
  };

  // Register avec login automatique après inscription
  const register = async (dto: RegisterRequest): Promise<void> => {
    await authApi.register(dto);

    await login({
      email: dto.email,
      password: dto.password,
    });
  };

  // Refresh Token
  const refreshToken = async (dto: RefreshTokenRequest): Promise<void> => {
    const data = await authApi.refreshToken(dto);

    setUser(data.user);
  };

  // Logout
  const logout = (): void => {
    authApi.logout();
    setUser(null);
  };

  // Restore session (token présent = authentifié)
  useEffect(() => {
    const restoreAuth = async () => {
      try {
        const token = authApi.getToken();

        if (!token) {
          setLoading(false);
          return;
        }

        setUser({ email: "authenticated@user" });
      } catch (error) {
        console.error("Auth restore failed", error);
        authApi.logout();
        setUser(null);
      } finally {
        setLoading(false);
      }
    };

    restoreAuth();
  }, []);

  // Valeur exposée par le AuthContext
  const value: AuthContextType = {
    user,
    login,
    register,
    logout,
    refreshToken,
    isAuthenticated: !!user,
    loading,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
