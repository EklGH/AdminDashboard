// Import des modules nécessaires de React Router
import { Routes, Route, Navigate } from "react-router-dom";
// Import du Layout global et des Pages
import Layout from "../components/layout/Layout";
import Dashboard from "../pages/Dashboard";
import Products from "../pages/Products";
import Reservations from "../pages/Reservations";
import Login from "../pages/Login";
import ProtectedRoute from "./ProtectedRoute";

// ======== Routes principales de l'application
export default function AppRoutes() {
  return (
    // Englobe toutes les routes de l'application
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route
        element={
          <ProtectedRoute>
            <Layout />
          </ProtectedRoute>
        }
      >
        <Route path="/" element={<Navigate to="/dashboard" />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/products" element={<Products />} />
        <Route path="/reservations" element={<Reservations />} />
      </Route>
    </Routes>
  );
}
