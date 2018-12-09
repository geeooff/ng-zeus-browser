import { OrderedResult } from './ordered-result';
import { PathResult } from '../path-result';
import { Fso } from '../fso';

export interface SiblingsResult extends OrderedResult {
	siblings: PathResult[];
	previous: Fso;
	next: Fso;
}
