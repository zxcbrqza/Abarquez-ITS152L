import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service'
import { TokenStorageService } from '../../services/token-storage.service'
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent implements OnInit{
  form: any = {
    username: null,
    password: null
  }
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private http: HttpClient,
    private router: Router) { }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.authService.isLoggedIn = true;
      this.router.navigate([this.authService.redirectUrl]);
    }
  }

  onSubmit() {
    const {username, password} = this.form;

    this.http.post<LoginPostData>("http://localhost:5090/api/Login/login", { username,
      password }). subscribe(data => {
        this.tokenStorage.saveToken(data.id_token);
        this.tokenStorage.saveUser(data.id);

        this.authService.isLoggedIn = true;

        this.router.navigate([this.authService.redirectUrl]);
        alert('Login successful! Click OK to proceed.');
        window.location.reload();
      }, error => {
        console.error('Login error:', error);
        this.errorMessage = "Invalid username or password";
      });
  }
}

export interface LoginPostData {
  id_token: string;
  id: number;
}
