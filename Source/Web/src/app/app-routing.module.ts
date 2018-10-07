import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home/home.component';
import { SigninOidcComponent } from './components/signin-oidc/signin-oidc.component';
import { AuthGuardService } from './../services/auth-guard-service';
import { ReportViewComponent } from './components/report-view/report-view.component';
import { StraightforwardViewComponent } from './components/straightforward-view/straightforward-view.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuardService] },
  { path: 'signin-oidc', component: SigninOidcComponent },
  { path: 'straightforward-view', component: StraightforwardViewComponent, canActivate: [AuthGuardService] },
  { path: 'report-view', component: ReportViewComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
