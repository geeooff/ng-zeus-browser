import { OrderedResult } from './ordered-result';
import { Fso } from '../fso';

export interface ChildrenResult extends OrderedResult {
	children: Fso[];
}
