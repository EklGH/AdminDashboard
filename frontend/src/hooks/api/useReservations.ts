// Imports des hooks ReactQuery (gestion des données et du cache)
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
// Import du service reservations
import { reservationsApi } from "../../services/api/reservations.api";
// Imports des types et DTOs
import type {
  Reservation,
  ReservationCreateDto,
  ReservationUpdateDto,
} from "../../types/index";

// ======== Query Keys (pour cache ReactQuery)
export const reservationsKeys = {
  all: ["reservations"] as const,
  lists: () => [...reservationsKeys.all, "list"] as const,
  list: (page: number, pageSize: number) =>
    [...reservationsKeys.lists(), { page, pageSize }] as const,
  detail: (id: string) => [...reservationsKeys.all, "detail", id] as const,
};

// ======== getPaginated
export const usePaginatedReservations = (page: number, pageSize: number) => {
  return useQuery<
    {
      items: Reservation[];
      page: number;
      pageSize: number;
      totalItems: number;
    },
    Error
  >({
    queryKey: reservationsKeys.list(page, pageSize),
    queryFn: () => reservationsApi.getPaginated(page, pageSize),
    placeholderData: (prev) => prev,
  });
};

// ======== getById
export const useReservation = (id: string) => {
  return useQuery<Reservation>({
    queryKey: reservationsKeys.detail(id),
    queryFn: () => reservationsApi.getById(id),
    enabled: !!id,
  });
};

// ======== create
export const useCreateReservation = () => {
  const queryClient = useQueryClient();

  return useMutation<Reservation, unknown, ReservationCreateDto>({
    mutationFn: (dto) => reservationsApi.create(dto),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: reservationsKeys.all,
      });
    },
  });
};

// ======== update
export const useUpdateReservation = () => {
  const queryClient = useQueryClient();

  return useMutation<
    Reservation,
    unknown,
    { id: string; dto: ReservationUpdateDto }
  >({
    mutationFn: ({ id, dto }) => reservationsApi.update(id, dto),

    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: reservationsKeys.all,
      });

      queryClient.invalidateQueries({
        queryKey: reservationsKeys.detail(variables.id),
      });
    },
  });
};

// ======== delete
export const useDeleteReservation = () => {
  const queryClient = useQueryClient();

  return useMutation<void, unknown, string>({
    mutationFn: (id) => reservationsApi.delete(id),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: reservationsKeys.all,
      });
    },
  });
};
