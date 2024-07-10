import { Component, OnInit } from '@angular/core';
import { DocumentService } from '../../services/document.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-edit-document',
  templateUrl: './edit-document.component.html',
  styleUrl: './edit-document.component.css'
})
export class EditDocumentComponent implements OnInit {
  private id: string = '';
  protected editForm!: FormGroup;

  constructor(private route: ActivatedRoute, private documentService: DocumentService, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
    });
    this.editForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)])
    })
  }

  onSubmit() {
    if (this.editForm.valid) {
      let name = this.editForm.get('name')?.value;

      this.documentService.patchDocument(this.id, name)
        ?.subscribe(() => this.router.navigateByUrl(""));
    } else {
      alert("Ошибка! Название библиотеки введено неверно!");
    }
  }
}
