import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { VsAuthorizationModule } from '@viasoft/authorization-management';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import {
  VsAuthorizationStorageLegacyService,
  VsCommonModule,
} from '@viasoft/common';
import { API_GATEWAY, VsHttpModule } from '@viasoft/http';
import { VsAppCoreModule } from '@viasoft/app-core';
import { VsNavigationViewModule } from '@viasoft/navigation';

import { ComponentsModule } from './components/components.module';
import { AppComponent } from './app.component';
import { environment } from './../environments/environment';
import { AppConsts } from './tokens/consts/app-consts.const';
import { NAVIGATION_MENU_ITEMS } from './tokens/consts/navigation.const';
import { CONCILIACAO_CONTABIL_APP_I18N_EN } from './tokens/i18n/consts/conciliacao-contabil-app-i18n-en.const';
import { CONCILIACAO_CONTABIL_APP_I18N_PT } from './tokens/i18n/consts/conciliacao-contabil-app-i18n-pt.const';

const routes = [
  {
    path: 'conciliacoes',
    loadChildren: () =>
      import('./pages/home/home.module').then((m) => m.HomeModule),
  },
  {
    path: 'configuracoes',
    loadChildren: () =>
      import('./pages/config-conciliacoes/config-conciliacoes.module').then((m) => m.ConfigConciliacoesModule),
  },
  {
    path: '**', redirectTo: 'conciliacoes'
  }
];

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    ComponentsModule,
    BrowserAnimationsModule,
    VsCommonModule,
    VsHttpModule.forRoot({
      environment: environment,
      isCompanyBased: false,
    }),
    VsAppCoreModule.forRoot({
      formLayout: 'vertical',
      portalConfig: {
        appId: 'CON21_W',
        appName: 'conciliacao-contabil',
        domain: 'Accounting',
        navbarTitle: 'ConciliacaoContabilApp.Navigation.Title',
      },
      translates: {
        en: CONCILIACAO_CONTABIL_APP_I18N_EN,
        pt: CONCILIACAO_CONTABIL_APP_I18N_PT,
      },
      apiPrefix: 'contabilidade/conciliacao-contabil',
      environment: environment,
      navigation: NAVIGATION_MENU_ITEMS,
      customServices: {
        authorizationStorageService: VsAuthorizationStorageLegacyService,
      },
    }),
    VsNavigationViewModule,
    VsAuthorizationModule,
  ],
  providers: [
    {
      provide: API_GATEWAY,
      useFactory: AppConsts.apiGateway,
    },
    {
      provide: VS_BACKEND_URL,
      useFactory: AppConsts.apiGateway,
    },
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
