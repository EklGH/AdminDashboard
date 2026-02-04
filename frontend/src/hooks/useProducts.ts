// Import des hooks ReactQuery (gestion des données et du cache)
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
// Import des fonctions CRUD du (faux) backend produits
import {
  getProducts,
  createProduct,
  updateProduct,
  deleteProduct,
} from "../services/products.api";
// Import du type Product
import type { Product } from "../types";

// ======== Hook useProducts (CRUD)
export function useProducts() {
  // Pour l'invalidation du cache
  const queryClient = useQueryClient();

  // ======== Récupère la liste des produits
  const query = useQuery<Product[]>({
    // Clé du cache
    queryKey: ["products"],
    // Fonction pour récupérer les produits
    queryFn: getProducts,
  });

  // ======== Crée un produit
  const create = useMutation<Product, unknown, Product>({
    // Fonction pour créer un produit
    mutationFn: createProduct,
    // Invalidation du cache
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
  });

  // ======== Update un produit
  const update = useMutation<Product, unknown, Product>({
    // Fonction pour update un produit
    mutationFn: updateProduct,
    // Invalidation du cache
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
  });

  // ======== Supprime un produit
  const remove = useMutation<void, unknown, number>({
    // Fonction pour supprimer un produit
    mutationFn: deleteProduct,
    // Invalidation du cache
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
  });

  return {
    ...query,
    create,
    update,
    remove,
  };
}

// ======== Typage du retour du hook useProducts
export type UseProductsReturn = ReturnType<typeof useProducts>;
