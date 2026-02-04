// Import du hook useState de React
import { useState } from "react";
// Import du hook useAuth pour gérer l’authentification
import { useAuth } from "../context/useAuth";
// Import du hook useNavigate pour redirections
import { useNavigate } from "react-router-dom";

// ======== PAGE LOGIN ========
export default function Login() {
  // Récupération des fonctions login et logout depuis le hook du Context
  const { login } = useAuth();

  // Pour naviguer vers d’autres routes
  const navigate = useNavigate();

  // Stocke l’email saisi par l’utilisateur
  const [email, setEmail] = useState("");

  // Stocke le mdp saisi
  const [password, setPassword] = useState("");

  // Affiche les erreurs de login
  const [error, setError] = useState("");

  // Envoie les identifiants et connecte l’utilisateur
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // Connexion vers la page Dashboard si ok
    try {
      await login(email, password);
      navigate("/dashboard");
    } catch {
      setError("Identifiants incorrects");
    }
  };

  // ======== Rendu JSX de la page
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-6 rounded shadow-md w-80 space-y-4"
      >
        <h2 className="text-lg font-bold text-center">Admin Login</h2>

        {error && <p className="text-red-500 text-sm">{error}</p>}

        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full border px-3 py-2 rounded"
        />

        <input
          type="password"
          placeholder="Mot de passe"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="w-full border px-3 py-2 rounded"
        />

        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded"
        >
          Login
        </button>
      </form>
    </div>
  );
}
