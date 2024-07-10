import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { LibraryService } from '../../services/library.service';
import { Router } from '@angular/router';
import { DocumentService } from '../../../document/services/document.service';
import { LibraryUserService } from '../../../library-user/services/library-user.service';

@Component({
  selector: 'app-add-library',
  templateUrl: './add-library.component.html',
  styleUrl: './add-library.component.css'
})
export class AddLibraryComponent implements OnInit {
  protected addForm!: FormGroup;

  constructor(private libraryService: LibraryService, private documentService: DocumentService, private libraryUserService: LibraryUserService, private router: Router) { }

  ngOnInit(): void {
    this.addForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
      documents: new FormArray([])
    })
  }

  onSubmit() {
    if (this.addForm.valid) {
      let name = this.addForm.get('name')?.value;
      let documents = this.addForm.get('documents')?.value;

      this.libraryService.postLibrary({ id: "", name: name, documents: [], libraryUsers: [] })
        ?.subscribe(libraryId => {
          this.libraryUserService.initLibraryUser(libraryId)
            ?.subscribe(() => {
              if (documents) {
                documents.forEach((file: any, index: number) => {
                  let documentData = new FormData();
                  documentData.append(`name`, `Документ №${index}`)
                  documentData.append(`libraryId`, libraryId)
                  documentData.append(`file`, file)
                  this.documentService.postDocument(documentData)?.subscribe();
                })
              }
              this.router.navigateByUrl("")
            });
        });

    } else {
      alert("Ошибка! Название библиотеки введено неверно!");
    }
  }

  onDocumentPicked(event: Event, fileInput: HTMLInputElement) {
    let files = fileInput.files;
    this.documentsArray.clear();
    if (!files || files?.length == 0) {
      return;
    }
    Array.from(files).forEach((file) => { this.documentsArray.push(new FormControl<File>(file)); });
  }

  get documentsArray(): FormArray {
    return this.addForm.get('documents') as FormArray;
  }
}
