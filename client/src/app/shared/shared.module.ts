import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// ng-bootstrap
import {
	NgbAlertModule,
	NgbButtonsModule,
	NgbDropdownModule,
	NgbTooltipModule
} from '@ng-bootstrap/ng-bootstrap';

// font awesome
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { faBug } from '@fortawesome/free-solid-svg-icons';

// shared components
import { JumbotronComponent } from './components/jumbotron/jumbotron.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';

@NgModule({
	imports: [
		CommonModule,
		RouterModule,
		NgbAlertModule,
		NgbButtonsModule,
		NgbDropdownModule,
		NgbTooltipModule,
		FontAwesomeModule
	],
	declarations: [
		JumbotronComponent,
		BreadcrumbComponent
	],
	exports: [
		JumbotronComponent,
		BreadcrumbComponent
	]
})
export class SharedModule {
	constructor() {
		// font awesome icon library
		library.add(faBug);
	}
}
