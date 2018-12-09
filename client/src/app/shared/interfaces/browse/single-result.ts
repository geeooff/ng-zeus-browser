import { Fso } from '../fso';
import { FsoResult } from '../fso-result';

export interface SingleResult extends FsoResult {
	ancestors: Fso[];
}
