import { Component } from '@angular/core';
import {ToolBarComponent} from '../components/tool-bar/tool-bar.component';
import {ContentComponent} from '../components/content/content.component';

@Component({
  selector: 'app-home',
  standalone: true,

  templateUrl: './home.component.html',
  imports: [
    ToolBarComponent,
    ContentComponent
  ],
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
