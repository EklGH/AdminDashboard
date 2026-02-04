// ======== Interface colonne générique
export interface Column<T> {
  key: keyof T;
  label: string;
}

// ======== Interface DataTable
interface DataTableProps<T> {
  columns: Column<T>[];
  data: T[];
  actions?: (row: T) => React.ReactNode;
}

// ======== Composant générique DataTable
export default function DataTable<T>({
  columns,
  data,
  actions,
}: DataTableProps<T>) {
  return (
    <table className="min-w-full bg-white border rounded overflow-hidden">
      <thead className="bg-gray-100">
        <tr>
          {columns.map((col) => (
            <th
              key={col.key as string}
              className="text-left px-4 py-2 border-b"
            >
              {col.label}
            </th>
          ))}
          {actions && <th className="px-4 py-2 border-b">Actions</th>}
        </tr>
      </thead>

      <tbody>
        {data.map((row, idx) => (
          <tr key={idx} className="hover:bg-gray-50">
            {columns.map((col) => (
              <td key={col.key as string} className="px-4 py-2 border-b">
                {String(row[col.key])}
              </td>
            ))}
            {actions && <td className="px-4 py-2 border-b">{actions(row)}</td>}
          </tr>
        ))}
      </tbody>
    </table>
  );
}
