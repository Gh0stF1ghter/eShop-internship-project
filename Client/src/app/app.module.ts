import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { RouterModule } from '@angular/router';
import { HeroComponent } from './hero/hero.component';
import { TypeDropdownComponent } from './top-bar/type-dropdown/type-dropdown.component';
import { CatalogComponent } from './catalog/catalog/catalog.component';

@NgModule({
  declarations: [
    AppComponent,
    TopBarComponent,
    TypeDropdownComponent,
    CatalogComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
