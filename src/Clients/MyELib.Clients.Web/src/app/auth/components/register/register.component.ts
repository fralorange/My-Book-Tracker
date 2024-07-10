import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      username: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(100)])
    })
  }

  onSubmit() {
    if (this.registerForm.valid) {
      let email = this.registerForm.get('email')?.value;
      let username = this.registerForm.get('username')?.value;
      let password = this.registerForm.get('password')?.value;
      this.authService.register(email, username, password);
      this.router.navigateByUrl("");
    } else {
      alert("Ошибка! Электронная почта, имя пользователя или пароль введены неверено!");
    }
  }
}
