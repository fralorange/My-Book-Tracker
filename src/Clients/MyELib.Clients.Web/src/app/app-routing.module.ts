import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LibraryComponent } from './library/components/library/library.component';
import { LoginComponent } from './auth/components/login/login.component';
import { RegisterComponent } from './auth/components/register/register.component';
import { AuthGuard } from './auth/guards/auth.guard';
import { GetUserComponent } from './user/components/get-user/get-user.component';
import { EditUserComponent } from './user/components/edit-user/edit-user.component';
import { GetLibraryComponent } from './library/components/get-library/get-library.component';
import { GetDocumentComponent } from './document/components/get-document/get-document.component';
import { EditLibraryComponent } from './library/components/edit-library/edit-library.component';
import { AddLibraryComponent } from './library/components/add-library/add-library.component';
import { EditDocumentComponent } from './document/components/edit-document/edit-document.component';
import { AddLibraryUserComponent } from './library-user/components/add-library-user/add-library-user.component';
import { LibraryUserComponent } from './library-user/components/library-user/library-user.component';
import { EditLibraryUserComponent } from './library-user/components/edit-library-user/edit-library-user.component';

const routes: Routes = [
  { path: "", component: LibraryComponent },
  { path: "library/add", component: AddLibraryComponent },
  { path: "library/:id", component: GetLibraryComponent },
  { path: "library/edit/:id", component: EditLibraryComponent },
  { path: "login", component: LoginComponent, canActivate: [AuthGuard], providers: [AuthGuard] },
  { path: "register", component: RegisterComponent, canActivate: [AuthGuard], providers: [AuthGuard] },
  { path: "user", component: GetUserComponent},
  { path: "user/edit", component: EditUserComponent},
  { path: "document/:id", component: GetDocumentComponent},
  { path: "document/edit/:id", component: EditDocumentComponent},
  { path: "libraryUser/:id", component: LibraryUserComponent},
  { path: "libraryUser/grant/:id", component: AddLibraryUserComponent},
  { path: "libraryUser/edit/:id", component: EditLibraryUserComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
