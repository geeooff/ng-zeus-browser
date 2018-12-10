import { Component, OnInit, Input } from '@angular/core';
import { Fso } from '../../../shared/interfaces/fso';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-children',
	templateUrl: './children.component.html',
	styleUrls: ['./children.component.scss']
})
export class ChildrenComponent implements OnInit {
	@Input() children: Fso[];

	constructor() { }

	ngOnInit() {

	}

}
