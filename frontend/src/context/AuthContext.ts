// Import pour créer un Context React
import { createContext } from "react";
// Import du type AuthContextType
import type { AuthContextType } from "../types";

// ======== Création de AuthContext
export const AuthContext = createContext<AuthContextType | null>(null);
