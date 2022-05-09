import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplicationStateSelector } from '../ngxs-core/application/application.selector';

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
export class LoginDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<LoginDialogComponent>,
    private fb: FormBuilder
    ) { }

  loginForm: FormGroup;

  static openLoginDialog(dialog: MatDialog) {
    const dialogRef = dialog.open(LoginDialogComponent, {
      width: '450px',
      height: 'auto',
      disableClose: true
    });

    return dialogRef.afterClosed();
  }


  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.loginForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(256)]],
      email: ['', [Validators.required, Validators.maxLength(256), Validators.email]]
    });
  }

  submitForm() {
    if (this.loginForm.valid && this.loginForm.dirty) {
      this.dialogRef.close({
        userName: this.loginForm.controls['name'].value,
        userEmail: this.loginForm.controls['email'].value
      });
    }
  }

}
