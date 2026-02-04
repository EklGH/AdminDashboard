// Import du hook useContext de React
import { useContext } from "react";
// Import du Context
import { AuthContext } from "./AuthContext";

// ======== Hook AuthContext
export function useAuth() {
  // Récupère la valeur du AuthContext
  const ctx = useContext(AuthContext);

  // Vérifie que le hook est utilisé dans un Provider
  if (!ctx) {
    throw new Error(
      "useAuth doit être utilisé à l'intérieur d'un AuthProvider",
    );
  }
  return ctx;
}
