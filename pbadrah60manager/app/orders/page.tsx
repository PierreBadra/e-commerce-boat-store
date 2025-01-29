"use client";

import { useState, useEffect, useMemo } from "react";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Order } from "@/types/order";
import {
  ColumnDef,
  ColumnFiltersState,
  FilterFn,
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable,
  VisibilityState,
} from "@tanstack/react-table";
import { rankItem } from "@tanstack/match-sorter-utils";
import { Skeleton } from "@/components/ui/skeleton";
import Image from "next/image";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { ArrowUpDown, ChevronDown } from "lucide-react";
import { Input } from "@/components/ui/input";

export default function OrderTable() {
  const SkeletonRow = () => (
    <TableRow>
      {columns.map((column, index) => (
        <TableCell key={index} className="p-4">
          <Skeleton className="h-6 w-full rounded-xl" />
        </TableCell>
      ))}
    </TableRow>
  );

  const columns: ColumnDef<Order>[] = [
    {
      accessorKey: "customerEmail",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            From
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => (
        <div className="flex items-center space-x-3 truncate">
          {row.original.customerEmail}
        </div>
      ),
    },
    
    {
      accessorKey: "dateCreated",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Date Created
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const date = new Date(row.original.dateCreated);
        const formattedDate = new Intl.DateTimeFormat("en-US", {
          year: "numeric",
          month: "long",
          day: "numeric",
        }).format(date);

        return (
          <div className="flex items-center space-x-3 truncate">
            {formattedDate}
          </div>
        );
      },
    },
    {
      accessorKey: "dateFulfilled",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Date Fulfilled
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const dateFulfilled = row.original.dateFulfilled;
        const formattedDate = dateFulfilled
          ? new Intl.DateTimeFormat("en-US", {
              year: "numeric",
              month: "long",
              day: "numeric",
            }).format(new Date(dateFulfilled))
          : "No fulfilled date";

        return (
          <div className={`flex ${formattedDate == "No fulfilled date"? "text-muted-foreground" : ""} items-center space-x-3 truncate`}>
            {formattedDate}
          </div>
        );
      },
    },
    {
      accessorKey: "total",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent flex w-full justify-end"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Total
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const amount = parseFloat(row.getValue("total"));
        const formatted = new Intl.NumberFormat("en-US", {
          style: "currency",
          currency: "USD",
        }).format(amount);

        return (
          <div className="flex items-center justify-end gap-2">
            <div className="text-right font-medium">{formatted}</div>
          </div>
        );
      },
    },
    {
      accessorKey: "taxes",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent flex w-full justify-end"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Taxes
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const amount = parseFloat(row.getValue("taxes"));
        const formatted = new Intl.NumberFormat("en-US", {
          style: "currency",
          currency: "USD",
        }).format(amount);

        return (
          <div className="flex items-center justify-end gap-2">
            <div className="text-right font-medium">{formatted}</div>
          </div>
        );
      },
    },
  ];
  const [orders, setOrders] = useState<Order[]>([]);
  const [sorting, setSorting] = useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([]);
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({});
  const [rowSelection, setRowSelection] = useState({});
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchProducts() {
      setLoading(true);
      const response = await fetch("https://api-boatbud.pierrebadra.me/api/Order");
      const data = await response.json();
      setOrders(data["$values"]);
      setLoading(false);
    }
    fetchProducts();
  }, []);

  const fuzzyFilter: FilterFn<Order> = (row, columnId, value, addMeta) => {
    // Convert search value to lowercase for case-insensitive comparison
    const searchValue = value.toLowerCase();
  
    // Create formatted dates matching the table display logic
    const dateCreatedFormatted = new Intl.DateTimeFormat("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    }).format(new Date(row.original.dateCreated));
  
    const dateFulfilledFormatted = row.original.dateFulfilled
      ? new Intl.DateTimeFormat("en-US", {
          year: "numeric",
          month: "short",
          day: "numeric",
        }).format(new Date(row.original.dateFulfilled))
      : "No fulfilled date";
  
    // Create an object with the columns you want to search
    const searchableColumns = {
      customerEmail: row.original.customerEmail.toLowerCase(),
      dateCreated: dateCreatedFormatted.toLowerCase(),
      dateFulfilled: dateFulfilledFormatted.toLowerCase()
    };
  
    // Check if the search value is in any of the specified columns
    const matchFound = Object.values(searchableColumns).some(columnValue => 
      columnValue.includes(searchValue)
    );
  
    const itemRank = matchFound ? { passed: true, compareValue: value } : { passed: false, compareValue: value };
    addMeta({ itemRank });
    return itemRank.passed;
  };

  const [globalFilter, setGlobalFilter] = useState("");

  const table = useReactTable({
    data: orders,
    columns,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    onColumnVisibilityChange: setColumnVisibility,
    onRowSelectionChange: setRowSelection,
    globalFilterFn: fuzzyFilter,
    state: {
      globalFilter,
      sorting,
      columnFilters,
      columnVisibility,
      rowSelection,
    },
    onGlobalFilterChange: setGlobalFilter,
  });

  const filteredTotals = useMemo(() => {
    const filteredRows = table.getFilteredRowModel().rows;
    
    return {
      totalAmount: filteredRows.reduce((sum, row) => 
        sum + row.original.total, 0),
      totalTaxes: filteredRows.reduce((sum, row) => 
        sum + row.original.taxes, 0)
    };
  }, [table.getFilteredRowModel().rows]);

  const handleSearch = (value: string) => {
    setGlobalFilter(value);
    table.setGlobalFilter(value);
  };

  return (
    <div className="w-full p-4 pt-0">
      <h2 className="text-3xl font-bold tracking-tight">Orders</h2>
      <div className="flex items-center py-4">
        <Input
          placeholder="Search by date created, date fulfilled or customer email..."
          value={globalFilter}
          onChange={(event) => handleSearch(event.target.value)}
          className="rounded-xl w-[500px]"
        />
        <DropdownMenu>
          <DropdownMenuTrigger className="rounded-xl" asChild>
            <Button variant="outline" className="ml-auto">
              Columns <ChevronDown className="ml-2 h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent className="rounded-xl" align="end">
            {table
              .getAllColumns()
              .filter((column) => column.getCanHide())
              .map((column) => {
                return (
                  <DropdownMenuCheckboxItem
                    key={column.id}
                    className="capitalize"
                    checked={column.getIsVisible()}
                    onCheckedChange={(value) =>
                      column.toggleVisibility(!!value)
                    }
                  >
                    {column.id}
                  </DropdownMenuCheckboxItem>
                );
              })}
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
      <div className="rounded-xl bg-muted/50 p-6">
        <Table className="overflow-hidden rounded-xl">
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id} className="truncate">
                {headerGroup.headers.map((header) => (
                  <TableHead key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                  </TableHead>
                ))}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {loading ? (
              Array.from({length: 10}).map((_, index) => (
                <SkeletonRow key={index} />
              ))
            ) : table.getRowModel().rows?.length ? (
              <>
                {table.getRowModel().rows.map((row) => (
                  <TableRow
                    key={row.id}
                    data-state={row.getIsSelected() && "selected"}
                  >
                    {row.getVisibleCells().map((cell) => (
                      <TableCell className="py-4" key={cell.id}>
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </TableCell>
                    ))}
                  </TableRow>
                ))}
                {!loading && (
                  <TableRow className="font-bold bg-muted">
                    <TableCell className="py-4" colSpan={3}>
                      {globalFilter ? "Filtered Total" : "Total"}
                    </TableCell>
                    <TableCell className="py-4 text-right">
                      {new Intl.NumberFormat("en-US", {
                        style: "currency",
                        currency: "USD",
                      }).format(filteredTotals.totalAmount)}
                    </TableCell>
                    <TableCell className="py-4 text-right">
                      {new Intl.NumberFormat("en-US", {
                        style: "currency",
                        currency: "USD",
                      }).format(filteredTotals.totalTaxes)}
                    </TableCell>
                  </TableRow>
                )}
              </>
            ) : (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="py-4 text-center"
                >
                  No results.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
