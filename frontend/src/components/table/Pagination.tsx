// ======== Interface Pagination
interface PaginationProps {
  currentPage: number; // Page actuellement sélectionnée
  totalPages: number; // Nombre total de pages
  onPageChange: (page: number) => void; // Fonction appelée quand l'utilisateur change de page
}

// ======== Composant Pagination
export default function Pagination({
  currentPage,
  totalPages,
  onPageChange,
}: PaginationProps) {
  // Création d'un tableau de numéros de page
  const pages = Array.from({ length: totalPages }, (_, i) => i + 1);

  // ======== Rendu JSX de la Pagination
  return (
    // Conteneur flex pour les boutons de pagination
    <div className="flex space-x-2 mt-4">
      {pages.map((page) => (
        <button
          key={page}
          onClick={() => onPageChange(page)}
          className={`px-3 py-1 rounded border ${page === currentPage ? "bg-blue-500 text-white" : ""}`}
        >
          {page}
        </button>
      ))}
    </div>
  );
}
