import { Component, OnInit, Input } from '@angular/core';
import { Fso } from '../../interfaces/fso';

@Component({
	selector: 'app-breadcrumb',
	templateUrl: './breadcrumb.component.html',
	styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {
	@Input() fso: Fso;
	@Input() ancestors: Fso[];

	constructor() { }

	ngOnInit() {
	}

}
