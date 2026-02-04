// Import du hook useQuery de ReactQuery
import { useQuery } from "@tanstack/react-query";
// Import des données mockées des réservations
import { reservationsMock } from "../services/mock/reservations.mock";
// Import du type Reservation
import type { Reservation } from "../types";

// ======== Hook useReservations (CRUD : récupération seulement)
export function useReservations() {
  return useQuery<Reservation[]>({
    // Clé du cache
    queryKey: ["reservations"],
    // Fonction pour récupérer les réservations
    queryFn: async () => {
      // Simule un délai réseau (0,5sec)
      await new Promise((res) => setTimeout(res, 500));
      return reservationsMock;
    },
  });
}

// ======== Typage du retour du hook UseReservations
export type UseReservationsReturn = ReturnType<typeof useReservations>;
