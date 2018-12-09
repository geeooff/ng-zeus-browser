import { MediaType } from './media-type';

export interface Fso {
	name: string;
	path: string;
	mediaType: MediaType;
	exists: boolean;
	isDir: boolean;
	isFile: boolean;
	fileSize: number;
	created: Date;
	modified: Date;
	fileExtension: string;
	instanceCreated: Date;
	instanceAged: any;
}
