// Import React Hooks
import { useEffect, useState } from "react";
// Imports des types et DTOs
import type {
  Reservation,
  ReservationCreateDto,
  ReservationUpdateDto,
} from "../../types";

// Interface ReservationForm
interface ReservationFormProps {
  reservation?: Reservation | null;
  onSubmit: (
    dto: ReservationCreateDto | ReservationUpdateDto,
    id?: string,
  ) => void;
  onCancel: () => void;
  products: { id: string; name: string; stock: number }[];
  loadingProducts?: boolean;
  loading?: boolean;
}

// ======== Composant ReservationForm
export default function ReservationForm({
  reservation,
  onSubmit,
  onCancel,
  products,
  loadingProducts,
  loading,
}: ReservationFormProps) {
  const isEdit = !!reservation?.id;

  // Valeurs initiales des champs du formulaire
  const [form, setForm] = useState<ReservationCreateDto>(() => ({
    customer: reservation?.customer ?? "",
    date: reservation?.date ?? "",
    status: reservation?.status ?? "Pending",
    productId: reservation?.productId ?? "",
    userId: reservation?.userId ?? null,
  }));

  const [touched, setTouched] = useState(false);

  const isValid = form.customer.trim() && form.date && form.productId;

  useEffect(() => {
    const el = document.getElementById("customer");
    el?.focus();
  }, []);

  // Update un champ spécifique à chaque saisie
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
  ) => {
    const { name, value } = e.target;

    setForm((prev) => ({ ...prev, [name]: value }));
    setTouched(true);
  };

  // Envoi du formulaire
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!isValid) return;

    if (isEdit && reservation?.id) {
      onSubmit(form as ReservationUpdateDto, reservation.id);
    } else {
      onSubmit(form as ReservationCreateDto);
    }
  };

  // ======== Rendu JSX du formulaire
  return (
    <div className="fixed inset-0 bg-black/30 flex items-center justify-center p-4">
      <div className="w-full max-w-lg bg-white rounded-xl shadow-xl p-6 animate-fadeIn">
        <h3 className="text-lg font-semibold mb-4">
          {isEdit ? "Modifier la réservation" : "Ajouter une réservation"}
        </h3>

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* CUSTOMER */}
          <input
            id="customer"
            name="customer"
            placeholder="Client"
            value={form.customer}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          />

          {/* DATE */}
          <input
            type="date"
            name="date"
            value={form.date}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          />

          {/* PRODUCT */}
          <select
            name="productId"
            value={form.productId}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          >
            <option value="">
              {loadingProducts ? "Chargement..." : "Choisir un produit"}
            </option>

            {products.map((p) => (
              <option key={p.id} value={p.id} disabled={p.stock === 0}>
                {p.name} ({p.stock})
              </option>
            ))}
          </select>

          {/* STATUS */}
          <select
            name="status"
            value={form.status}
            onChange={handleChange}
            className="w-full border px-3 py-2 rounded"
          >
            <option value="Pending">Pending</option>
            <option value="Confirmed">Confirmed</option>
            <option value="Cancelled">Cancelled</option>
          </select>

          {/* ACTIONS */}
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

          {/* EDGE CASE */}
          {touched && !isValid && (
            <p className="text-red-500 text-sm">
              Tous les champs obligatoires doivent être remplis
            </p>
          )}
        </form>
      </div>
    </div>
  );
}
