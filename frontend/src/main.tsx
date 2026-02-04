// Imports React et ReactDOM
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
// Import des styles (Tailwind)
import "./index.css";
// Import de l'App
import App from "./App";
// Import des outils ReactQuery
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
// Import du Provider
import { AuthProvider } from "./context/AuthProvider";

// Création du client ReactQuery
const queryClient = new QueryClient();

// ======== Point d'éntrée de l'application
createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <AuthProvider>
      <QueryClientProvider client={queryClient}>
        <App />
      </QueryClientProvider>
    </AuthProvider>
  </StrictMode>,
);
