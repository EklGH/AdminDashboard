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
// Import du Loader
import Loader from "../components/ui/Loader";
// Import du hook useIsMutating de ReactQuery
import { useIsMutating } from "@tanstack/react-query";

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
  // Feedbacks utilisateur (success/error)
  const [statusMessage, setStatusMessage] = useState<string | null>(null);
  // Nombre d’éléments par page
  const pageSize = 3;

  // Valeur par défaut pour créer un nouveau produit
  const emptyProduct: ProductFromHook = {
    id: 0,
    name: "",
    category: "",
    price: 0,
    stock: 0,
  };

  // Colonnes de la DataTable
  const columns: Column<ProductFromHook>[] = [
    { key: "id", label: "ID" },
    { key: "name", label: "Nom" },
    { key: "category", label: "Catégorie" },
    { key: "price", label: "Prix" },
    { key: "stock", label: "Stock" },
  ];

  // Détecte si une modification est en cours
  const mutationCount = useIsMutating({ mutationKey: ["products"] });
  const isMutating = mutationCount > 0;

  // Feedbacks pour le chargement des pages
  if (isLoading) return <Loader message="Chargement des produits..." />;
  if (error)
    return (
      <p className="text-red-500">Erreur lors du chargement des produits</p>
    );

  // Produits à afficher sur la page courante
  const start = (currentPage - 1) * pageSize;
  const paginated = data?.slice(start, start + pageSize) || [];

  // ======== CRUD
  // Création et update d’un produit
  const handleSave = (product: ProductFromHook) => {
    if (product.id === 0) {
      // Create + réinitialise les champs
      create.mutate(product, {
        onSuccess: () => {
          setStatusMessage("Produit créé avec succès !");
          setEditingProduct(null);
        },
        onError: () => setStatusMessage("Echec de la création du produit"),
      });
    } else {
      // Update + réinitialise les champs
      update.mutate(product, {
        onSuccess: () => {
          setStatusMessage("Produit mis à jour avec succès !");
          setEditingProduct(null);
        },
        onError: () => setStatusMessage("Echec de la mise à jour du produit"),
      });
    }
  };

  // Suppression d’un produit + réinitialise les champs
  const handleDelete = (id: number) => {
    remove.mutate(id, {
      onSuccess: () => {
        setStatusMessage("Produit supprimé avec succès !");
        setEditingProduct(null);
      },
      onError: () => setStatusMessage("Echec de la suppression du produit"),
    });
  };

  // ======== Rendu JSX de la page
  return (
    <div>
      <h2 className="text-xl font-bold mb-4">Produits</h2>

      <button
        onClick={() => setEditingProduct(emptyProduct)}
        className="mb-4 bg-green-600 text-white px-4 py-2 rounded shadow hover:bg-green-700"
      >
        + Ajouter un produit
      </button>

      {statusMessage && (
        <div className="mb-4 px-4 py-2 bg-green-100 text-green-800 rounded">
          {statusMessage}
        </div>
      )}

      <ProductForm
        key={editingProduct?.id ?? "new"}
        product={editingProduct}
        onSubmit={handleSave}
        onCancel={() => setEditingProduct(null)}
      />

      {isMutating && <Loader message="Opération en cours..." />}

      <div className="overflow-x-auto">
        <DataTable
          columns={columns}
          data={paginated}
          actions={(row) => (
            <div className="space-x-2">
              <button
                onClick={() => setEditingProduct(row)}
                className="px-2 py-1 bg-blue-500 text-white rounded shadow hover:bg-blue-600"
              >
                Modifier
              </button>

              <button
                onClick={() => handleDelete(row.id)}
                className="px-2 py-1 bg-red-500 text-white rounded shadow hover:bg-red-600"
              >
                Supprimer
              </button>
            </div>
          )}
        />
      </div>
      <Pagination
        currentPage={currentPage}
        totalPages={Math.ceil((data?.length || 0) / pageSize)}
        onPageChange={setCurrentPage}
      />
    </div>
  );
}
