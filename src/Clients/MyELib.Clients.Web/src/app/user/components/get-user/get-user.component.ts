import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserModel } from '../../models/user.model';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-get-user',
  templateUrl: './get-user.component.html',
  styleUrl: './get-user.component.css'
})
export class GetUserComponent implements OnInit {
  user!: UserModel;

  constructor(private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
    let currentUser = this.userService.getCurrentUser();
    if (!currentUser) {
      // обработать ошибку
    }
    currentUser?.subscribe((result: UserModel) => this.user = result);
  }

  deleteCurrentUser() {
    this.userService.deleteUser(this.user.id);
    this.logout();
  }

  logout() {
    this.authService.logout();
  }
}
