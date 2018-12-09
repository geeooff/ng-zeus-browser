import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowseComponent } from './browse/browse.component';

const appRoutes: Routes = [
	{
		path: 'browse',
		component: BrowseComponent
	}
];

@NgModule({
	imports: [
		RouterModule.forChild(appRoutes)
	]
})
export class BrowseRoutingModule { }
