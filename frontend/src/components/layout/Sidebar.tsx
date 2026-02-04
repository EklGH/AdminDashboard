// Import du composant NavLink pour la navigation active
import { NavLink } from "react-router-dom";

// Variable de style réutilisable
const linkClass = "block px-4 py-2 rounded hover:bg-blue-600 hover:text-white";

// ======== Composant Sidebar
export default function Sidebar() {
  return (
    // Conteneur de la sidebar + liens
    <aside className="w-64 bg-gray-900 text-gray-200 p-4">
      <nav className="space-y-2">
        <NavLink to="/dashboard" className={linkClass}>
          Dashboard
        </NavLink>

        <NavLink to="/products" className={linkClass}>
          Products
        </NavLink>

        <NavLink to="/reservations" className={linkClass}>
          Reservations
        </NavLink>
      </nav>
    </aside>
  );
}
