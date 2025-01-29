export type Product = {
  productId: number;
  prodCatId: number;
  description: string;
  manufacturer: string;
  stock: number;
  buyPrice: number;
  sellPrice: number;
  imageName: string;
  imageFile: string;
  categoryName: string;
};

export type ProductStock = {
  productId: number;
  stock: number;
};

export type ProductPrice = {
  productId: number;
  sellPrice: number;
  buyPrice: number;
};