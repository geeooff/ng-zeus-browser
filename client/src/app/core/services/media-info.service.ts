import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { DEFAULTS, IDefaults } from '../../../defaults/defaults';

@Injectable({
	providedIn: 'root'
})
export class MediaInfoService {
	constructor(
		@Inject(DEFAULTS) private defaults: IDefaults,
		@Inject('BASE_URL') private baseUrl: string,
		private http: HttpClient
	) {
		baseUrl += 'api/mediainfo';
	}

	public GetText(path: string = this.defaults.path): Observable<string> {
		return this.http.get(
			`${this.baseUrl}/text/${path}`,
			{ responseType: 'text' }
		);
	}

	public GetXML(path: string = this.defaults.path): Observable<string> {
		return this.http.get(
			`${this.baseUrl}/xml/${path}`,
			{ responseType: 'text' }
		);
	}

	// TODO strong-type
	public GetJson(path: string = this.defaults.path): Observable<any> {
		return this.http.get(
			`${this.baseUrl}/json/${path}`,
			{ responseType: 'json' }
		);
	}

	public GetHtml(path: string = this.defaults.path): Observable<string> {
		return this.http.get(
			`${this.baseUrl}/html/${path}`,
			{ responseType: 'text' }
		);
	}
}
