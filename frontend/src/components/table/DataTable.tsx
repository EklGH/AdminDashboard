// Import React
import React from "react";

// ======== Interface colonne générique
export interface Column<T> {
  key: keyof T;
  label: string;
  render?: (value: T[keyof T], row: T) => React.ReactNode;
  align?: "left" | "center" | "right";
}

// ======== Interface DataTable
interface DataTableProps<T> {
  columns: Column<T>[];
  data: T[];
  actions?: (row: T) => React.ReactNode;
  loading?: boolean;
  emptyMessage?: string;
}

// ======== Composant générique DataTable
export default function DataTable<T>({
  columns,
  data,
  actions,
  loading = false,
  emptyMessage = "Aucune donnée",
}: DataTableProps<T>) {
  // ======== Rendu JSX de la DataTable
  return (
    <div className="overflow-x-auto rounded-lg border shadow-sm bg-white">
      <table className="min-w-full text-sm">
        {/* HEADER */}
        <thead className="bg-gray-50 sticky top-0">
          <tr>
            {columns.map((col) => (
              <th
                key={col.key as string}
                className={`px-4 py-3 text-xs font-semibold text-gray-600 uppercase tracking-wide ${
                  col.align === "right"
                    ? "text-right"
                    : col.align === "center"
                      ? "text-center"
                      : "text-left"
                }`}
              >
                {col.label}
              </th>
            ))}

            {actions && (
              <th className="px-4 py-3 text-right text-xs font-semibold text-gray-600 uppercase">
                Actions
              </th>
            )}
          </tr>
        </thead>

        {/* BODY */}
        <tbody>
          {loading ? (
            <tr>
              <td
                colSpan={columns.length + (actions ? 1 : 0)}
                className="text-center py-10 text-gray-500"
              >
                Chargement...
              </td>
            </tr>
          ) : data.length === 0 ? (
            <tr>
              <td
                colSpan={columns.length + (actions ? 1 : 0)}
                className="text-center py-10 text-gray-500"
              >
                {emptyMessage}
              </td>
            </tr>
          ) : (
            data.map((row, idx) => (
              <tr key={idx} className="border-t hover:bg-gray-50 transition">
                {columns.map((col) => {
                  const value = row[col.key];

                  return (
                    <td
                      key={col.key as string}
                      className={`px-4 py-3 text-gray-700 ${
                        col.align === "right"
                          ? "text-right"
                          : col.align === "center"
                            ? "text-center"
                            : "text-left"
                      }`}
                    >
                      {col.render ? col.render(value, row) : String(value)}
                    </td>
                  );
                })}

                {actions && (
                  <td className="px-4 py-3 text-right">{actions(row)}</td>
                )}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
