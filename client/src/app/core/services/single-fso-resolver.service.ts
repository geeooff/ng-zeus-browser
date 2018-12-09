import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';
import { FsoResult } from '../../shared/interfaces/fso-result';
import { BrowseService } from './browse.service';

@Injectable({
	providedIn: 'root'
})
export class SingleFsoResolverService implements Resolve<FsoResult> {
	constructor(
		private router: Router,
		private browseService: BrowseService
	) { }

	resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<FsoResult> {
		const path = route.paramMap.get('path');
		return this.browseService.GetSingle(path)
			.pipe(
				take(1),
				tap((data) => {
					console.log(data);
					return data;
				})
			);
	}
}
