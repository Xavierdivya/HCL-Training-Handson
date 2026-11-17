
export interface FoodItem {
  foodItemId: number;
  foodName: string;
  description?: string;
  price: number;
  imageUrl?: string;
  isAvailable: boolean;
  categoryId: number;
}

export interface CreateFoodItem {
  foodName: string;
  description?: string;
  price: number;
  imageUrl?: string;
  isAvailable: boolean;
  categoryId: number;
}
