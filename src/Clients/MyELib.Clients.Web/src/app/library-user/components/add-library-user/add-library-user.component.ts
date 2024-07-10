import { Component, OnInit } from '@angular/core';
import { LibraryUserService } from '../../services/library-user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-add-library-user',
  templateUrl: './add-library-user.component.html',
  styleUrl: './add-library-user.component.css'
})
export class AddLibraryUserComponent implements OnInit {
  private id = "";
  protected editForm!: FormGroup;

  constructor(private route: ActivatedRoute, private libraryUserService: LibraryUserService, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params['id'];
    });
    this.editForm = new FormGroup({
      id: new FormControl('', [Validators.required]),
      role: new FormControl(0, [Validators.required])
    })
  }

  onSubmit() {
    if (this.editForm.valid) {
      let userId = this.editForm.get('id')?.value;
      let role = +this.editForm.get('role')?.value;

      this.libraryUserService.grantAccessToUser(this.id, userId, role)
        ?.subscribe(() => this.router.navigateByUrl(""));
    } else {
      alert("Ошибка!");
    }
  }
}
