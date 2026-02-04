// Import du type LoginResponse
import type { LoginResponse } from "../types";

// ======== Faux backend login (mock)
export async function loginApi(
  email: string,
  password: string,
): Promise<LoginResponse> {
  // Simule un délai réseau (0,7sec)
  await new Promise((res) => setTimeout(res, 700));

  // Vérifie que les champs sont remplis
  if (!email || !password) throw new Error("Identifiants incorrects");

  return {
    user: { email },
    token: "fake-jwt-token",
  };
}
