import { PriceItem } from './PriceItem';
import { ProductUsageType } from './ProductUsageType';

export interface Product {
  id: string;
  dateCreated: string;
  dateModified: string;
  name: string;
  image: string | null;
  description: string | null;
  labels: string[] | null;
  unit: string | null;
  prices: PriceItem[] | null;
  tags: string[] | null;
  usages: ProductUsageType[] | null;
}
