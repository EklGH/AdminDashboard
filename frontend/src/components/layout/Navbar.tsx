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
    // Titre à gauche, user + logout à droite
    <header className="h-14 bg-white border-b flex items-center justify-between px-6 shadow-sm">
      <h1 className="font-bold text-lg tracking-tight">AdminDashboard</h1>

      {user && (
        <div className="flex items-center gap-4">
          <div className="text-right">
            <p className="text-sm font-medium">{user.email}</p>
            <p className="text-xs text-gray-400">Administrateur</p>
          </div>

          <button
            onClick={handleLogout}
            className="text-sm px-3 py-1 rounded bg-red-50 text-red-600 hover:bg-red-100 transition"
          >
            Déconnexion
          </button>
        </div>
      )}
    </header>
  );
}
