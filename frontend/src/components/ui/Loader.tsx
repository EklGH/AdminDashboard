// ======== Composant Loader réutilisable
export default function Loader({
  message = "Chargement...",
}: {
  message?: string;
}) {
  // ======== Rendu JSX du Loader
  return (
    <div className="flex flex-col items-center justify-center py-10 text-gray-500">
      <div className="w-10 h-10 border-4 border-blue-500 border-t-transparent rounded-full animate-spin mb-2"></div>
      <span>{message}</span>
    </div>
  );
}
