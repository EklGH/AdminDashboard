// Import du type Reservation
import type { Reservation } from "../../types";

// ======== Fausse DB réservations (mock)
export const reservationsMock: Reservation[] = [
  { id: 1, customer: "Alice Johnson", date: "2026-01-15", status: "Confirmed" },
  { id: 2, customer: "Robert White", date: "2026-01-16", status: "Pending" },
  { id: 3, customer: "Emma Clark", date: "2026-01-17", status: "Cancelled" },
];
