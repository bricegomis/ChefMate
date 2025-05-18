import { ModelBase } from './ModelBase';
import { PriceItem } from './PriceItem';
import { ProductUsageType } from './ProductUsageType';

export interface Product extends ModelBase {
  name: string;
  image: string | null;
  description: string | null;
  labels: string[] | null;
  unit: string | null;
  prices: PriceItem[] | null;
  tags: string[] | null;
  usages: ProductUsageType[] | null;
}
