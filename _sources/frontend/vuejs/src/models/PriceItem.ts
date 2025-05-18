import { ModelBase } from './ModelBase';
import { ProductQuantityUnit } from './ProductQuantityUnit';

export interface PriceItem extends ModelBase {
  price: number;
  quantity: number;
  unit: ProductQuantityUnit;
  dateBuying: string;
  storeId: string;
}
