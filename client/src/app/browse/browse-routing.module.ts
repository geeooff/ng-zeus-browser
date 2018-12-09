import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowseComponent } from './browse/browse.component';
import { SingleFsoResolverService } from '../core/services/single-fso-resolver.service';

const appRoutes: Routes = [
	{
		path: 'browse/:path',
		component: BrowseComponent,
		resolve: {
			fsoResult: SingleFsoResolverService
		}
	}
];

@NgModule({
	imports: [
		RouterModule.forChild(appRoutes)
	]
})
export class BrowseRoutingModule { }
