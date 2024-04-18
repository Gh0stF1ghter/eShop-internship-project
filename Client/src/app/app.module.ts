import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CatalogComponent } from './components/catalog/catalog/catalog.component';
import { TopBarComponent } from './components/top-bar/top-bar.component';
import { TypeDropdownComponent } from './components/top-bar/type-dropdown/type-dropdown.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ItemsDropdownComponent } from './components/top-bar/items-dropdown/items-dropdown.component';

@NgModule({
  declarations: [
    AppComponent,
    TopBarComponent,
    TypeDropdownComponent,
    CatalogComponent,
    RegisterComponent,
    LoginComponent,
    ItemsDropdownComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
