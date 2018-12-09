import { OrderedResult } from './ordered-result';
import { MediaTypeType } from '../../enums/media-type-type.enum';

export interface FilteredOrderedResult extends OrderedResult {
	mediaType: MediaTypeType;
}
