import { Component } from '@angular/core';

import { HeaderComponent } from 'src/app/components-header/header/header.component';
import { NavComponent } from 'src/app/components-header/nav/nav.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'WebServer';
}
