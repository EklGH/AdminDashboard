// Imports React
import { useState, useEffect } from "react";
// Imports des hooks (useReservations et useProducts)
import {
  usePaginatedReservations,
  useCreateReservation,
  useUpdateReservation,
  useDeleteReservation,
} from "../hooks/api/useReservations";
import { usePaginatedProductsGraphQL } from "../hooks/graphql/useProductsGraphQL";
// Imports des Components (UI)
import DataTable from "../components/table/DataTable";
import Pagination from "../components/table/Pagination";
import ReservationForm from "../components/forms/ReservationForm";
import Loader from "../components/ui/Loader";
// Imports types et DTOs
import type {
  Reservation,
  ReservationCreateDto,
  ReservationUpdateDto,
  Product,
} from "../types";
import type { Column } from "../components/table/DataTable";

// ======== PAGE RESERVATIONS ========
export default function Reservations() {
  // Pagination (page actuelle)
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 5;

  // Réservation sélectionnée pour être modifiée
  const [editingReservation, setEditingReservation] = useState<
    Reservation | "new" | null
  >(null);

  // Feedbacks utilisateur (success/error)
  const [status, setStatus] = useState<{
    type: "success" | "error";
    message: string;
  } | null>(null);

  // Hooks REST/GraphQL
  const { data, isLoading, error } = usePaginatedReservations(
    currentPage,
    pageSize,
  );
  const { data: productsData, isLoading: loadingProducts } =
    usePaginatedProductsGraphQL({
      page: 1,
      pageSize: 100,
      orderBy: "NAME_ASC",
    });
  const create = useCreateReservation();
  const update = useUpdateReservation();
  const remove = useDeleteReservation();

  // Détecte si une modification est en cours
  const isMutating = create.isPending || update.isPending || remove.isPending;

  // Auto disparition du message
  useEffect(() => {
    if (status) {
      const timer = setTimeout(() => setStatus(null), 3000);
      return () => clearTimeout(timer);
    }
  }, [status]);

  // Colonnes DataTable
  const columns: Column<Reservation>[] = [
    { key: "id", label: "ID" },
    { key: "customer", label: "Client" },
    { key: "date", label: "Date" },
    { key: "status", label: "Statut" },
    { key: "productId", label: "Produit" },
  ];

  // ======== CRUD
  // Update/create reservation
  const handleSave = (
    dto: ReservationCreateDto | ReservationUpdateDto,
    id?: string,
  ) => {
    if (id) {
      update.mutate(
        { id, dto: dto as ReservationUpdateDto },
        {
          onSuccess: () => {
            setStatus({
              type: "success",
              message: "Réservation mise à jour avec succès !",
            });
            setEditingReservation(null);
          },
          onError: () =>
            setStatus({
              type: "error",
              message: "Échec de la mise à jour",
            }),
        },
      );
    } else {
      create.mutate(dto as ReservationCreateDto, {
        onSuccess: () => {
          setStatus({
            type: "success",
            message: "Réservation créée avec succès !",
          });
          setEditingReservation(null);
        },
        onError: () =>
          setStatus({
            type: "error",
            message: "Échec de la création",
          }),
      });
    }
  };

  // Suppression reservation + ajuste pagination
  const handleDelete = (id: string) => {
    if (!confirm("Supprimer cette réservation ?")) return;

    remove.mutate(id, {
      onSuccess: () => {
        setStatus({
          type: "success",
          message: "Réservation supprimée avec succès !",
        });
        if (data?.items.length === 1 && currentPage > 1) {
          setCurrentPage((p) => p - 1);
        }
      },
      onError: () =>
        setStatus({
          type: "error",
          message: "Échec de la suppression",
        }),
    });
  };

  // ======== Mapping produits pour le formulaire
  const products =
    productsData?.items.map((p: Product) => ({
      id: p.id,
      name: p.name,
      stock: p.stock,
    })) || [];

  // ======== Feedbacks chargement des pages
  if (error) {
    return (
      <p className="text-red-500">Erreur lors du chargement des réservations</p>
    );
  }

  // ======== Rendu JSX de la page
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Réservations</h2>

      <button
        onClick={() => setEditingReservation("new")}
        className="mb-4 bg-green-600 text-white px-4 py-2 rounded shadow hover:bg-green-700"
      >
        + Ajouter une réservation
      </button>

      {status && (
        <div
          className={`mb-4 px-4 py-2 rounded ${
            status.type === "success"
              ? "bg-green-100 text-green-800"
              : "bg-red-100 text-red-800"
          }`}
        >
          {status.message}
        </div>
      )}

      {editingReservation !== null && (
        <ReservationForm
          key={editingReservation === "new" ? "new" : editingReservation.id}
          reservation={editingReservation === "new" ? null : editingReservation}
          onSubmit={handleSave}
          onCancel={() => setEditingReservation(null)}
          products={products}
          loadingProducts={loadingProducts}
        />
      )}

      {isMutating && (
        <div className="mb-2">
          <Loader message="Opération en cours..." />
        </div>
      )}

      <DataTable
        columns={columns}
        data={data?.items || []}
        loading={isLoading}
        actions={(row) => (
          <div className="space-x-2">
            <button
              onClick={() => setEditingReservation(row)}
              disabled={isMutating}
              className="px-2 py-1 bg-blue-500 text-white rounded shadow hover:bg-blue-600 disabled:opacity-50"
            >
              Modifier
            </button>

            <button
              onClick={() => handleDelete(row.id)}
              disabled={isMutating}
              className="px-2 py-1 bg-red-500 text-white rounded shadow hover:bg-red-600 disabled:opacity-50"
            >
              Supprimer
            </button>
          </div>
        )}
      />

      {data?.items.length === 0 && !isLoading && (
        <p className="text-gray-500 mt-4">Aucune réservation disponible</p>
      )}

      <Pagination
        currentPage={currentPage}
        totalPages={Math.ceil((data?.totalItems || 0) / pageSize)}
        onPageChange={setCurrentPage}
      />
    </div>
  );
}
