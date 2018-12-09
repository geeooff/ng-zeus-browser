import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { BrowseModule } from './browse/browse.module';
import { environment } from '../environments/environment';

@NgModule({
	imports: [
		AppRoutingModule,
		BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
		HttpClientModule,
		HttpClientXsrfModule.withOptions({
			headerName: 'X-Zeus-Lightning',
			cookieName: 'Zeus-Lightning'
		}),
		SharedModule,
		CoreModule,
		BrowseModule
	],
	declarations: [
		AppComponent
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule {
	constructor(router: Router) {
		if (!environment.production) {
			console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
		}
	}
}
