import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowseComponent } from './components/browse.component';
import { SingleFsoResolverService } from '../core/services/single-fso-resolver.service';

const appRoutes: Routes = [
	{
		path: 'browse/:fsoPath',
		component: BrowseComponent,
		resolve: {
			singleFsoResult: SingleFsoResolverService
		}
	}
];

@NgModule({
	imports: [
		RouterModule.forChild(appRoutes)
	]
})
export class BrowseRoutingModule { }
