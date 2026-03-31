// ======== Composant Loader réutilisable
export default function Loader({
  message = "Chargement...",
}: {
  message?: string;
}) {
  // ======== Rendu JSX du Loader
  return (
    <div className="flex items-center justify-center py-10">
      <div className="bg-white shadow-sm border rounded-xl px-6 py-4 flex flex-col items-center">
        <div className="w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full animate-spin mb-3" />
        <span className="text-sm text-gray-600">{message}</span>
      </div>
    </div>
  );
}
