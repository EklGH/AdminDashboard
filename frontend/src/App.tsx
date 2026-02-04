// Import des modules nécessaires de React Router
import { BrowserRouter } from "react-router-dom";
// Import du composant qui contient toutes les routes
import AppRoutes from "./routes/AppRoutes";

// ======== Composant racine de l'application
export default function App() {
  return (
    <BrowserRouter>
      <AppRoutes />
    </BrowserRouter>
  );
}
