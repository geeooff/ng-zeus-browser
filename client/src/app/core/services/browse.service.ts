import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { DEFAULTS, IDefaults } from '../../../defaults/defaults';
import { SingleResult } from '../../shared/interfaces/browse/single-result';
import { ChildrenResult } from '../../shared/interfaces/browse/children-result';
import { SiblingsResult } from '../../shared/interfaces/browse/siblings-result';
import { DescendantsResult } from '../../shared/interfaces/browse/descendants-result';
import { GroupBy } from '../../shared/enums/group-by.enum';
import { OrderBy } from '../../shared/enums/order-by.enum';

@Injectable({
	providedIn: 'root'
})
export class BrowseService {

	constructor(
		@Inject(DEFAULTS) private defaults: IDefaults,
		@Inject('BASE_URL') private baseUrl: string,
		private http: HttpClient
	) {
		this.baseUrl += 'api/browse';
	}

	public GetSingle(
		path: string = this.defaults.path
	): Observable<SingleResult> {
		return this.http.get<SingleResult>(
			`${this.baseUrl}/single${path}`
		);
	}

	public GetSiblings(
		path: string = this.defaults.path,
		groupBy: GroupBy = this.defaults.groupBy,
		orderBy: OrderBy = this.defaults.orderBy,
	): Observable<SiblingsResult> {
		return this.http.get<SiblingsResult>(
			`${this.baseUrl}/siblings${path}`,
			{ params: this.getParams(groupBy, orderBy) }
		);
	}

	public GetChildren(
		path: string = this.defaults.path,
		groupBy: GroupBy = this.defaults.groupBy,
		orderBy: OrderBy = this.defaults.orderBy,
	): Observable<ChildrenResult> {
		return this.http.get<ChildrenResult>(
			`${this.baseUrl}/children${path}`,
			{ params: this.getParams(groupBy, orderBy) }
		);
	}

	public GetDescendants(
		path: string = this.defaults.path,
		groupBy: GroupBy = this.defaults.groupBy,
		orderBy: OrderBy = this.defaults.orderBy,
	): Observable<DescendantsResult> {
		return this.http.get<DescendantsResult>(
			`${this.baseUrl}/descendants${path}`,
			{ params: this.getParams(groupBy, orderBy) }
		);
	}

	private getParams(groupBy: GroupBy, orderBy: OrderBy) {

		// ordering parameters
		const params = new HttpParams()
			.set('groupBy', groupBy.toString())
			.set('orderBy', orderBy.toString());

		return params;
	}
}
