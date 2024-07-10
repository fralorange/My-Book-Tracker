import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { LibraryService } from '../../services/library.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { LibraryModel } from '../../models/library.model';

@Component({
  selector: 'app-edit-library',
  templateUrl: './edit-library.component.html',
  styleUrl: './edit-library.component.css'
})
export class EditLibraryComponent implements OnInit {
  private id: string = '';
  private library!: LibraryModel
  protected editForm!: FormGroup;

  constructor(private route: ActivatedRoute, private libraryService: LibraryService, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
      this.getLibraryById();
    });
    this.editForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)])
    })
  }

  onSubmit() {
    if (this.editForm.valid) {
      let name = this.editForm.get('name')?.value;

      this.libraryService.putLibrary({ id: this.id, name: name, documents: this.library.documents, libraryUsers: this.library.libraryUsers })
        ?.subscribe(() => this.router.navigateByUrl(""));
    } else {
      alert("Ошибка! Название библиотеки введено неверно!");
    }
  }

  getLibraryById() {
    return this.libraryService.getLibraryById(this.id)
      ?.subscribe((result: LibraryModel) => this.library = result);
  }
}
