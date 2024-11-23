import { Component } from '@angular/core';
import {ToolBarComponent} from '../components/tool-bar/tool-bar.component';
import { QueryBarComponent } from "../components/query-bar/query-bar.component";
import {DataWindowComponent} from '../components/data-window/data-window.component';

@Component({
  selector: 'app-home',
  standalone: true,

  templateUrl: './home.component.html',
  imports: [
    ToolBarComponent,
    QueryBarComponent,
    DataWindowComponent
  ],
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
