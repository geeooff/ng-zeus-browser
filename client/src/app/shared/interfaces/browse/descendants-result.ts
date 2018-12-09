import { FilteredOrderedResult } from './filtered-ordered-result';
import { Fso } from '../fso';

export interface DescendantsResult extends FilteredOrderedResult {
	descendants: Fso[];
}
