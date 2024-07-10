import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
      password: new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(100)])
    })
  }

  onSubmit() {
    if (this.loginForm.valid) {
      let username = this.loginForm.get('username')?.value;
      let password = this.loginForm.get('password')?.value;
      this.authService.login(username, password);
    } else {
      alert("Ошибка! Имя пользователя или пароль введены неверено!");
    }
  }
}
