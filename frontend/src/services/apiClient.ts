// ======== Faux client HTTP (mock)
export async function fetcher<T>(url: string, delay = 500): Promise<T> {
  // Temps artificiel de réponse pour simuler un backend (0.5sec)
  await new Promise((res) => setTimeout(res, delay));

  // Si aucun mock défini pour l'url
  throw new Error("Aucun mock défini pour cet endpoint : " + url);
}
