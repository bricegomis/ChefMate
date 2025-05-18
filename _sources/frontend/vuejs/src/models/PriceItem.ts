import { ProductQuantityUnit } from './ProductQuantityUnit';

export interface PriceItem {
  price: number;
  quantity: number;
  unit: ProductQuantityUnit;
  dateBuying: string;
  storeId: string;
}
