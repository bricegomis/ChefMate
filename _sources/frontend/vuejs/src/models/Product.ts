import { ModelBase } from './ModelBase';
import { PriceItem } from './PriceItem';

export interface Product extends ModelBase {
  name: string;
  image: string | null;
  labels: string[] | null;
  type: string | null;
  comments: string | null;
  unit: string | null;
  prices: PriceItem[] | null;
  tags: string[] | null;
  profileId: string | null;
}
