import { Component } from '@angular/core';
import {ToolBarComponent} from '../components/tool-bar/tool-bar.component';
import {SearchBarComponent} from '../components/search-bar/search-bar.component';
import {ContentComponent} from '../components/content/content.component';

@Component({
  selector: 'app-home',
  standalone: true,

  templateUrl: './home.component.html',
  imports: [
    ToolBarComponent,
    SearchBarComponent,
    ContentComponent
  ],
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
