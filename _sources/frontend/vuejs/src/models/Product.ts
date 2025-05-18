import { PriceItem } from './PriceItem';
import { ProductUsageType } from './ProductUsageType';

export interface Product {
  id: string;
  name: string;
  description: string | null;
  image: string | null;
  labels: string[] | null;
  tags: string[] | null;
  prices: PriceItem[] | null;
  usages: ProductUsageType[] | null;
  dateCreated: string;
  dateModified: string;
}
