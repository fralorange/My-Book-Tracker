import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { UserModel } from '../../models/user.model';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent implements OnInit {
  editForm!: FormGroup;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.editForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(100)])
    })
  }

  onSubmit() {
    if (this.editForm.valid) {
      let email = this.editForm.get('email')?.value;
      let password = this.editForm.get('password')?.value;

      var currentUser = this.userService.getCurrentUser()
        ?.subscribe((result: UserModel) => this.userService.putUser({ id: result.id, email: email, username: result.username}, password));

      currentUser

      this.router.navigateByUrl("");
    } else {
      alert("Ошибка! Электронная почта, имя пользователя или пароль введены неверено!");
    }
  }
}
