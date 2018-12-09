import { TestBed } from '@angular/core/testing';

import { SingleFsoResolverService } from './single-fso-resolver.service';

describe('SingleFsoResolverService', () => {
	beforeEach(() => TestBed.configureTestingModule({}));

	it('should be created', () => {
		const service: SingleFsoResolverService = TestBed.get(SingleFsoResolverService);
		expect(service).toBeTruthy();
	});
});
