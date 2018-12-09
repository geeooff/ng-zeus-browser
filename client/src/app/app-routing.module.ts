import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

const appRoutes: Routes = [
	{ path: '', redirectTo: '/browse', pathMatch: 'full' }
];

@NgModule({
	imports: [
		RouterModule.forRoot(
			appRoutes,
			{
				// enableTracing: true,
				preloadingStrategy: PreloadAllModules
			}
		)
	],
	exports: [RouterModule]
})
export class AppRoutingModule { }
