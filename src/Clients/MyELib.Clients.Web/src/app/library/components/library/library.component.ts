import { Component } from '@angular/core';
import { LibraryModel } from '../../models/library.model';
import { LibraryService } from '../../services/library.service';
import { TokenService } from '../../../auth/services/token.service';
import { PaginationData } from '../../pagination/pagination.data';

@Component({
  selector: 'app-library',
  templateUrl: './library.component.html',
  styleUrl: './library.component.css'
})
export class LibraryComponent {
  libraries: LibraryModel[] = [];
  paginationData: PaginationData = {
    currentPage: 0,
    pageCount: 0,
    pageSize: 0,
    totalCount: 0,
  }
  currentPage: number = 1;
  pageSize: number = 10;
  maxPagesToShow: number = 9;

  constructor(private libraryService: LibraryService, private tokenService: TokenService) { }

  ngOnInit(): void {
    if (!this.tokenService.getToken()) {
      return;
    }

    this.loadLibraries();
  }

  loadLibraries(): void {
    let loadedLibraries = this.libraryService.getLibraries(this.currentPage, this.pageSize);
    if (!loadedLibraries) {
      // обработать ошибку 401
    } else {
      loadedLibraries.subscribe((result: LibraryModel[]) => {
        this.libraries = result;
        this.paginationData = this.libraryService.getPaginationData();
      });
    }
  }

  deleteLibrary(id: string) : void {
    this.libraryService.deleteLibraryById(id)?.subscribe(() => {
      window.location.reload();
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadLibraries();
  }

  get pagesToShow(): number[] {
    const pageCount = this.paginationData.pageCount;
    const maxPages = Math.min(pageCount, this.maxPagesToShow);
    const startPage = Math.max(1, this.currentPage - Math.floor(maxPages / 2));
    const endPage = Math.min(pageCount, startPage + maxPages - 1);
    const pages = [];
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }
}
