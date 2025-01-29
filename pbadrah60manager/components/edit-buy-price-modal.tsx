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
  sellPrice: z.string().nullable().transform(value => value ? value : null),
  buyPrice: z
    .string({
      required_error: "Buy price value is required",
    })
    .refine((value) => value.trim() !== "", {
      message: "Buy price value is required",
    })
    .transform((value) => Number(value))
    .pipe(
      z
        .number({
          invalid_type_error: "Must be a valid number",
        })
        .min(0, {
          message: "Value cannot be negative",
        })
    )
    .transform((value) => value.toString()),
}).refine((data) => {
  // Only perform this check if both sell price and buy price are numbers
  if (data.sellPrice !== null && data.sellPrice !== undefined) {
    return Number(data.buyPrice) <= Number(data.sellPrice);
  }
  return true;
}, {
  message: "Buy price must be less than or equal to sell price",
  path: ["buyPrice"], // This will attach the error to the buyPrice field
});

interface EditBuyPriceDialogProps {
  productId: number;
  buyPrice: number;
  sellPrice: number;
  onBuyPriceUpdate: (productId: number, buyPrice: number, sellPrice: number) => void;
}

export function EditBuyPriceDialog({
  productId,
  buyPrice,
  sellPrice,
  onBuyPriceUpdate
}: EditBuyPriceDialogProps) {
  const [isOpen, setIsOpen] = useState(false);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      buyPrice: buyPrice.toString(),
    },
  });

  const handleOpenChange = useCallback(
    (open: boolean) => {
      setIsOpen(open);
      if (open) {
        form.reset({ buyPrice: buyPrice.toString(), sellPrice: sellPrice.toString()});
      }
    },
    [form, buyPrice, sellPrice]
  );

  const onSubmit = useCallback(
    (values: z.infer<typeof formSchema>) => {
      onBuyPriceUpdate(productId, Number(values.buyPrice), Number(values.sellPrice));
      setIsOpen(false);
    },
    [onBuyPriceUpdate, productId]
  );

  return (
    <Dialog open={isOpen} onOpenChange={handleOpenChange}>
      <DialogTrigger asChild>
        <Button variant='ghost' size="icon">
          <SquareArrowOutUpRight className="text-primary" size={20} />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Edit Buy Price</DialogTitle>
          <DialogDescription>Enter the new buy price value.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="buyPrice"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Buy Price Value</FormLabel>
                  <FormControl>
                    <Input
                      type="text"
                      placeholder="Enter new buy price value"
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
