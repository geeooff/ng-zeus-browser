import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { DEFAULTS, IDefaults } from '../../../defaults/defaults';
import { GroupBy } from '../../shared/enums/group-by.enum';
import { OrderBy } from '../../shared/enums/order-by.enum';

@Injectable({
	providedIn: 'root'
})
export class PlaylistService {
	constructor(
		@Inject(DEFAULTS) private defaults: IDefaults,
		@Inject('BASE_URL') private baseUrl: string,
		private http: HttpClient
	) {
		this.baseUrl += 'api/playlist';
	}

	public GetM3U(
		path: string = this.defaults.path,
		groupBy: GroupBy = this.defaults.groupBy,
		orderBy: OrderBy = this.defaults.orderBy,
	): Observable<string> {
		return this.http.get(
			`${this.baseUrl}/m3u${path}`,
			{
				responseType: 'text',
				params: this.getParams(groupBy, orderBy)
			}
		);
	}

	public GetASX(
		path: string = this.defaults.path,
		groupBy: GroupBy = this.defaults.groupBy,
		orderBy: OrderBy = this.defaults.orderBy,
	): Observable<string> {
		return this.http.get(
			`${this.baseUrl}/asx${path}`,
			{
				responseType: 'text',
				params: this.getParams(groupBy, orderBy)
			}
		);
	}

	public GetUrls(
		path: string = this.defaults.path,
		groupBy: GroupBy = this.defaults.groupBy,
		orderBy: OrderBy = this.defaults.orderBy,
	): Observable<string> {
		return this.http.get(
			`${this.baseUrl}/urls${path}`,
			{
				responseType: 'text',
				params: this.getParams(groupBy, orderBy)
			}
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
