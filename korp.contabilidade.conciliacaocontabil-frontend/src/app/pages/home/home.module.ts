import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';

import { VsCommonModule } from '@viasoft/common';
import {
  VsHeaderModule as VsHeader,
  VsButtonModule as VsButton,
  VsSelectModule as VsSelect,
  VsIconModule as VsIcon,
  VsLabelModule as VsLabel,
  VsGridModule as VsGrid,
  VsDialogModule as VsDialog,
  VsDatepickerModule as VsDatepicker,
  VsInputModule as VsInput,
  VsCheckboxModule as VsCheckbox,
  VsLayoutModule,
  VsFormModule,
  VsTreeViewModule,
  VsSpinnerModule,
} from '@viasoft/components';

import { VsDrawerComponent } from '@viasoft/components/drawer'
import { VsNavbarModule as VsNavbar } from '@viasoft/navigation';

import { ComponentsModule } from '../../components/components.module';
import { Home } from './home.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AddPeriodoComponent } from './add-periodo/add-periodo.component';
import { CONCILIACAO_CONTABIL_PT } from './i18n/pt';
import { CONCILIACAO_CONTABIL_EN } from './i18n/en';
import { StatusErrorComponent } from './status-error/status-error.component';

const routes = [
  {
    path: '',
    component: Home,
  },
];

@NgModule({
  declarations: [
    Home,
    AddPeriodoComponent,
    StatusErrorComponent
  ],
  imports: [
    ComponentsModule,
    MatSidenavModule,
    RouterModule.forChild(routes),
    VsCommonModule.forChild({
      translates: {
        pt: CONCILIACAO_CONTABIL_PT,
        en: CONCILIACAO_CONTABIL_EN
      }
    }),
    VsButton,
    VsCheckbox,
    VsCommonModule,
    VsDatepicker,
    VsDialog,
    VsFormModule,
    VsGrid,
    VsHeader,
    VsIcon,
    VsInput,
    VsLabel,
    VsLayoutModule,
    VsDrawerComponent,
    VsNavbar,
    VsSelect,
    VsSpinnerModule,
    VsTreeViewModule
  ],
  exports: [Home],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class HomeModule { }
