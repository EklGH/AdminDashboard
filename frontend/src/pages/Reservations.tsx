// Import du hook useState de React
import { useState } from "react";
// Import des composants table et pagination
import DataTable from "../components/table/DataTable";
import Pagination from "../components/table/Pagination";
// Import des données mockées et types
import { reservationsMock } from "../services/mock/reservations.mock";
import type { Reservation } from "../types";
import type { Column } from "../components/table/DataTable";

// ======== PAGE RESERVATIONS ========
export default function Reservations() {
  // Hook useState pour gérer le CRUD local (mock)
  const [reservations /*setReservations*/] =
    useState<Reservation[]>(reservationsMock);

  // Pagination (page actuelle)
  const [currentPage, setCurrentPage] = useState(1);

  // Nombre d’éléments par page
  const pageSize = 2;

  // Colonnes de la DataTable
  const columns: Column<Reservation>[] = [
    { key: "id", label: "ID" },
    { key: "customer", label: "Customer" },
    { key: "date", label: "Date" },
    { key: "status", label: "Status" },
  ];

  // ======== Réservations à afficher sur la page courante
  const start = (currentPage - 1) * pageSize;
  const paginated = reservations.slice(start, start + pageSize);

  // ======== Rendu JSX de la page
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Reservations</h2>
      <DataTable
        columns={columns}
        data={paginated}
        actions={
          (/*row*/) => (
            <div className="space-x-2">
              <button className="px-2 py-1 bg-blue-500 text-white rounded">
                Edit
              </button>
              <button className="px-2 py-1 bg-red-500 text-white rounded">
                Delete
              </button>
            </div>
          )
        }
      />
      <Pagination
        currentPage={currentPage}
        totalPages={Math.ceil(reservations.length / pageSize)}
        onPageChange={setCurrentPage}
      />
    </div>
  );
}
