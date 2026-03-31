// Import du client GraphQL
import { gql } from "@apollo/client";
import { graphqlClient } from "./graphqlClient";
// Imports des types et DTOs
import type {
  Product,
  PaginatedProducts,
  ProductFilter,
} from "../../types/index";

// ======== Queries
// PaginatedProducts
const PAGINATED_PRODUCTS_QUERY = gql`
  query PaginatedProducts(
    $page: Int!
    $pageSize: Int!
    $category: String
    $orderBy: ProductOrderBy
  ) {
    paginatedProducts(
      input: {
        page: $page
        pageSize: $pageSize
        category: $category
        orderBy: $orderBy
      }
    ) {
      items {
        id
        name
        category
        price
        stock
      }
      page
      pageSize
      totalItems
    }
  }
`;

// Products
const PRODUCTS_QUERY = gql`
  query Products(
    $first: Int
    $after: String
    $where: ProductResponseDtoFilterInput
    $order: [ProductResponseDtoSortInput!]
  ) {
    products(first: $first, after: $after, where: $where, order: $order) {
      nodes {
        id
        name
        category
        price
        stock
      }
      pageInfo {
        hasNextPage
        hasPreviousPage
        startCursor
        endCursor
      }
    }
  }
`;

// ======== Products GraphQL
export const productsGraphqlApi = {
  // getPaginated (Récupère les produits paginés)
  getPaginated: async (
    page: number,
    pageSize: number,
    category?: string,
    orderBy?:
      | "NAME_ASC"
      | "NAME_DESC"
      | "PRICE_ASC"
      | "PRICE_DESC"
      | "STOCK_ASC"
      | "STOCK_DESC",
  ): Promise<PaginatedProducts> => {
    const { data } = await graphqlClient.query<{
      paginatedProducts: PaginatedProducts;
    }>({
      query: PAGINATED_PRODUCTS_QUERY,
      variables: { page, pageSize, category, orderBy },
      fetchPolicy: "network-only",
    });

    if (!data) {
      throw new Error("Erreur lors de la récupération des produits paginés");
    }

    return data.paginatedProducts;
  },

  // getall (Récupère tous les produits)
  getAll: async (
    first?: number,
    after?: string,
    where?: ProductFilter,
    order?: Array<{ [key: string]: "ASC" | "DESC" }>,
  ): Promise<Product[]> => {
    const { data } = await graphqlClient.query<{
      products: { nodes: Product[] };
    }>({
      query: PRODUCTS_QUERY,
      variables: { first, after, where, order },
      fetchPolicy: "network-only",
    });

    if (!data) {
      throw new Error("Erreur lors de la récupération des produits");
    }

    return data.products.nodes;
  },
};
