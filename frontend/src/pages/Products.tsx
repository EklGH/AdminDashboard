// Imports React
import { useState, useEffect } from "react";
// Import du hook useProductsGraphQL
import { usePaginatedProductsGraphQL } from "../hooks/graphql/useProductsGraphQL";
// Import du hook useProducts (REST)
import {
  useCreateProduct,
  useUpdateProduct,
  useDeleteProduct,
} from "../hooks/api/useProducts";
// Imports des Components (UI)
import DataTable from "../components/table/DataTable";
import Pagination from "../components/table/Pagination";
import ProductForm from "../components/forms/ProductForm";
import Loader from "../components/ui/Loader";
// Imports des types et DTOs
import type { Product, ProductCreateDto, ProductUpdateDto } from "../types";
import type { Column } from "../components/table/DataTable";

// ======== PAGE PRODUCTS ========
export default function Products() {
  // Pagination (page actuelle)
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 5;

  // Produit sélectionné pour être modifié
  const [editingProduct, setEditingProduct] = useState<Product | "new" | null>(
    null,
  );

  // Feedbacks utilisateur (success/error)
  const [status, setStatus] = useState<{
    type: "success" | "error";
    message: string;
  } | null>(null);

  // Hooks GraphQL/REST
  const {
    data: paginatedData,
    isLoading: isPaginatedLoading,
    error: paginatedError,
    refetch: refetchPaginated,
  } = usePaginatedProductsGraphQL({
    page: currentPage,
    pageSize,
  });
  const create = useCreateProduct();
  const update = useUpdateProduct();
  const remove = useDeleteProduct();

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
  const columns: Column<Product>[] = [
    { key: "id", label: "ID" },
    { key: "name", label: "Nom" },
    { key: "category", label: "Catégorie" },
    { key: "price", label: "Prix" },
    { key: "stock", label: "Stock" },
  ];

  // ======== CRUD REST
  // Update/create produit
  const handleSave = (
    dto: ProductCreateDto | ProductUpdateDto,
    id?: string,
  ) => {
    if (id) {
      update.mutate(
        { id, dto: dto as ProductUpdateDto },
        {
          onSuccess: () => {
            setStatus({ type: "success", message: "Produit mis à jour !" });
            setEditingProduct(null);
            refetchPaginated();
          },
          onError: () =>
            setStatus({ type: "error", message: "Échec de la mise à jour" }),
        },
      );
    } else {
      create.mutate(dto as ProductCreateDto, {
        onSuccess: () => {
          setStatus({ type: "success", message: "Produit créé !" });
          setEditingProduct(null);
          refetchPaginated();
        },
        onError: () =>
          setStatus({ type: "error", message: "Échec de la création" }),
      });
    }
  };

  // Suppression produit + ajuste pagination
  const handleDelete = (id: string) => {
    if (!confirm("Supprimer ce produit ?")) return;

    remove.mutate(id, {
      onSuccess: () => {
        setStatus({ type: "success", message: "Produit supprimé !" });
        if (paginatedData?.items.length === 1 && currentPage > 1) {
          setCurrentPage((p) => p - 1);
        }
        refetchPaginated();
      },
      onError: () =>
        setStatus({ type: "error", message: "Échec de la suppression" }),
    });
  };

  // ======== Feedbacks chargement des pages
  if (paginatedError) {
    return (
      <p className="text-red-500">Erreur lors du chargement des produits</p>
    );
  }

  // ======== Rendu JSX de la page
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Produits</h2>

      <button
        onClick={() => setEditingProduct("new")}
        className="mb-4 bg-green-600 text-white px-4 py-2 rounded shadow hover:bg-green-700"
      >
        + Ajouter un produit
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

      {editingProduct !== null && (
        <ProductForm
          key={editingProduct === "new" ? "new" : editingProduct.id}
          product={editingProduct === "new" ? null : editingProduct}
          onSubmit={handleSave}
          onCancel={() => setEditingProduct(null)}
          loading={isMutating}
        />
      )}

      {isMutating && (
        <div className="mb-2">
          <Loader message="Opération en cours..." />
        </div>
      )}

      <DataTable
        columns={columns}
        data={paginatedData?.items || []}
        loading={isPaginatedLoading}
        actions={(row) => (
          <div className="space-x-2">
            <button
              onClick={() => setEditingProduct(row)}
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
      {paginatedData?.items.length === 0 && !isPaginatedLoading && (
        <p className="text-gray-500 mt-4">Aucun produit disponible</p>
      )}

      <Pagination
        currentPage={currentPage}
        totalPages={Math.ceil((paginatedData?.totalItems || 0) / pageSize)}
        onPageChange={setCurrentPage}
      />
    </div>
  );
}
