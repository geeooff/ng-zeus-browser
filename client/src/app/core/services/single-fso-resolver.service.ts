import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';
import { SingleResult } from '../../shared/interfaces/browse/single-result';
import { BrowseService } from './browse.service';

@Injectable({
	providedIn: 'root'
})
export class SingleFsoResolverService implements Resolve<SingleResult> {
	constructor(
		private router: Router,
		private browseService: BrowseService
	) { }

	resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<SingleResult> {
		const fsoPath = route.paramMap.get('fsoPath');
		return this.browseService.GetSingle(fsoPath);
	}
}
