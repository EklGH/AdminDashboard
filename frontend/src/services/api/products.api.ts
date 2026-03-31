// Import du client HTTP
import { apiClient } from "./apiClient";
// Imports des types et DTOs
import type {
  Product,
  ProductCreateDto,
  ProductUpdateDto,
} from "../../types/index";

// ======== Products API
export const productsApi = {
  // getById
  getById: async (id: string): Promise<Product> => {
    const res = await apiClient.get<Product>(`/Products/${id}`);
    return res.data;
  },

  // create
  create: async (dto: ProductCreateDto): Promise<Product> => {
    const res = await apiClient.post<Product>("/Products", dto);
    return res.data;
  },

  // update
  update: async (id: string, dto: ProductUpdateDto): Promise<Product> => {
    const res = await apiClient.put<Product>(`/Products/${id}`, dto);
    return res.data;
  },

  // delete
  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/Products/${id}`);
  },
};
