// Imports axios
import axios from "axios";
import type { AxiosInstance } from "axios";
// Imports des types et DTOs
import type {
  Product,
  ProductCreateDto,
  ProductUpdateDto,
  Reservation,
  ReservationCreateDto,
  ReservationUpdateDto,
  User,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RefreshTokenRequest,
} from "../../types/index";

// ======== Axios instance
export const apiClient: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL || "http://localhost:5000/api",
  headers: {
    "Content-Type": "application/json",
  },
});

// ======== Interceptor pour ajouter le token si présent
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("authToken");
  if (token && config.headers) {
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
});

// ======== Auth API
export const authApi = {
  // login
  login: async (dto: LoginRequest): Promise<LoginResponse> => {
    const res = await apiClient.post<LoginResponse>("/Auth/login", dto);
    if (res.data.token) {
      localStorage.setItem("authToken", res.data.token);
    }
    return res.data;
  },

  // register
  register: async (dto: RegisterRequest): Promise<User> => {
    const res = await apiClient.post<User>("/Auth/register", dto);
    return res.data;
  },

  // refreshToken
  refreshToken: async (dto: RefreshTokenRequest): Promise<LoginResponse> => {
    const res = await apiClient.post<LoginResponse>("/Auth/refresh", dto);
    if (res.data.token) {
      localStorage.setItem("authToken", res.data.token);
    }
    return res.data;
  },

  // logout
  logout: (): void => {
    localStorage.removeItem("authToken");
  },
};

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

// ======== Reservations API
export const reservationsApi = {
  // getPaginated
  getPaginated: async (
    page: number,
    pageSize: number,
  ): Promise<{
    items: Reservation[];
    page: number;
    pageSize: number;
    totalItems: number;
  }> => {
    const res = await apiClient.get("/Reservations", {
      params: { page, pageSize },
    });
    return res.data;
  },

  // getById
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
