"use client";

import { useState, useCallback } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "./ui/form";
import { SquareArrowOutUpRight } from "lucide-react";

const formSchema = z.object({
    stock: z.string({
      required_error: "Stock value is required",
    })
    .refine((value) => value.trim() !== '', {
      message: "Stock value is required",
    })
    .transform((value) => Number(value))
    .pipe(
      z.number({
        invalid_type_error: "Must be a valid number"
      })
      .int({
        message: "Must be an integer"
      })
      .min(Number.MIN_SAFE_INTEGER, {
        message: "Value is too small"
      })
    )
    .transform(value => value.toString())
  });

interface EditStockDialogProps {
  productId: number;
  currentStock: number;
  onStockUpdate: (productId: number, newStock: number) => void;
}

export function EditStockDialog({
  productId,
  currentStock,
  onStockUpdate,
}: EditStockDialogProps) {
  const [isOpen, setIsOpen] = useState(false);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      stock: currentStock.toString(),
    },
  });

  const handleOpenChange = useCallback(
    (open: boolean) => {
      setIsOpen(open);
      if (open) {
        form.reset({ stock: currentStock.toString() });
      }
    },
    [form, currentStock]
  );

  const onSubmit = useCallback(
    (values: z.infer<typeof formSchema>) => {
      onStockUpdate(productId, Number(values.stock));
      setIsOpen(false);
    },
    [onStockUpdate, productId]
  );

  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogTrigger asChild>
        <Button variant="ghost" size="icon">
          <SquareArrowOutUpRight className="text-primary" size={20} />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Edit Stock</DialogTitle>
          <DialogDescription>Enter the new stock value.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="stock"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Stock Value</FormLabel>
                  <FormControl>
                    <Input
                      type="text"
                      placeholder="Enter new stock value"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <div className="flex gap-2">
              <Button type="submit" variant="default">
                Save
              </Button>
              <Button
                type="button"
                variant="outline"
                onClick={() => handleOpenChange(false)}
              >
                Cancel
              </Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
