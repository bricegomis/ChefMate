import { ModelBase } from './ModelBase';

export interface PriceItem extends ModelBase {
  price: number;
  dateBuying: string;
  storeName: string;
}
