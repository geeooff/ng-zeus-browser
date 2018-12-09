import { MediaTypeType } from '../enums/media-type-type.enum';

export interface MediaType {
	type: MediaTypeType;
	mimeType: string;
	playerMimeType: string;
}
