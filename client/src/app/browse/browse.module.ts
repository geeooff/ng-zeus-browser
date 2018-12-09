import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowseRoutingModule } from './browse-routing.module';
import { BrowseComponent } from './browse/browse.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
	imports: [
		CommonModule,
		BrowseRoutingModule,
		SharedModule
	],
	declarations: [BrowseComponent],
	exports: []
})
export class BrowseModule { }
