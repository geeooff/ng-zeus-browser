import { PathResult } from '../path-result';
import { GroupBy } from '../../enums/group-by.enum';
import { OrderBy } from '../../enums/order-by.enum';

export interface OrderedResult extends PathResult {
	groupBy: GroupBy;
	orderBy: OrderBy;
}
