import { Component } from '@angular/core';
import { TokenService } from './auth/services/token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'My E-Lib';

  constructor(protected tokenService: TokenService) { }
}
