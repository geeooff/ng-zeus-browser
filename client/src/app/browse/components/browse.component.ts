import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BrowseService } from '../../core/services/browse.service';
import { SingleResult } from '../../shared/interfaces/browse/single-result';
import { Fso } from '../../shared/interfaces/fso';
import { Subscription } from 'rxjs';

@Component({
	selector: 'app-browse',
	templateUrl: './browse.component.html',
	styleUrls: ['./browse.component.scss']
})
export class BrowseComponent implements OnInit, OnDestroy {

	// observables subscriptions
	private _singleResultSubscription: Subscription;
	private _siblingResultSubscription: Subscription;
	private _childrenResultSubscription: Subscription;

	// public observables data
	public fso: Fso;
	public ancestors: Fso[];
	public previous: Fso;
	public next: Fso;
	public children: Fso[];

	constructor(
		private router: Router,
		private activatedRoute: ActivatedRoute,
		private browseService: BrowseService
	) {
		// // get single fso result from resolver,
		// // and split/share this observable to two other ones
		// this._singleResult = this.activatedRoute.data.pipe(
		// 	map((data) => data['singleFsoResult'] as SingleResult),
		// 	share()
		// );
		// this.obsFso = this._singleResult.pipe(map((data) => data.fso));
		// this.obsAncestors = this._singleResult.pipe(map((data) => data.ancestors));

		// // get siblings observable, shared to next/previous observables
		// this._siblingResult = this.browseService.GetSiblings(fsoPath).pipe(share());
		// this.obsPrevious = this._siblingResult.pipe(map((data) => data.previous));
		// this.obsNext = this._siblingResult.pipe(map((data) => data.next));

		// // get children observable
		// this._childrenResult = this.browseService.GetChildren(fsoPath);
		// this.obsChildren = this._childrenResult.pipe(map((data) => data.children));
	}

	ngOnInit() {
		this.getSingleResult();
	}

	ngOnDestroy() {
		this._singleResultSubscription.unsubscribe();
		this._siblingResultSubscription.unsubscribe();
		this._childrenResultSubscription.unsubscribe();
	}

	private getSingleResult(): void {
		if (this._singleResultSubscription != null) {
			this._singleResultSubscription.unsubscribe();
		}

		// single fso result resolved from router resolver
		this._singleResultSubscription = this.activatedRoute.data.subscribe((data) => {
			const singleFsoResult = data['singleFsoResult'] as SingleResult;
			this.fso = singleFsoResult.fso;
			this.ancestors = singleFsoResult.ancestors;

			// related siblings and children
			this.getSiblingsResult(this.fso.path);
			this.getChildrenResult(this.fso.path);
		});
	}

	private getSiblingsResult(fsoPath: string) {
		if (this._siblingResultSubscription != null) {
			this._siblingResultSubscription.unsubscribe();
		}

		// siblings
		this._siblingResultSubscription = this.browseService.GetSiblings(fsoPath).subscribe((data) => {
			this.previous = data.previous;
			this.next = data.next;
		});
	}

	private getChildrenResult(fsoPath: string) {
		if (this._childrenResultSubscription != null) {
			this._childrenResultSubscription.unsubscribe();
		}

		// siblings
		this._childrenResultSubscription = this.browseService.GetChildren(fsoPath).subscribe((data) => {
			this.children = data.children;
		});
	}
}
