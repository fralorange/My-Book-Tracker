import { Component, OnInit } from '@angular/core';
import { LibraryUserModel } from '../../models/library-user.model';
import { ActivatedRoute, Params } from '@angular/router';
import { LibraryModel } from '../../../library/models/library.model';
import { LibraryService } from '../../../library/services/library.service';
import { LibraryUserService } from '../../services/library-user.service';

@Component({
  selector: 'app-library-user',
  templateUrl: './library-user.component.html',
  styleUrl: './library-user.component.css'
})
export class LibraryUserComponent implements OnInit {
  protected libraryUsers: LibraryUserModel[] = [];
  private id = "";

  constructor(private route: ActivatedRoute, private libraryService: LibraryService, private libraryUserService: LibraryUserService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
      this.getLibraryUsersByLibraryId();
    });
  }

  getLibraryUsersByLibraryId() {
    return this.libraryService.getLibraryById(this.id)
      ?.subscribe((result: LibraryModel) => this.libraryUsers = result.libraryUsers);
  }

  deleteLibraryUser(id: string) {
    return this.libraryUserService.deleteLibraryUser(id)
      ?.subscribe(() => window.location.reload());
  }

  roleNumberToString(role: number) {
    switch (role) {
      case 0:
        return "Читатель";
      case 1:
        return "Писатель";
      default:
        return "Владелец";
    }
  }
}
