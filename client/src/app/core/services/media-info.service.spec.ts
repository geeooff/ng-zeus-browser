import { TestBed } from '@angular/core/testing';

import { MediaInfoService } from './media-info.service';

describe('MediaInfoService', () => {
	beforeEach(() => TestBed.configureTestingModule({}));

	it('should be created', () => {
		const service: MediaInfoService = TestBed.get(MediaInfoService);
		expect(service).toBeTruthy();
	});
});
