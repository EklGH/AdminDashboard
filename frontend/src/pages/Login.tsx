// Import du hook useState de React
import { useState } from "react";
// Import du hook du Context
import { useAuth } from "../context/useAuth";
// Import du hook useNavigate de React pour redirections
import { useNavigate } from "react-router-dom";
// Import du type Axios pour gérer les erreurs Axios
import type { AxiosError } from "axios";
// Import du Loader
import Loader from "../components/ui/Loader";

// ======== PAGE LOGIN ========
export default function Login() {
  // Récupération des fonctions login depuis le hook du Context
  const { login } = useAuth();

  // Hook pour la navigation entre pages
  const navigate = useNavigate();

  // States pour email, mdp, erreurs et chargement
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [status, setStatus] = useState<{
    type: "success" | "error";
    message: string;
  } | null>(null);

  // Gestion de la soumission du formulaire
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setStatus(null);
    setLoading(true);

    try {
      // Appel du login via AuthContext
      await login({ email, password });
      setStatus({
        type: "success",
        message: "Connexion réussie !",
      });

      setTimeout(() => navigate("/dashboard"), 500);
    } catch (err: unknown) {
      let message = "Identifiants incorrects";

      if (err && typeof err === "object" && "response" in err) {
        const axiosErr = err as AxiosError<{ message: string }>;
        message = axiosErr.response?.data?.message || message;
      }
      setStatus({
        type: "error",
        message,
      });
    } finally {
      setLoading(false);
    }
  };

  // ======== Rendu JSX de la page
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="w-full max-w-md bg-white rounded-xl shadow-xl p-6">
        <h2 className="text-xl font-bold mb-4 text-center">Connexion Admin</h2>

        {status && (
          <div
            className={`mb-4 px-4 py-2 rounded ${
              status.type === "success"
                ? "bg-green-100 text-green-800"
                : "bg-red-100 text-red-800"
            }`}
          >
            {status.message}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="email"
            placeholder="Adresse email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full border px-3 py-2 rounded"
            required
          />

          <input
            type="password"
            placeholder="Mot de passe"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full border px-3 py-2 rounded"
            required
          />

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-600 text-white py-2 rounded shadow hover:bg-blue-700 disabled:opacity-50"
          >
            {loading ? "Connexion..." : "Connexion"}
          </button>
        </form>

        {loading && (
          <div className="mt-3">
            <Loader message="Connexion en cours..." />
          </div>
        )}
      </div>
    </div>
  );
}
