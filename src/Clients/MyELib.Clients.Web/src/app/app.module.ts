import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideHttpClient } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './auth/components/login/login.component';
import { RegisterComponent } from './auth/components/register/register.component';
import { LibraryComponent } from './library/components/library/library.component';
import { EditUserComponent } from './user/components/edit-user/edit-user.component';
import { GetUserComponent } from './user/components/get-user/get-user.component';
import { GetLibraryComponent } from './library/components/get-library/get-library.component';
import { GetDocumentComponent } from './document/components/get-document/get-document.component';
import { EditLibraryComponent } from './library/components/edit-library/edit-library.component';
import { AddLibraryComponent } from './library/components/add-library/add-library.component';
import { EditDocumentComponent } from './document/components/edit-document/edit-document.component';
import { AddLibraryUserComponent } from './library-user/components/add-library-user/add-library-user.component';
import { LibraryUserComponent } from './library-user/components/library-user/library-user.component';
import { EditLibraryUserComponent } from './library-user/components/edit-library-user/edit-library-user.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    LibraryComponent,
    EditUserComponent,
    GetUserComponent,
    GetLibraryComponent,
    GetDocumentComponent,
    EditLibraryComponent,
    AddLibraryComponent,
    EditDocumentComponent,
    AddLibraryUserComponent,
    LibraryUserComponent,
    EditLibraryUserComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    ReactiveFormsModule
  ],
  providers: [
    provideHttpClient(),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
