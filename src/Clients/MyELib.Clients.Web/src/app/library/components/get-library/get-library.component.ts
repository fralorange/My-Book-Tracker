import { Component, OnInit } from '@angular/core';
import { LibraryService } from '../../services/library.service';
import { ActivatedRoute, Params } from '@angular/router';
import { LibraryModel } from '../../models/library.model';
import { DocumentService } from '../../../document/services/document.service';
import { UserService } from '../../../user/services/user.service';

@Component({
  selector: 'app-get-library',
  templateUrl: './get-library.component.html',
  styleUrl: './get-library.component.css'
})
export class GetLibraryComponent implements OnInit {
  private id = "";
  private userId = ""
  protected library!: LibraryModel

  constructor(private route: ActivatedRoute, private libraryService: LibraryService, private documentService: DocumentService, private userService: UserService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
      this.getLibraryById();
      this.userService.getCurrentUser()?.subscribe(result => this.userId = result.id);
    });
  }

  getLibraryById() {
    return this.libraryService.getLibraryById(this.id)
      ?.subscribe((result: LibraryModel) => this.library = result);
  }

  deleteDocument(id: string) {
    return this.documentService.deleteDocument(id)
      ?.subscribe(() => window.location.reload());
  }

  get isCurrentUserOwner(): boolean {
    return this.library.libraryUsers.some(lu => lu.userId == this.userId && lu.role > 1);
  }
}
