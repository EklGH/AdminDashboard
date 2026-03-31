// Import du client HTTP
import { apiClient } from "./apiClient";
// Imports des types et DTOs
import type {
  Reservation,
  ReservationCreateDto,
  ReservationUpdateDto,
  PaginatedReservations,
} from "../../types/index";

// ======== Reservations API
export const reservationsApi = {
  // getPaginated (Récupère les réservations paginées)
  getPaginated: async (
    page: number,
    pageSize: number,
  ): Promise<PaginatedReservations> => {
    const res = await apiClient.get<PaginatedReservations>("/Reservations", {
      params: { page, pageSize },
    });

    return res.data;
  },

  // getById (Récupère une réservation par ID)
  getById: async (id: string): Promise<Reservation> => {
    const res = await apiClient.get<Reservation>(`/Reservations/${id}`);
    return res.data;
  },

  // create
  create: async (dto: ReservationCreateDto): Promise<Reservation> => {
    const res = await apiClient.post<Reservation>("/Reservations", dto);
    return res.data;
  },

  // update
  update: async (
    id: string,
    dto: ReservationUpdateDto,
  ): Promise<Reservation> => {
    const res = await apiClient.put<Reservation>(`/Reservations/${id}`, dto);
    return res.data;
  },

  // delete
  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/Reservations/${id}`);
  },
};
