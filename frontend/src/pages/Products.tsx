// Import du hook useState de React
import { useState } from "react";
// Import des composants table et pagination
import DataTable from "../components/table/DataTable";
import Pagination from "../components/table/Pagination";
// Import du composant formulaire
import ProductForm from "../components/forms/ProductForm";
// Import du hook useProducts et de son retour
import { useProducts } from "../hooks/useProducts";
import type { UseProductsReturn } from "../hooks/useProducts";
// Import du type Column
import type { Column } from "../components/table/DataTable";

// Typage d'un produit directement depuis le hook useProducts
type ProductFromHook = NonNullable<UseProductsReturn["data"]>[number];

// ======== PAGE PRODUCTS ========
export default function Products() {
  // Hook useProducts pour gérer le CRUD
  const { data, isLoading, error, create, update, remove } = useProducts();
  // Pagination (page actuelle)
  const [currentPage, setCurrentPage] = useState(1);
  // Produit sélectionné pour être modifié
  const [editingProduct, setEditingProduct] = useState<ProductFromHook | null>(
    null,
  );
  // Nombre d’éléments par page
  const pageSize = 3;

  // Colonnes de la DataTable
  const columns: Column<ProductFromHook>[] = [
    { key: "id", label: "ID" },
    { key: "name", label: "Name" },
    { key: "category", label: "Category" },
    { key: "price", label: "Price" },
    { key: "stock", label: "Stock" },
  ];

  // Feedbacks pour le chargement des pages
  if (isLoading) return <p>Loading products...</p>;
  if (error) return <p className="text-red-500">Error loading products</p>;

  // Produits à afficher sur la page courante
  const start = (currentPage - 1) * pageSize;
  const paginated = data?.slice(start, start + pageSize) || [];

  // ======== CRUD
  // Création et update d’un produit
  const handleSave = (product: ProductFromHook) => {
    if (!product.id || product.id === 0) {
      // Create
      create.mutate(product);
    } else {
      // Update
      update.mutate(product);
    }
    setEditingProduct(null);
  };

  // Suppression d’un produit
  const handleDelete = (id: number) => {
    remove.mutate(id);
  };

  // ======== Rendu JSX de la page
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Products</h2>

      <ProductForm
        product={editingProduct}
        onSubmit={handleSave}
        onCancel={() => setEditingProduct(null)}
      />

      <DataTable
        columns={columns}
        data={paginated}
        actions={(row) => (
          <div className="space-x-2">
            <button
              onClick={() => setEditingProduct(row)}
              className="px-2 py-1 bg-blue-500 text-white rounded"
            >
              Edit
            </button>

            <button
              onClick={() => handleDelete(row.id)}
              className="px-2 py-1 bg-red-500 text-white rounded"
            >
              Delete
            </button>
          </div>
        )}
      />
      <Pagination
        currentPage={currentPage}
        totalPages={Math.ceil((data?.length || 0) / pageSize)}
        onPageChange={setCurrentPage}
      />
    </div>
  );
}
