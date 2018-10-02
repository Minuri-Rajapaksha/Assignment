import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { CounterComponent } from './components/counter/counter.component';
import { FetchDataComponent } from './components/fetch-data/fetch-data.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';

import { AuthService } from './../services/auth-service';
import { AuthGuardService } from './../services/auth-guard-service';
import { SigninOidcComponent } from './components/signin-oidc/signin-oidc.component';
import { StraightforwardViewComponent } from './components/straightforward-view/straightforward-view.component';
import { ReportViewComponent } from './components/report-view/report-view.component';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../services/api-service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    NavMenuComponent,
    SigninOidcComponent,
    StraightforwardViewComponent,
    ReportViewComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [AuthGuardService, AuthService, ApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
