// Import React Hooks
import { useState } from "react";
// Import du type Product
import type { Product } from "../../types";

// Interface ProductForm
interface ProductFormProps {
  product?: Product | null;
  onSubmit: (product: Product) => void;
  onCancel: () => void;
}

// ======== Composant ProductForm
export default function ProductForm({
  product,
  onSubmit,
  onCancel,
}: ProductFormProps) {
  // Valeurs initiales des champs du formulaire
  const [form, setForm] = useState<Product>(
    () => product ?? { id: 0, name: "", category: "", price: 0, stock: 0 },
  );

  // Update un champ spécifique à chaque saisie
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    // Récupère le nom et la valeur du champ modifié
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: name === "price" || name === "stock" ? Number(value) : value,
    }));
  };

  // Envoi du formulaire
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(form);
  };

  // ======== Rendu JSX du formulaire
  return (
    <div className="bg-white p-6 rounded shadow-md mb-6">
      <h3 className="text-lg font-semibold mb-4">
        {product && product.id !== 0
          ? "Modifier le produit"
          : "Ajouter un produit"}
      </h3>

      <form
        onSubmit={handleSubmit}
        className="space-y-4"
        onKeyDown={(e) => {
          if (e.key === "Enter") e.preventDefault();
        }}
      >
        <input
          name="name"
          placeholder="Nom du produit"
          value={form.name}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded"
          required
        />

        <input
          name="category"
          placeholder="Catégorie"
          value={form.category}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded"
          required
        />

        <input
          name="price"
          type="number"
          placeholder="Prix"
          value={form.price === 0 ? "" : form.price}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded"
          required
        />

        <input
          name="stock"
          type="number"
          placeholder="Stock"
          value={form.stock === 0 ? "" : form.stock}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded"
          required
        />

        <div className="flex space-x-2">
          <button
            type="submit"
            className="bg-blue-600 text-white px-4 py-2 rounded shadow hover:bg-blue-700"
          >
            Enregistrer
          </button>
          <button
            type="button"
            onClick={onCancel}
            className="bg-gray-300 text-gray-800 px-4 py-2 rounded shadow hover:bg-gray-400"
          >
            Annuler
          </button>
        </div>
      </form>
    </div>
  );
}
