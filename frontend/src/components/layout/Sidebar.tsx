// Import du composant NavLink pour la navigation active
import { NavLink } from "react-router-dom";

// Variable de style réutilisable
const linkClass = ({ isActive }: { isActive: boolean }) =>
  `block px-4 py-2 rounded transition-all duration-200 ${
    isActive ? "bg-blue-600 text-white" : "hover:bg-gray-800 text-gray-300"
  }`;

// ======== Composant Sidebar
export default function Sidebar() {
  // ======== Rendu JSX de la Sidebar
  return (
    <aside className="w-64 bg-gray-900 text-gray-200 p-4 hidden md:block">
      <div className="mb-6 text-lg font-bold text-white">AdminPanel</div>

      <nav className="space-y-2">
        <NavLink to="/dashboard" className={linkClass}>
          Tableau de bord
        </NavLink>

        <NavLink to="/products" className={linkClass}>
          Produits
        </NavLink>

        <NavLink to="/reservations" className={linkClass}>
          Réservations
        </NavLink>
      </nav>
    </aside>
  );
}
