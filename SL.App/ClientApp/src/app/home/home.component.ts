import { Component } from '@angular/core';
import { Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { ApplicationStateSelector } from '../ngxs-core/application/application.selector';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  @Select(ApplicationStateSelector.getLoadingStatus) $loading: Observable<boolean>;
}
