import { Component, OnInit, Input } from '@angular/core';
import { Fso } from '../../interfaces/fso';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-jumbotron',
	templateUrl: './jumbotron.component.html',
	styleUrls: ['./jumbotron.component.scss']
})
export class JumbotronComponent implements OnInit {
	@Input() fso: Fso;
	@Input() ancestors: Fso[];

	constructor() { }

	ngOnInit() {
	}

}
