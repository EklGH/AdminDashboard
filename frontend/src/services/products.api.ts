// Import du type Product
import type { Product } from "../types";
// Import des données mockées
import { productsMock } from "./mock/products.mock";

// DB locale produits simulée
let productsDB = [...productsMock];

// ======== CRUD Faux backend Products (mock)
// ======== Récupère tous les produits (GET)
export async function getProducts(): Promise<Product[]> {
  // Simule un délai réseau (0,5sec)
  await new Promise((res) => setTimeout(res, 500));
  return productsDB;
}

// ======== Ajoute un nouveau produit (CREATE)
export async function createProduct(product: Product): Promise<Product> {
  // Simule un délai réseau (0,5sec)
  await new Promise((res) => setTimeout(res, 500));

  // Création du produit avec nouvel ID
  const newProduct = {
    ...product,
    id: Math.max(...productsDB.map((p) => p.id)) + 1,
  };
  // Ajout dans la DB locale
  productsDB.push(newProduct);
  return newProduct;
}

// ======== Update un produit existant (UPDATE)
export async function updateProduct(product: Product): Promise<Product> {
  // Simule un délai réseau (0,5sec)
  await new Promise((res) => setTimeout(res, 500));
  // Update le produit dans la DB locale
  productsDB = productsDB.map((p) => (p.id === product.id ? product : p));
  return product;
}

// ======== Supprime un produit par ID (DELETE)
export async function deleteProduct(id: number): Promise<void> {
  // Simule un délai réseau (0,5sec)
  await new Promise((res) => setTimeout(res, 500));
  // Filtre la DB locale pour supprimer le produit
  productsDB = productsDB.filter((p) => p.id !== id);
}
