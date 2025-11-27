import { AppModule } from './app/app.module';
import { enableProdMode, NgZone } from '@angular/core';
import { environment } from './environments/environment';
import { getSingleSpaExtraProviders, singleSpaAngular } from 'single-spa-angular';
import { initAppSettings } from '@viasoft/common';
import { NavigationStart, Router } from '@angular/router';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { singleSpaPropsSubject } from './single-spa/single-spa-props';

if (environment.production) {
  enableProdMode();
}

const lifecycles = singleSpaAngular({
  bootstrapFunction: singleSpaProps => {
    singleSpaPropsSubject.next(singleSpaProps);
    return initAppSettings(environment, true).then(() => {
      return platformBrowserDynamic(getSingleSpaExtraProviders()).bootstrapModule(AppModule)
        .catch(err => { throw err });
    });
  },
  template: '<app-root />',
  Router,
  NgZone,
  NavigationStart
});

export const bootstrap = lifecycles.bootstrap;
export const mount = lifecycles.mount;
export const unmount = lifecycles.unmount;
