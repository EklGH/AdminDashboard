// Import React Hooks
import { useEffect, useState } from "react";
// Imports des types et DTOs
import type { Product, ProductCreateDto, ProductUpdateDto } from "../../types";

// Interface ProductForm
interface ProductFormProps {
  product?: Product | null;
  onSubmit: (dto: ProductCreateDto | ProductUpdateDto, id?: string) => void;
  onCancel: () => void;
  loading?: boolean;
}

// ======== Composant ProductForm
export default function ProductForm({
  product,
  onSubmit,
  onCancel,
  loading,
}: ProductFormProps) {
  const isEdit = !!product?.id;
  // Valeurs initiales des champs du formulaire
  const [form, setForm] = useState<ProductCreateDto>(() => ({
    name: product?.name ?? "",
    category: product?.category ?? "",
    price: product?.price ?? 0,
    stock: product?.stock ?? 0,
  }));

  const [touched, setTouched] = useState(false);

  const isValid =
    form.name.trim() &&
    form.category.trim() &&
    form.price !== undefined &&
    form.stock !== undefined;

  useEffect(() => {
    const input = document.getElementById("product-name");
    input?.focus();
  }, []);

  // Update un champ spécifique à chaque saisie
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]:
        name === "price" || name === "stock"
          ? value === ""
            ? undefined
            : Math.max(0, Number(value))
          : value,
    }));
    setTouched(true);
  };

  // Envoi du formulaire
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!isValid) return;

    if (isEdit && product?.id) {
      onSubmit(form as ProductUpdateDto, product.id);
    } else {
      onSubmit(form as ProductCreateDto);
    }
  };

  // ======== Rendu JSX du formulaire
  return (
    <div className="fixed inset-0 bg-black/30 flex items-center justify-center p-4">
      <div className="w-full max-w-lg bg-white rounded-xl shadow-xl p-6 animate-fadeIn">
        <h3 className="text-lg font-semibold mb-4">
          {isEdit ? "Modifier le produit" : "Ajouter un produit"}
        </h3>

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            id="product-name"
            name="name"
            placeholder="Nom"
            value={form.name}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          />

          <input
            name="category"
            placeholder="Catégorie"
            value={form.category}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          />

          <input
            name="price"
            type="number"
            placeholder="Prix"
            value={form.price ?? ""}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          />

          <input
            name="stock"
            type="number"
            placeholder="Stock"
            value={form.stock ?? ""}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          />

          {/* Actions */}
          <div className="flex flex-col sm:flex-row gap-2">
            <button
              type="submit"
              disabled={!isValid || loading}
              className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-50"
            >
              {loading ? "Sauvegarde..." : "Enregistrer"}
            </button>

            <button
              type="button"
              onClick={onCancel}
              className="bg-gray-200 px-4 py-2 rounded hover:bg-gray-300"
            >
              Annuler
            </button>
          </div>

          {/* Edge case feedback */}
          {touched && !isValid && (
            <p className="text-red-500 text-sm">
              Tous les champs sont obligatoires
            </p>
          )}
        </form>
      </div>
    </div>
  );
}
