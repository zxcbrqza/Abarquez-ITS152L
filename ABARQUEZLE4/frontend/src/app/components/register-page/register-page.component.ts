import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.css'
})
export class RegisterPageComponent implements OnInit{
  form: any = {
    username: null,
    password: null,
    firstName: null,
    lastName: null,
  }
  errorMessage: string | null = null;

  constructor(private http: HttpClient,
    private route: Router,) {
     }
  
    ngOnInit(): void {
    }

    onSubmit(): void {
      const {
        username, password, firstName, lastName
      } = this.form

      console.log(this.form);

      this.http.post("http://localhost:5090/api/Login/register", this.form, {responseType:
        'text'}).subscribe(data => {
          if (window.confirm('Registration successful! Click OK to proceed to login.')) {
            this.route.navigate(['/login']);
          }
        }, error => {
          console.error('Login error:', error);
          this.errorMessage = "Please fill out all required fields.";
        });
    }
}
