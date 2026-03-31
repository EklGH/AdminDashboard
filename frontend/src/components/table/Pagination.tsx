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
  if (totalPages <= 1) return null;

  // Création d'un tableau de numéros de page
  const pages = Array.from({ length: totalPages }, (_, i) => i + 1);

  // ======== Rendu JSX de la Pagination
  return (
    <div className="flex items-center justify-center gap-2 mt-6">
      <button
        disabled={currentPage === 1}
        onClick={() => onPageChange(currentPage - 1)}
        className="px-3 py-1 rounded border disabled:opacity-40 hover:bg-gray-100"
      >
        ←
      </button>

      {pages.map((page) => (
        <button
          key={page}
          onClick={() => onPageChange(page)}
          className={`px-3 py-1 rounded border transition ${
            page === currentPage
              ? "bg-blue-600 text-white border-blue-600"
              : "hover:bg-gray-100"
          }`}
        >
          {page}
        </button>
      ))}

      <button
        disabled={currentPage === totalPages}
        onClick={() => onPageChange(currentPage + 1)}
        className="px-3 py-1 rounded border disabled:opacity-40 hover:bg-gray-100"
      >
        →
      </button>
    </div>
  );
}
