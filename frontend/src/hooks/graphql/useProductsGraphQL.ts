// Imports des hooks ReactQuery
import { useQuery } from "@tanstack/react-query";
// Import du service products (graphql)
import { productsGraphqlApi } from "../../services/graphql/products.graphql";
// Imports des types et DTOs
import type { Product, PaginatedProducts, ProductFilter } from "../../types";

// ======== Query Keys (pour cache ReactQuery)
export const productsGraphqlKeys = {
  all: ["products-graphql"] as const,
  lists: () => [...productsGraphqlKeys.all, "list"] as const,
  list: (params: unknown) => [...productsGraphqlKeys.lists(), params] as const,
  paginated: (params: unknown) =>
    [...productsGraphqlKeys.all, "paginated", params] as const,
};

// ======== getPaginated
export const usePaginatedProductsGraphQL = (params: {
  page: number;
  pageSize: number;
  category?: string;
  orderBy?:
    | "NAME_ASC"
    | "NAME_DESC"
    | "PRICE_ASC"
    | "PRICE_DESC"
    | "STOCK_ASC"
    | "STOCK_DESC";
}) => {
  return useQuery<PaginatedProducts, Error>({
    queryKey: productsGraphqlKeys.paginated(params),
    queryFn: () =>
      productsGraphqlApi.getPaginated(
        params.page,
        params.pageSize,
        params.category,
        params.orderBy,
      ),
    placeholderData: (previousData) => previousData,
  });
};

// ======== getAll
export const useProductsGraphQL = (params?: {
  first?: number;
  after?: string;
  where?: ProductFilter;
  order?: Array<{ [key: string]: "ASC" | "DESC" }>;
}) => {
  return useQuery<Product[], Error>({
    queryKey: productsGraphqlKeys.list(params),
    queryFn: () =>
      productsGraphqlApi.getAll(
        params?.first,
        params?.after,
        params?.where,
        params?.order,
      ),
  });
};
