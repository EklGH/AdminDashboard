// Import du hook useAuth
import { useAuth } from "../../context/useAuth";
// Import du hook useNavigate pour naviguer entre les routes
import { useNavigate } from "react-router-dom";

// ======== Composant Navbar
export default function Navbar() {
  // Récupère l'utilisateur connecté et la fonction logout
  const { user, logout } = useAuth();

  // Hook pour naviguer entre les routes
  const navigate = useNavigate();

  // Déconnecte l'utilisateur et redirige vers la page Login
  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  // ======== Rendu JSX de la Navbar
  return (
    // Conteneur navbar : titre à gauche, user + logout à droite
    <header className="h-14 bg-white border-b flex items-center justify-between px-6">
      <h1 className="font-bold italic text-lg">AdminDashboard</h1>

      {user && (
        <div className="flex items-center gap-4">
          <span className="text-sm text-gray-600 truncate max-w-xs">
            {user.email}
          </span>

          <button
            onClick={handleLogout}
            className="text-sm text-red-600 hover:text-red-800"
          >
            Déconnexion
          </button>
        </div>
      )}
    </header>
  );
}
