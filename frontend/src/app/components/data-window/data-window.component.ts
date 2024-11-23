import { Component } from '@angular/core';
import {VisualizedDataComponent} from "./visualized-data/visualized-data.component";

@Component({
  selector: 'app-data-window',
    imports: [
        VisualizedDataComponent
    ],
  templateUrl: './data-window.component.html',
  styleUrl: './data-window.component.scss'
})
export class DataWindowComponent {

}
