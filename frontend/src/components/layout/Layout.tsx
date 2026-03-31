// Import du composant Outlet (zone d'affichage des pages)
import { Outlet } from "react-router-dom";
// Import des composants de layout
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";

// ======== Composant Layout global
export default function Layout() {
  // ======== Rendu JSX du Layout
  return (
    // Conteneur principal en flex sur toute la hauteur de l'écran
    <div className="flex h-screen bg-gray-100">
      <Sidebar />

      <div className="flex flex-col flex-1 min-w-0">
        <Navbar />

        <main className="p-4 md:p-6 overflow-auto">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
