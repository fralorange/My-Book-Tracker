import { Component, OnInit } from '@angular/core';
import { LibraryUserService } from '../../services/library-user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-edit-library-user',
  templateUrl: './edit-library-user.component.html',
  styleUrl: './edit-library-user.component.css'
})
export class EditLibraryUserComponent implements OnInit {
  private id = "";
  protected editForm!: FormGroup;

  constructor(private route: ActivatedRoute, private libraryUserService: LibraryUserService, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
    });
    this.editForm = new FormGroup({
      role: new FormControl(0, [Validators.required])
    })
  }

  onSubmit() {
    if (this.editForm.valid) {
      let role = +this.editForm.get('role')?.value;

      this.libraryUserService.getLibraryUserById(this.id)?.subscribe((libUser) => {
        this.libraryUserService.patchLibraryUser(this.id, libUser.libraryId, role)
          ?.subscribe(() => this.router.navigateByUrl(""));
      });
      
    } else {
      alert("Ошибка!");
    }
  }
}
