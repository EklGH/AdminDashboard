// Imports Apollo Client
import {
  ApolloClient,
  InMemoryCache,
  HttpLink,
  ApolloLink,
} from "@apollo/client";
import { SetContextLink } from "@apollo/client/link/context";

// ======== Apollo Client instance
const GRAPHQL_URL =
  import.meta.env.VITE_GRAPHQL_URL || "http://localhost:5000/graphql";

// ======== Interceptor pour ajouter le token si présent
const authLink = new SetContextLink((prevContext) => {
  const token = localStorage.getItem("authToken");
  return {
    ...prevContext,
    headers: {
      ...prevContext.headers,
      Authorization: token ? `Bearer ${token}` : "",
    },
  };
});

// ======== GraphQL Client
export const graphqlClient = new ApolloClient({
  link: ApolloLink.from([authLink, new HttpLink({ uri: GRAPHQL_URL })]),
  cache: new InMemoryCache({
    typePolicies: {
      Query: {
        fields: {
          paginatedProducts: { keyArgs: ["input"] },
        },
      },
    },
  }),
});
