import { NgModule } from '@angular/core';
import { ConfigConciliacoesComponent } from './config-conciliacoes.component';
import { VsCommonModule } from '@viasoft/common';
import { RouterModule } from '@angular/router';

import {
  VsLabelModule,
  VsGridModule,
  VsLayoutModule,
  VsIconModule,
  VsSelectModule,
  VsInputModule,
  VsFormModule,
  VsButtonModule,
  VsHeaderModule
} from '@viasoft/components';
import { CONCILIACAO_CONFIG_PT } from './i18n/pt';
import { CONCILIACAO_CONFIG_EN } from './i18n/en';

const routes = [
  {
    path: '',
    component: ConfigConciliacoesComponent,
  },
];

@NgModule({
  declarations: [ConfigConciliacoesComponent],
  imports: [
    RouterModule.forChild(routes),
    VsCommonModule.forChild({
      translates: {
        pt: CONCILIACAO_CONFIG_PT,
        en: CONCILIACAO_CONFIG_EN
      }
    }),
    VsButtonModule,
    VsFormModule,
    VsGridModule,
    VsHeaderModule,
    VsIconModule,
    VsInputModule,
    VsLabelModule,
    VsLayoutModule,
    VsSelectModule
  ]
})
export class ConfigConciliacoesModule { }
