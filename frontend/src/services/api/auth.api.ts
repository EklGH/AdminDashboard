// Import du client HTTP
import { apiClient } from "./apiClient";
// Imports des types et DTOs
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  User,
  RefreshTokenRequest,
} from "../../types";

// ======== Auth API
export const authApi = {
  // login
  login: async (dto: LoginRequest): Promise<LoginResponse> => {
    const res = await apiClient.post<LoginResponse>("/Auth/login", dto);
    const data = res.data;
    if (data.token) {
      localStorage.setItem("authToken", data.token);
    }

    return data;
  },

  // register
  register: async (dto: RegisterRequest): Promise<User> => {
    const res = await apiClient.post<User>("/Auth/register", dto);
    return res.data;
  },

  // refreshToken
  refreshToken: async (dto: RefreshTokenRequest): Promise<LoginResponse> => {
    const res = await apiClient.post<LoginResponse>("/Auth/refresh", dto);
    const data = res.data;

    if (data.token) {
      localStorage.setItem("authToken", data.token);
    }

    return data;
  },

  // logout
  logout: (): void => {
    localStorage.removeItem("authToken");
  },

  // getToken (Récupérer le token stocké)
  getToken: (): string | null => {
    return localStorage.getItem("authToken");
  },

  // isAuthenticated (vérifie si l'utilisateur est connecté)
  isAuthenticated: (): boolean => {
    return !!localStorage.getItem("authToken");
  },
};
