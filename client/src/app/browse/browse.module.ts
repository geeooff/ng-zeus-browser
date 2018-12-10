import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SharedModule } from '../shared/shared.module';
import { BrowseRoutingModule } from './browse-routing.module';
import { BrowseComponent } from './components/browse.component';
import { ChildrenComponent } from './containers/children/children.component';

@NgModule({
	imports: [
		CommonModule,
		RouterModule,
		FontAwesomeModule,
		SharedModule,
		BrowseRoutingModule
	],
	declarations: [BrowseComponent, ChildrenComponent],
	exports: []
})
export class BrowseModule { }
