"use client";

import * as React from "react";
import {
  ColumnDef,
  ColumnFiltersState,
  FilterFn,
  SortingState,
  VisibilityState,
  flexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { ArrowUpDown, ChevronDown, SquareArrowOutUpRight } from "lucide-react";
import { rankItem } from "@tanstack/match-sorter-utils";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuCheckboxItem,
  DropdownMenuContent,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Product, ProductPrice, ProductStock } from "@/types/product";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import Image from "next/image";
import { EditStockDialog } from "@/components/edit-stock-modal";
import { useToast } from "@/hooks/use-toast";
import { EditBuyPriceDialog } from "@/components/edit-buy-price-modal";
import { EditSellPriceDialog } from "@/components/edit-sell-price-modal";

export default function ProductTable() {
  const SkeletonRow = () => (
    <TableRow>
      {columns.map((column, index) => (
        <TableCell key={index} className="p-4">
          <Skeleton className="h-6 w-full rounded-xl" />
        </TableCell>
      ))}
    </TableRow>
  );

  const columns: ColumnDef<Product>[] = [
    {
      accessorKey: "description",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Description
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => (
        <div className="flex items-center space-x-3 truncate">
          <Image
            alt={row.original.imageName || "Product Image"}
            width={50}
            height={50}
            src={`data:image/jpeg;base64,${row.original.imageFile}`}
            className="object-cover rounded-full w-10 h-10"
          />
          <div className="flex flex-col items-start justify-between truncate">
            <p className="">{row.original.description}</p>
            <p className="text-xs text-muted-foreground">
              {row.original.manufacturer}
            </p>
          </div>
        </div>
      ),
    },
    {
      accessorKey: "categoryName",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Category
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => (
        <Badge className="rounded-xl truncate">
          {row.getValue("categoryName")}
        </Badge>
      ),
    },
    {
      accessorKey: "stock",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent flex w-full justify-end"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            Stock
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const productId = row.original.productId; // Assuming you have an id field
        const currentStock = row.original.stock;

        return (
          <div className="flex items-center justify-end gap-2">
            <div className="text-right">{currentStock}</div>
            <EditStockDialog
              productId={productId}
              currentStock={currentStock as number}
              onStockUpdate={handleStockUpdate}
            />
          </div>
        );
      },
    },
    {
      accessorKey: "buyPrice",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent flex w-full justify-end"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            <p>Buy Price</p>
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const amount = parseFloat(row.getValue("buyPrice"));
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
      accessorKey: "sellPrice",
      header: ({ column }) => {
        return (
          <Button
            className="p-0 hover:bg-transparent flex w-full justify-end"
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
          >
            <p>Sell Price</p>
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        );
      },
      cell: ({ row }) => {
        const amount = parseFloat(row.getValue("sellPrice"));
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
  const { toast } = useToast();
  const [products, setProducts] = React.useState<Product[]>([]);
  const [sorting, setSorting] = React.useState<SortingState>([]);
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>(
    []
  );
  const [columnVisibility, setColumnVisibility] =
    React.useState<VisibilityState>({});
  const [rowSelection, setRowSelection] = React.useState({});
  const [loading, setLoading] = React.useState(true);

  React.useEffect(() => {
    async function fetchProducts() {
      setLoading(true);
      const response = await fetch("http://localhost:5202/api/ManagerProducts");
      const data = await response.json();
      setProducts(data["$values"]);
      setLoading(false);
    }
    fetchProducts();
  }, []);

  const handleStockUpdate = React.useCallback(
    async (productId: number, adjustment: number) => {
      const product: ProductStock = {
        productId: productId,
        stock: adjustment,
      };

      try {
        const response = await fetch(
          `http://localhost:5202/api/Products/ProductStock/${productId}`,
          {
            headers: {
              "Content-Type": "application/json",
            },
            method: "PATCH",
            body: JSON.stringify(product),
          }
        );

        if (!response.ok) {
          throw new Error(response.statusText);
        }

        // Update the local state
        setProducts((prevProducts) =>
          prevProducts.map((p) =>
            p.productId === productId
              ? { ...p, stock: p.stock + adjustment }
              : p
          )
        );

        toast({
          variant: "default",
          title: "Stock Updated",
          description: `The stock has been successfully updated!`,
        });
      } catch (error) {
        toast({
          variant: "destructive",
          title: "Stock Update Failed",
          description: "Stock adjustment cannot be negative",
        });
      }
    },
    [toast]
  );

  const handleBuyPriceUpdate = React.useCallback(
    async (productId: number, buyPrice: number, sellPrice: number) => {
      const product: ProductPrice = {
        productId: productId,
        buyPrice: buyPrice,
        sellPrice: sellPrice,
      };

      try {
        const response = await fetch(
          `http://localhost:5202/api/Products/ProductBuyPrice/${productId}`,
          {
            headers: {
              "Content-Type": "application/json",
            },
            method: "PATCH",
            body: JSON.stringify(product),
          }
        );

        if (!response.ok) {
          throw new Error(response.statusText);
        }

        // Update the local state
        setProducts((prevProducts) =>
          prevProducts.map((p) =>
            p.productId === productId ? { ...p, buyPrice: product.buyPrice } : p
          )
        );

        toast({
          variant: "default",
          title: "Buy Price Updated",
          description: `The buy price has been successfully updated!`,
        });
      } catch (error) {
        toast({
          variant: "destructive",
          title: "Buy Price Update Failed",
          description: "An error occured while updating the buy price.",
        });
      }
    },
    [toast]
  );

  const handleSellPriceUpdate = React.useCallback(
    async (productId: number, buyPrice: number, sellPrice: number) => {
      const product: ProductPrice = {
        productId: productId,
        buyPrice: buyPrice,
        sellPrice: sellPrice,
      };

      try {
        const response = await fetch(
          `http://localhost:5202/api/Products/ProductSellPrice/${productId}`,
          {
            headers: {
              "Content-Type": "application/json",
            },
            method: "PATCH",
            body: JSON.stringify(product),
          }
        );

        if (!response.ok) {
          throw new Error(response.statusText);
        }

        // Update the local state
        setProducts((prevProducts) =>
          prevProducts.map((p) =>
            p.productId === productId
              ? { ...p, sellPrice: product.sellPrice }
              : p
          )
        );

        toast({
          variant: "default",
          title: "Sell Price Updated",
          description: `The sell price has been successfully updated!`,
        });
      } catch (error) {
        toast({
          variant: "destructive",
          title: "Buy Price Update Failed",
          description: "An error occured while updating the sell price.",
        });
      }
    },
    [toast]
  );

  const fuzzyFilter: FilterFn<any> = (row, columnId, value, addMeta) => {
    const itemRank = rankItem(row.getValue(columnId), value);
    addMeta({ itemRank });
    return itemRank.passed;
  };

  const [globalFilter, setGlobalFilter] = React.useState("");

  const table = useReactTable({
    data: products,
    columns,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
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
  const handleSearch = (value: string) => {
    setGlobalFilter(value);
    table.setGlobalFilter(value);
  };

  return (
    <div className="w-full p-4 pt-0">
      <h2 className="text-3xl font-bold tracking-tight">Products</h2>
      <div className="flex items-center py-4">
        <Input
          placeholder="Search by description, category, stock, buy or sell price..."
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
              Array.from({ length: 10 }).map((_, index) => (
                <SkeletonRow key={index} />
              ))
            ) : table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  data-state={row.getIsSelected() && "selected"}
                >
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id}>
                      {cell.column.id === "stock" ? (
                        <div className="flex items-center justify-end gap-2">
                          <div className="text-right">
                            {cell.getValue() as number}
                          </div>
                          <EditStockDialog
                            productId={row.original.productId}
                            currentStock={cell.getValue() as number}
                            onStockUpdate={handleStockUpdate}
                          />
                        </div>
                      ) : cell.column.id === "buyPrice" ? (
                        <div className="flex items-center justify-end gap-2">
                          <div className="text-right">
                            {flexRender(
                              cell.column.columnDef.cell,
                              cell.getContext()
                            )}
                          </div>
                          <EditBuyPriceDialog
                            productId={row.original.productId}
                            buyPrice={cell.getValue() as number}
                            sellPrice={row.original.sellPrice}
                            onBuyPriceUpdate={handleBuyPriceUpdate}
                          />
                        </div>
                      ) : cell.column.id === "sellPrice" ? (
                        <div className="flex items-center justify-end gap-2">
                          <div className="text-right">
                            {flexRender(
                              cell.column.columnDef.cell,
                              cell.getContext()
                            )}
                          </div>
                          <EditSellPriceDialog
                            productId={row.original.productId}
                            buyPrice={row.original.buyPrice}
                            sellPrice={cell.getValue() as number}
                            onSellPriceUpdate={handleSellPriceUpdate}
                          />
                        </div>
                      ) : (
                        flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )
                      )}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="h-24 text-center"
                >
                  No results.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>
      <div className="flex items-center justify-end space-x-2 py-4">
        <div className="space-x-2">
          <Button
            className="rounded-xl py-2 px-4 bg-muted/50"
            variant="outline"
            onClick={() => table.previousPage()}
            disabled={!table.getCanPreviousPage()}
          >
            Previous
          </Button>
          <Button
            className="rounded-xl py-2 px-4 bg-muted/50"
            variant="outline"
            onClick={() => table.nextPage()}
            disabled={!table.getCanNextPage()}
          >
            Next
          </Button>
        </div>
      </div>
    </div>
  );
}
