import { Component, OnInit } from '@angular/core';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplicationStateSelector } from './ngxs-core/application/application.selector';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  @Select(ApplicationStateSelector.getLoggedInStatus) $loggedIn: Observable<boolean>;

  constructor(
    ) {}

  ngOnInit() {
  }
}
