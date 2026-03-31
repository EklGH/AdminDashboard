// Imports des hooks ReactQuery (gestion des données et du cache)
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
// Import du service products
import { productsApi } from "../../services/api/products.api";
// Imports des types et DTOs
import type { Product, ProductCreateDto, ProductUpdateDto } from "../../types";

// ======== Query Keys (pour cache ReactQuery)
export const productsKeys = {
  all: ["products"] as const,
  lists: () => [...productsKeys.all, "list"] as const,
  list: (page: number, pageSize: number) =>
    [...productsKeys.lists(), { page, pageSize }] as const,
  detail: (id: string) => [...productsKeys.all, "detail", id] as const,
};

// ======== getById
export const useProduct = (id: string) => {
  return useQuery<Product>({
    queryKey: productsKeys.detail(id),
    queryFn: () => productsApi.getById(id),
    enabled: !!id,
  });
};

// ======== create
export const useCreateProduct = () => {
  const queryClient = useQueryClient();

  return useMutation<Product, unknown, ProductCreateDto>({
    mutationFn: (dto: ProductCreateDto) => productsApi.create(dto),

    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: productsKeys.all });
    },
  });
};

// ======== update
export const useUpdateProduct = () => {
  const queryClient = useQueryClient();

  return useMutation<Product, unknown, { id: string; dto: ProductUpdateDto }>({
    mutationFn: ({ id, dto }) => productsApi.update(id, dto),

    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({ queryKey: productsKeys.all });
      queryClient.invalidateQueries({
        queryKey: productsKeys.detail(variables.id),
      });
    },
  });
};

// ======== delete
export const useDeleteProduct = () => {
  const queryClient = useQueryClient();

  return useMutation<void, unknown, string>({
    mutationFn: (id: string) => productsApi.delete(id),

    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: productsKeys.all });
    },
  });
};
